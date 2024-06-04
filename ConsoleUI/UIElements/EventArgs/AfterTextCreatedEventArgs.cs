namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>AfterTextCreated</c> event.
    /// </summary>
    public class AfterTextCreatedEventArgs
    {
        /// <summary>
        /// The sender of the event.
        /// </summary>
        public readonly BaseUI sender;
        /// <summary>
        /// The text the UI element created.
        /// </summary>
        public readonly string createdText;
        /// <summary>
        /// The OptionsUI containing the UI element.
        /// </summary>
        public readonly OptionsUI? optionsUI;

        /// <summary>
        /// If not null, halts text creartion and returns this text instead.
        /// </summary>
        public string? OverrideText { get; set; }

        /// <summary>
        /// <inheritdoc cref="AfterTextCreatedEventArgs"/>
        /// </summary>
        /// <param name="sender"><inheritdoc cref="sender" path="//summary"/></param>
        /// <param name="createdText"><inheritdoc cref="createdText" path="//summary"/></param>
        /// <param name="optionsUI"><inheritdoc cref="optionsUI" path="//summary"/></param>
        /// <param name="overrideText"><inheritdoc cref="OverrideText" path="//summary"/></param>
        public AfterTextCreatedEventArgs(
            BaseUI sender,
            string createdText,
            OptionsUI? optionsUI = null,
            string? overrideText = null
        )
        {
            this.sender = sender;
            this.createdText = createdText;
            this.optionsUI = optionsUI;
            OverrideText = overrideText;
        }
    }
}
