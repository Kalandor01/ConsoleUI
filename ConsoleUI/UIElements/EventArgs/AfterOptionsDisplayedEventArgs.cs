namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>AfterOptionsDisplayed</c> event.
    /// </summary>
    public class AfterOptionsDisplayedEventArgs
    {
        /// <summary>
        /// The text the UI element created.
        /// </summary>
        public readonly int lastDisplayedElementIndex;

        /// <summary>
        /// <inheritdoc cref="AfterOptionsDisplayedEventArgs"/>
        /// </summary>
        /// <param name="lastDisplayedElementIndex"><inheritdoc cref="lastDisplayedElementIndex" path="//summary"/></param>
        public AfterOptionsDisplayedEventArgs(int lastDisplayedElementIndex)
        {
            this.lastDisplayedElementIndex = lastDisplayedElementIndex;
        }
    }
}
