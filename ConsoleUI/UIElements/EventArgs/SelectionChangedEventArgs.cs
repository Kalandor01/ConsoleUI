namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>SelectionChanged</c> event.
    /// </summary>
    public class SelectionChangedEventArgs
    {
        /// <summary>
        /// The previous index of the selected element.
        /// </summary>
        public readonly int oldSelected;

        /// <summary>
        /// The new index of the selected element.
        /// </summary>
        public int NewSelected { get; set; }
        /// <summary>
        /// Whether to update the screen after moving the selection.
        /// </summary>
        public bool? UpdateScreen { get; set; }

        /// <summary>
        /// <inheritdoc cref="SelectionChangedEventArgs"/>
        /// </summary>
        /// <param name="oldSelected"><inheritdoc cref="oldSelected" path="//summary"/></param>
        /// <param name="newSelected"><inheritdoc cref="NewSelected" path="//summary"/></param>
        /// <param name="updateScreen"><inheritdoc cref="UpdateScreen" path="//summary"/></param>
        public SelectionChangedEventArgs(
            int oldSelected,
            int newSelected,
            bool? updateScreen = null
        )
        {
            this.oldSelected = oldSelected;
            NewSelected = newSelected;
            UpdateScreen = updateScreen;
        }
    }
}
