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
        /// Whether to pass in the object into the element's <c>HandleAction</c> function.
        /// </summary>
        public bool passInObject;
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
        /// <exception cref="UINoSelectablesExeption">Exceptions thrown, if there are no selectable UI elements in the list.</exception>
        public OptionsUI(IEnumerable<BaseUI?> elements, string? title = null, CursorIcon? cursorIcon = null, bool canEscape = true, bool passInObject = true)
        {
            if (elements.All(answer => answer is null || !answer.IsSelectable()))
            {
                throw new UINoSelectablesExeption();
            }
            this.elements = elements;
            this.title = title;
            this.cursorIcon = cursorIcon ?? new CursorIcon();
            this.canEscape = canEscape;
            this.passInObject = passInObject;
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
            if (elements.All(answer => answer is null || !answer.IsSelectable()))
            {
                throw new UINoSelectablesExeption();
            }

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
            var noEnter = true;
            foreach (var element in elements)
            {
                if (
                    element is not null &&
                    (
                        typeof(Toggle).IsAssignableFrom(element.GetType()) ||
                        typeof(Button).IsAssignableFrom(element.GetType())
                    )
                )
                {
                    noEnter = false;
                    break;
                }
            }
            // put selected on selectable
            var selected = 0;
            while (
                elements.ElementAt(selected)?.IsSelectable() != true
            )
            {
                selected++;
            }
            // render/getkey loop
            object pressedKey;
            do
            {
                // prevent infinite loop
                if (elements.All(answer => answer is null || !answer.IsSelectable()))
                {
                    throw new UINoSelectablesExeption();
                }
                // render
                // clear screen
                var txt = new StringBuilder("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                if (title is not null)
                {
                    txt.Append(title + "\n\n");
                }
                for (var x = 0; x < elements.Count(); x++)
                {
                    var element = elements.ElementAt(x);
                    if (element is not null && typeof(BaseUI).IsAssignableFrom(element.GetType()))
                    {
                        txt.Append(element.MakeText(
                            selected == x ? cursorIcon.sIcon : cursorIcon.icon,
                            selected == x ? cursorIcon.sIconR : cursorIcon.iconR,
                            elements
                        ));
                    }
                    else if (element is null)
                    {
                        txt.Append('\n');
                    }
                    else
                    {
                        txt.Append(element.ToString() + "\n");
                    }
                }
                Console.WriteLine(txt.ToString());
                // move selection/change value
                var actualMove = false;
                do
                {
                    // get pressedKey
                    pressedKey = keyResults.ElementAt((int)Key.ENTER);
                    var selectedElement = elements.ElementAt(selected);
                    if (
                        selectedElement is not null &&
                        selectedElement.IsOnlyClickable()
                    )
                    {
                        pressedKey = Utils.GetKey(GetKeyMode.IGNORE_HORIZONTAL, keybinds);
                    }
                    else
                    {
                        while (pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)))
                        {
                            pressedKey = Utils.GetKey(GetKeyMode.NO_IGNORE, keybinds);
                            if (pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)) && noEnter)
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
                                newSelected.IsSelectable()
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
                        selectedElement.IsSelectable() &&
                        (
                            pressedKey.Equals(keyResults.ElementAt((int)Key.LEFT)) ||
                            pressedKey.Equals(keyResults.ElementAt((int)Key.RIGHT)) ||
                            pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)))
                        )
                    {
                        var returned = selectedElement.HandleAction(pressedKey, keyResults, keybinds, this);
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
