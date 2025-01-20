namespace ConsoleUI.UIElements
{
    /// <summary>
    /// A class for representing a button in <see cref="MultiButton"/>.
    /// </summary>
    public class MultiButtonElement
    {
        /// <summary>
        /// The action to do, when the button is pressed. Same as <see cref="Button.action"/>.
        /// </summary>
        public UIAction action;

        /// <summary>
        /// The text to display, when this button is not selected.
        /// </summary>
        public string inactiveText;

        /// <summary>
        /// The text to display, when this button is selected.
        /// </summary>
        public string activeText;

        /// <summary>
        /// <inheritdoc cref="MultiButtonElement" path="//summary"/>
        /// </summary>
        /// <param name="action"><inheritdoc cref="action" path="//summary"/></param>
        /// <param name="inactiveText"><inheritdoc cref="inactiveText" path="//summary"/></param>
        /// <param name="activeText"><inheritdoc cref="activeText" path="//summary"/></param>
        public MultiButtonElement(UIAction action, string inactiveText, string activeText)
        {
            this.action = action;
            this.inactiveText = inactiveText;
            this.activeText = activeText;
        }

        /// <inheritdoc cref="MultiButtonElement(UIAction, string, string)"/>>
        /// <param name="text">The text to display for this button.</param>
        public MultiButtonElement(UIAction action, string text)
            :this(action, text, text) { }
    }
}
