namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>BeforeOptionsDisplayed</c> event.
    /// </summary>
    public class BeforeOptionsDisplayedEventArgs
    {
        /// <summary>
        /// If not null, halts text creartion and returns this text instead.
        /// </summary>
        public string? OverrideText { get; set; }

        /// <summary>
        /// <inheritdoc cref="BeforeOptionsDisplayedEventArgs"/>
        /// </summary>
        /// <param name="overrideText"><inheritdoc cref="OverrideText" path="//summary"/></param>
        public BeforeOptionsDisplayedEventArgs(string? overrideText = null)
        {
            OverrideText = overrideText;
        }
    }
}
