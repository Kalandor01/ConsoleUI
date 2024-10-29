namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>AfterElementsDisplayed</c> event.
    /// </summary>
    public class AfterElementsDisplayedEventArgs
    {
        /// <summary>
        /// The text the UI element created.
        /// </summary>
        public readonly int lastDisplayedElementIndex;

        /// <summary>
        /// <inheritdoc cref="AfterElementsDisplayedEventArgs"/>
        /// </summary>
        /// <param name="lastDisplayedElementIndex"><inheritdoc cref="lastDisplayedElementIndex" path="//summary"/></param>
        public AfterElementsDisplayedEventArgs(int lastDisplayedElementIndex)
        {
            this.lastDisplayedElementIndex = lastDisplayedElementIndex;
        }
    }
}
