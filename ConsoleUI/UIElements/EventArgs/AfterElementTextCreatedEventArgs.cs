namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>AfterTextCreated</c> event.
    /// </summary>
    public class AfterElementTextCreatedEventArgs
    {
        /// <summary>
        /// The text the UI element created.
        /// </summary>
        public readonly string createdText;
        /// <summary>
        /// The index of the current BaseUI element.
        /// </summary>
        public readonly int currentUIIndex;

        /// <summary>
        /// If not null, halts text creartion and returns this text instead.
        /// </summary>
        public string? OverrideText { get; set; }

        /// <summary>
        /// <inheritdoc cref="AfterElementTextCreatedEventArgs"/>
        /// </summary>
        /// <param name="createdText"><inheritdoc cref="createdText" path="//summary"/></param>
        /// <param name="currentUIIndex"><inheritdoc cref="currentUIIndex" path="//summary"/></param>
        /// <param name="overrideText"><inheritdoc cref="OverrideText" path="//summary"/></param>
        public AfterElementTextCreatedEventArgs(
            string createdText,
            int currentUIIndex,
            string? overrideText = null
        )
        {
            this.createdText = createdText;
            this.currentUIIndex = currentUIIndex;
            OverrideText = overrideText;
        }
    }
}
