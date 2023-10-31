using System.Text;

namespace SaveFileManager
{
    /// <summary>
    /// Object for displaying an options UI using the <c>Display</c> function.<br/>
    /// Prints the <c>title</c> and then the list of elements that the user can cycle between with the selected keys (arrow keys by default) and interact with them.
    /// </summary>
    public class OptionsUI
    {
        #region Public fields
        /// <summary>
        /// The list of <c>BaseUI</c> objects to use.
        /// </summary>
        public IEnumerable<BaseUI?> elements;
        /// <summary>
        /// The string to print before the <c>elements</c>.
        /// </summary>
        public string? title;
        /// <summary>
        /// The cursor icon style to use.
        /// </summary>
        public CursorIcon cursorIcon;
        /// <summary>
        /// Allows the user to press the key associated with escape, to exit the menu.
        /// </summary>
        public bool canEscape;
        /// <summary>
        /// Whether to pass in the object into the element's functions.
        /// </summary>
        public bool passInObject;
        /// <summary>
        /// The settings for the scrolling of UI elements.
        /// </summary>
        public ScrollSettings scrollSettings;
        /// <summary>
        /// The index of the currently selected element in the list.
        /// </summary>
        public int selected;
        /// <summary>
        /// The start index of the currently displayed section of the elements list.
        /// </summary>
        public int startIndex;
        /// <summary>
        /// The text to display, to clear the screen.
        /// </summary>
        public string clearScreenText;
        #endregion

        #region Public constructors
        /// <summary>
        /// <inheritdoc cref="OptionsUI"/>
        /// </summary>
        /// <param name="elements"><inheritdoc cref="elements" path="//summary"/></param>
        /// <param name="title"><inheritdoc cref="title" path="//summary"/></param>
        /// <param name="cursorIcon"><inheritdoc cref="cursorIcon" path="//summary"/></param>
        /// <param name="canEscape"><inheritdoc cref="canEscape" path="//summary"/></param>
        /// <param name="passInObject"><inheritdoc cref="passInObject" path="//summary"/></param>
        /// <param name="scrollSettings"><inheritdoc cref="scrollSettings" path="//summary"/></param>
        /// <param name="clearScreenText"><inheritdoc cref="clearScreenText" path="//summary"/><br/>
        /// By default, it's 70 newlines (faster than actualy clearing the screen).</param>
        /// <exception cref="UINoSelectablesExeption">Exceptions thrown, if there are no selectable UI elements in the list.</exception>
        public OptionsUI(
            IEnumerable<BaseUI?> elements,
            string? title = null,
            CursorIcon? cursorIcon = null,
            bool canEscape = true,
            bool passInObject = true,
            ScrollSettings? scrollSettings = null,
            string? clearScreenText = null)
        {
            if (elements.All(answer => answer is null || !answer.IsSelectable))
            {
                throw new UINoSelectablesExeption();
            }
            this.elements = elements;
            this.title = title;
            this.cursorIcon = cursorIcon ?? new CursorIcon();
            this.canEscape = canEscape;
            this.passInObject = passInObject;
            this.scrollSettings = scrollSettings ?? new ScrollSettings();
            this.clearScreenText = clearScreenText ?? "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
        }
        #endregion

        /// <summary>
        /// Prints the title and then a list of elements that the user can cycle between with the up and down arrows, and adjust with either the left and right arrow keys or the enter pressedKey depending on the input object type, and exit with the pressedKey assigned to escape.<br/>
        /// if an element in the list is null, the line will be blank and cannot be selected.
        /// </summary>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use, if the selected action is a <c>UIList</c>.</param>
        /// <param name="keyResults">The list of posible results returned by pressing a key.<br/>
        /// The order of the elements in the list should be:<br/>
        /// - escape, up, down, left, right, enter<br/>
        ///If it is null, the default value is either returned from the <c>keybinds</c> or:<br/>
        /// - { Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER }</param>
        /// <exception cref="UINoSelectablesExeption">Exceptions thrown, if there are no selectable UI elements in the list.</exception>
        public object? Display(IEnumerable<KeyAction>? keybinds = null, IEnumerable<object>? keyResults = null)
        {
            // no selectable element
            if (elements.All(element => element is null || !element.IsSelectable))
            {
                throw new UINoSelectablesExeption();
            }

            // keybinds
            if (keyResults is null || keyResults.Count() < 6)
            {
                if (keybinds is null)
                {
                    keyResults = new List<object> { Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER };
                }
                else
                {
                    keyResults = Utils.GetResultsList(keybinds);
                }
            }
            cursorIcon ??= new CursorIcon();

            // is enter needed?
            var enterKeyNeeded = elements.Any(element => element is not null && element.IsClickable);
            // put selected on selectable
            selected = 0;
            while (
                elements.ElementAt(selected)?.IsSelectable != true
            )
            {
                selected++;
            }

