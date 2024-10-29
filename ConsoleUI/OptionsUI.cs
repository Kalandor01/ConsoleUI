using ConsoleUI.Keybinds;
using ConsoleUI.UIElements;
using ConsoleUI.UIElements.EventArgs;
using System.Text;
using static ConsoleUI.Utils;

namespace ConsoleUI
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

        #region Event delegates
        /// <summary>
        /// Called before the UI elements are displayed.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void BeforeElementsDisplayedEventHandler(OptionsUI sender, BeforeElementsDisplayedEventArgs args);

        /// <summary>
        /// Called after the UI elements are displayed.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void AfterElementsDisplayedEventHandler(OptionsUI sender, AfterElementsDisplayedEventArgs args);

        /// <summary>
        /// Called before a UI element text is created.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void BeforeOptionsTextCreatedEventHandler(OptionsUI sender, BeforeElementTextCreatedEventArgs args);

        /// <summary>
        /// Called after a UI element text is created.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void AfterOptionsTextCreatedEventHandler(OptionsUI sender, AfterElementTextCreatedEventArgs args);

        /// <summary>
        /// Called after a UI element text is displayed.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void AfterOptionsTextDisplayedEventHandler(OptionsUI sender, AfterElementTextDisplayedEventArgs args);

        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void OptionsKeyPressedEventHandler(OptionsUI sender, KeyPressedEventArgs args);

        /// <summary>
        /// Called when the selected element is changed.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void SelectionChangedEventHandler(OptionsUI sender, SelectionChangedEventArgs args);

        /// <summary>
        /// Called before the UI exits.
        /// </summary>
        /// <param name="sender">The sender of this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void BeforeExitingEventHandler(OptionsUI sender, BeforeExitingEventArgs args);
        #endregion

        #region Events
        /// <summary>
        /// Called before the UI elements are displayed.
        /// </summary>
        public event BeforeElementsDisplayedEventHandler BeforeElementsDisplayed;

        /// <summary>
        /// Called after the UI elements are displayed.
        /// </summary>
        public event AfterElementsDisplayedEventHandler AfterElementsDisplayed;

        /// <summary>
        /// Called before a UI element text is created.
        /// </summary>
        public event BeforeOptionsTextCreatedEventHandler BeforeTextCreated;

        /// <summary>
        /// Called after a UI element is text is created.
        /// </summary>
        public event AfterOptionsTextCreatedEventHandler AfterTextCreated;

        /// <summary>
        /// Called after a UI element text is displayed.
        /// </summary>
        public event AfterOptionsTextDisplayedEventHandler AfterTextDisplayed;

        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        public event OptionsKeyPressedEventHandler KeyPressed;

        /// <summary>
        /// Called when the selected element is changed.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Called before the UI exits.
        /// </summary>
        public event BeforeExitingEventHandler BeforeExiting;
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

        #region EventCallFunctions
        /// <summary>
        /// Calls the <c>BeforeElementsDisplayed</c> event.
        /// </summary>
        private void RaiseBeforeElementsDisplayedEvent(BeforeElementsDisplayedEventArgs args)
        {
            if (BeforeElementsDisplayed is not null)
            {
                BeforeElementsDisplayed(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>AfterElementsDisplayed</c> event.
        /// </summary>
        private void RaiseAfterElementsDisplayedEvent(AfterElementsDisplayedEventArgs args)
        {
            if (AfterElementsDisplayed is not null)
            {
                AfterElementsDisplayed(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>BeforeTextCreated</c> event.
        /// </summary>
        private void RaiseBeforeTextCreatedEvent(BeforeElementTextCreatedEventArgs args)
        {
            if (BeforeTextCreated is not null)
            {
                BeforeTextCreated(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>AfterTextCreated</c> event.
        /// </summary>
        protected void RaiseAfterTextCreatedEvent(AfterElementTextCreatedEventArgs args)
        {
            if (AfterTextCreated is not null)
            {
                AfterTextCreated(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>AfterTextDisplayed</c> event.
        /// </summary>
        protected void RaiseAfterTextDisplayedEvent(AfterElementTextDisplayedEventArgs args)
        {
            if (AfterTextDisplayed is not null)
            {
                AfterTextDisplayed(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>KeyPressed</c> event.
        /// </summary>
        protected void RaiseKeyPressedEvent(KeyPressedEventArgs args)
        {
            if (KeyPressed is not null)
            {
                KeyPressed(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>SelectionChanged</c> event.
        /// </summary>
        protected void RaiseSelectionChangedEvent(SelectionChangedEventArgs args)
        {
            if (SelectionChanged is not null)
            {
                SelectionChanged(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>BeforeExiting</c> event.
        /// </summary>
        protected void RaiseBeforeExitingEvent(BeforeExitingEventArgs args)
        {
            if (BeforeExiting is not null)
            {
                BeforeExiting(this, args);
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Prints the title and then a list of elements that the user can cycle between with the up and down arrows, and adjust with either the left and right arrow keys or the enter pressedKey depending on the input object type, and exit with the pressedKey assigned to escape.<br/>
        /// if an element in the list is null, the line will be blank and cannot be selected.
        /// </summary>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use. The order of the actions should be:<br/>
        /// - escape, up, down, left, right, enter.</param>
        /// <param name="getKeyFunction">The function to get the next valid key the user pressed.<br/>
        /// Should function similarly to <see cref="GetKey(GetKeyMode, IEnumerable{KeyAction}?)"/>.></param>
        /// <exception cref="UINoSelectablesExeption">Exceptions thrown, if there are no selectable UI elements in the list.</exception>
        public object? Display(
            IEnumerable<KeyAction>? keybinds = null,
            GetKeyFunctionDelegate? getKeyFunction = null
        )
        {
            // no selectable element
            if (elements.All(element => element is null || !element.IsSelectable))
            {
                throw new UINoSelectablesExeption();
            }

            // keybinds
            if (keybinds is null || keybinds.Count() < 6)
            {
                keybinds = GetDefaultKeybinds();
            }
            getKeyFunction ??= GetKey;

            cursorIcon ??= new CursorIcon();

            // is enter needed?
            var enterKeyNeeded = elements.Any(element => element is not null && element.IsClickable);
            // put selected on selectable
            selected = 0;
            while (elements.ElementAt(selected)?.IsSelectable != true)
            {
                selected++;
            }

            startIndex = Math.Clamp(0, selected - scrollSettings.scrollUpMargin, elements.Count() - 1);

            // render/getkey loop
            KeyAction pressedKey;
            var actualMove = true;
            while (true)
            {
                // prevent infinite loop
                if (elements.All(answer => answer is null || !answer.IsSelectable))
                {
                    throw new UINoSelectablesExeption();
                }

                if (actualMove)
                {
                    DisplayOptions();
                }
                actualMove = false;

                // move selection/change value
                do
                {
                    // get pressedKey
                    pressedKey = keybinds.ElementAt((int)Key.ENTER);
                    var selectedElement = elements.ElementAt(selected);
                    if (
                        selectedElement is not null &&
                        selectedElement.IsClickable &&
                        selectedElement.IsOnlyClickable
                    )
                    {
                        pressedKey = getKeyFunction(GetKeyMode.IGNORE_HORIZONTAL, keybinds);
                    }
                    else
                    {
                        while (pressedKey.Equals(keybinds.ElementAt((int)Key.ENTER)))
                        {
                            pressedKey = getKeyFunction(GetKeyMode.NO_IGNORE, keybinds);
                            if (pressedKey.Equals(keybinds.ElementAt((int)Key.ENTER)) && !enterKeyNeeded)
                            {
                                pressedKey = keybinds.ElementAt((int)Key.ESCAPE);
                            }
                        }
                    }

                    var keyPressedEventArgs = new KeyPressedEventArgs(pressedKey, keybinds, getKeyFunction);
                    RaiseKeyPressedEvent(keyPressedEventArgs);
                    if (keyPressedEventArgs.UpdateScreen != null)
                    {
                        actualMove = (bool)keyPressedEventArgs.UpdateScreen;
                    }
                    if (keyPressedEventArgs.CancelKeyHandling)
                    {
                        continue;
                    }

                    // move selection
                    var selectionMoveUp = pressedKey.Equals(keybinds.ElementAt((int)Key.UP));
                    if (
                        selectionMoveUp ||
                        pressedKey.Equals(keybinds.ElementAt((int)Key.DOWN))
                    )
                    {
                        MoveSelectedion(selectionMoveUp ? -1 : 1, ref actualMove);
                    }
                    // change value
                    else if (
                        selectedElement is not null &&
                        selectedElement.IsSelectable &&
                        !pressedKey.Equals(keybinds.ElementAt((int)Key.ESCAPE))
                    )
                    {
                        var returned = selectedElement.HandleAction(pressedKey, keybinds, getKeyFunction, passInObject ? this : null);
                        if (returned is not null)
                        {
                            if (returned.GetType() == typeof(bool))
                            {
                                actualMove = (bool)returned;
                            }
                            else
                            {
                                var beforeExitingEventArgs = new BeforeExitingEventArgs(returned, true);
                                RaiseBeforeExitingEvent(beforeExitingEventArgs);

                                if (!beforeExitingEventArgs.CancelExiting)
                                {
                                    return returned;
                                }
                                if (beforeExitingEventArgs.UpdateScreen != null)
                                {
                                    actualMove = (bool)beforeExitingEventArgs.UpdateScreen;
                                }
                            }
                        }
                    }
                    else if (canEscape && pressedKey.Equals(keybinds.ElementAt((int)Key.ESCAPE)))
                    {
                        actualMove = true;
                    }
                }
                while (!actualMove);

                if (canEscape && pressedKey.Equals(keybinds.ElementAt((int)Key.ESCAPE)))
                {
                    var beforeExitingEventArgs = new BeforeExitingEventArgs(null, false);
                    RaiseBeforeExitingEvent(beforeExitingEventArgs);

                    if (!beforeExitingEventArgs.CancelExiting)
                    {
                        return null;
                    }
                    if (beforeExitingEventArgs.UpdateScreen != null)
                    {
                        actualMove = (bool)beforeExitingEventArgs.UpdateScreen;
                    }
                }
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Displays all visible options.
        /// </summary>
        private void DisplayOptions()
        {
            var beforeDisplayArgs = new BeforeElementsDisplayedEventArgs();
            RaiseBeforeElementsDisplayedEvent(beforeDisplayArgs);
            if (beforeDisplayArgs.OverrideText != null)
            {
                Console.WriteLine(beforeDisplayArgs.OverrideText);
                return;
            }

            // clear screen + render
            var txtBeginning = new StringBuilder(clearScreenText);

            // title
            if (title is not null)
            {
                txtBeginning.Append($"{title}\n\n");
            }

            // elements
            int endIndex;
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

            txtBeginning.Append(startIndex == 0 ? scrollSettings.scrollIcon.topEndIndicator : scrollSettings.scrollIcon.topContinueIndicator);
            Console.Write(txtBeginning.ToString());
            for (var x = startIndex; x < endIndex; x++)
            {
                var element = elements.ElementAt(x)!;
                var beforeTextCreatedArgs = new BeforeElementTextCreatedEventArgs(x);
                RaiseBeforeTextCreatedEvent(beforeTextCreatedArgs);
                if (beforeTextCreatedArgs.OverrideText != null)
                {
                    Console.Write(beforeTextCreatedArgs.OverrideText);
                    continue;
                }

                var elementText = element?.MakeText(
                        selected == x ? cursorIcon.sIcon : cursorIcon.icon,
                        selected == x ? cursorIcon.sIconR : cursorIcon.iconR,
                        passInObject ? this : null
                    ) ?? "\n";

                var afterTextCreatedArgs = new AfterElementTextCreatedEventArgs(elementText, x);
                RaiseAfterTextCreatedEvent(afterTextCreatedArgs);

                Console.Write(afterTextCreatedArgs.OverrideText ?? elementText);

                var afterTextDisplayedArgs = new AfterElementTextDisplayedEventArgs(elementText, x);
                RaiseAfterTextDisplayedEvent(afterTextDisplayedArgs);
            }

            Console.WriteLine(endIndex == elements.Count() ? scrollSettings.scrollIcon.bottomEndIndicator : scrollSettings.scrollIcon.bottomContinueIndicator);

            var afterDisplayArgs = new AfterElementsDisplayedEventArgs(endIndex);
            RaiseAfterElementsDisplayedEvent(afterDisplayArgs);
        }

        /// <summary>
        /// Moves the selection.
        /// </summary>
        /// <param name="selectionOffset">How much to move the selection down.</param>
        /// <param name="updateScreen">If the screen should update.</param>
        /// <returns>If the selection changed.</returns>
        private void MoveSelectedion(int selectionOffset, ref bool updateScreen)
        {
            var prevSelected = selected;
            while (true)
            {
                selected += selectionOffset;
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

            var selectionChangedEventArgs = new SelectionChangedEventArgs(prevSelected, selected);
            RaiseSelectionChangedEvent(selectionChangedEventArgs);
            selected = selectionChangedEventArgs.NewSelected;
            if (selectionChangedEventArgs.UpdateScreen != null)
            {
                updateScreen = (bool)selectionChangedEventArgs.UpdateScreen;
            }
            updateScreen |= prevSelected != selected;
        }
        #endregion
    }
}
