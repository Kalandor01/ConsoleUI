namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>BeforeTextCreated</c> event.
    /// </summary>
    public class BeforeTextCreatedEventArgs
    {
        /// <summary>
        /// The sender of the event.
        /// </summary>
        public readonly BaseUI sender;
        /// <summary>
        /// The icon to be displayed on the left of the created text.
        /// </summary>
        public readonly string icon;
        /// <summary>
        /// The icon to be displayed on the right of the created text.
        /// </summary>
        public readonly string iconR;
        /// <summary>
        /// The OptionsUI containing the UI element.
        /// </summary>
        public readonly OptionsUI? optionsUI;

        /// <summary>
        /// If not null, halts text creartion and returns this text instead.
        /// </summary>
        public string? OverrideText { get; set; }

        /// <summary>
        /// <inheritdoc cref="BeforeTextCreatedEventArgs"/>
        /// </summary>
        /// <param name="sender"><inheritdoc cref="sender" path="//summary"/></param>
        /// <param name="icon"><inheritdoc cref="icon" path="//summary"/></param>
        /// <param name="iconR"><inheritdoc cref="iconR" path="//summary"/></param>
        /// <param name="optionsUI"><inheritdoc cref="optionsUI" path="//summary"/></param>
        /// <param name="overrideText"><inheritdoc cref="OverrideText" path="//summary"/></param>
        public BeforeTextCreatedEventArgs(
            BaseUI sender,
            string icon,
            string iconR,
            OptionsUI? optionsUI = null,
            string? overrideText = null
        )
        {
            this.sender = sender;
            this.icon = icon;
            this.iconR = iconR;
            this.optionsUI = optionsUI;
            OverrideText = overrideText;
        }
    }
}