            startIndex = Math.Clamp(0, selected - scrollSettings.scrollUpMargin, elements.Count() - 1);
            int endIndex;

            // render/getkey loop
            object pressedKey;
            do
            {
                // prevent infinite loop
                if (elements.All(answer => answer is null || !answer.IsSelectable))
                {
                    throw new UINoSelectablesExeption();
                }

                // clear screen + render
                var txt = new StringBuilder(clearScreenText);

                // title
                if (title is not null)
                {
                    txt.Append(title + "\n\n");
                }

                // elements
                if (scrollSettings.maxElements == -1 || scrollSettings.maxElements >= elements.Count())
                {
                    startIndex = 0;
                    endIndex = elements.Count();
                }
                else
                {
                    if (startIndex > selected - scrollSettings.scrollUpMargin)
                    {
                        startIndex = selected - scrollSettings.scrollUpMargin;
                    }
                    if (startIndex + scrollSettings.maxElements - 1 < selected + scrollSettings.scrollDownMargin)
                    {
                        startIndex = selected + scrollSettings.scrollDownMargin - (scrollSettings.maxElements - 1);
                    }

                    startIndex = Math.Clamp(startIndex, 0, elements.Count() - 1);
                    endIndex = Math.Clamp(startIndex + scrollSettings.maxElements, 0, elements.Count());
                    startIndex = Math.Clamp(endIndex - scrollSettings.maxElements, 0, elements.Count() - 1);
                }

                txt.Append(startIndex == 0 ? scrollSettings.scrollIcon.topEndIndicator : scrollSettings.scrollIcon.topContinueIndicator);
                for (var x = startIndex; x < endIndex; x++)
                {
                    var element = elements.ElementAt(x);
                    txt.Append(element?.MakeText(
                            selected == x ? cursorIcon.sIcon : cursorIcon.icon,
                            selected == x ? cursorIcon.sIconR : cursorIcon.iconR,
                            passInObject ? this : null
                        ) ??
                        "\n");
                }
                txt.Append(endIndex == elements.Count() ? scrollSettings.scrollIcon.bottomEndIndicator : scrollSettings.scrollIcon.bottomContinueIndicator);

                Console.WriteLine(txt);

                // move selection/change value
                var actualMove = false;
                do
                {
                    // get pressedKey
                    pressedKey = keyResults.ElementAt((int)Key.ENTER);
                    var selectedElement = elements.ElementAt(selected);
                    if (
                        selectedElement is not null &&
                        selectedElement.IsClickable &&
                        selectedElement.IsOnlyClickable
                    )
                    {
                        pressedKey = Utils.GetKey(GetKeyMode.IGNORE_HORIZONTAL, keybinds);
                    }
                    else
                    {
                        while (pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)))
                        {
                            pressedKey = Utils.GetKey(GetKeyMode.NO_IGNORE, keybinds);
                            if (pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)) && !enterKeyNeeded)
                            {
                                pressedKey = keyResults.ElementAt((int)Key.ESCAPE);
                            }
                        }
                    }
                    // move selection
                    if (
                        pressedKey.Equals(keyResults.ElementAt((int)Key.UP)) ||
                        pressedKey.Equals(keyResults.ElementAt((int)Key.DOWN))
                    )
                    {
                        var prevSelected = selected;
                        while (true)
                        {
                            selected += pressedKey.Equals(keyResults.ElementAt((int)Key.DOWN)) ? 1 : -1;
                            selected %= elements.Count();
                            if (selected < 0)
                            {
                                selected = elements.Count() - 1;
                            }
                            var newSelected = elements.ElementAt(selected);
                            if (
                                newSelected is not null &&
                                newSelected.IsSelectable
                            )
                            {
                                break;
                            }
                        }
                        if (prevSelected != selected)
                        {
                            actualMove = true;
                        }
                    }
                    // change value
                    else if (
                        selectedElement is not null &&
                        selectedElement.IsSelectable &&
                        (
                            pressedKey.Equals(keyResults.ElementAt((int)Key.LEFT)) ||
                            pressedKey.Equals(keyResults.ElementAt((int)Key.RIGHT)) ||
                            pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)))
                        )
                    {
                        var returned = selectedElement.HandleAction(pressedKey, keyResults, keybinds, passInObject ? this : null);
                        if (returned is not null)
                        {
                            if (returned.GetType() == typeof(bool))
                            {
                                actualMove = (bool)returned;
                            }
                            else
                            {
                                return returned;
                            }
                        }
                    }
                    else if (canEscape && pressedKey.Equals(keyResults.ElementAt((int)Key.ESCAPE)))
                    {
                        actualMove = true;
                    }
                }
                while (!actualMove);
            }
            while (!canEscape || !pressedKey.Equals(keyResults.ElementAt((int)Key.ESCAPE)));
            return null;
        }
    }
}
