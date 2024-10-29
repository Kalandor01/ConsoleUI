namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>BeforeElementsDisplayed</c> event.
    /// </summary>
    public class BeforeElementsDisplayedEventArgs
    {
        /// <summary>
        /// If not null, halts text creartion and returns this text instead.
        /// </summary>
        public string? OverrideText { get; set; }

        /// <summary>
        /// <inheritdoc cref="BeforeElementsDisplayedEventArgs"/>
        /// </summary>
        /// <param name="overrideText"><inheritdoc cref="OverrideText" path="//summary"/></param>
        public BeforeElementsDisplayedEventArgs(string? overrideText = null)
        {
            OverrideText = overrideText;
        }
    }
}
