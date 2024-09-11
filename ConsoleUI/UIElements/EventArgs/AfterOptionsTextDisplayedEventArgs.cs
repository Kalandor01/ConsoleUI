namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>AfterTextDisplayed</c> event.
    /// </summary>
    public class AfterOptionsTextDisplayedEventArgs
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
        /// <inheritdoc cref="AfterOptionsTextCreatedEventArgs"/>
        /// </summary>
        /// <param name="createdText"><inheritdoc cref="createdText" path="//summary"/></param>
        /// <param name="currentUIIndex"><inheritdoc cref="currentUIIndex" path="//summary"/></param>
        public AfterOptionsTextDisplayedEventArgs(
            string createdText,
            int currentUIIndex
        )
        {
            this.createdText = createdText;
            this.currentUIIndex = currentUIIndex;
        }
    }
}
