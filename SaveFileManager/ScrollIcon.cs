namespace SaveFileManager
{
    /// <summary>
    /// Object for storing the scroll icons for UI objects.
    /// </summary>
    public class ScrollIcon
    {
        #region Public fields
        /// <summary>
        /// The indicator at the top of the list, if the top of the list is not visible.
        /// </summary>
        public readonly string? topContinueIndicator;
        /// <summary>
        /// The indicator at the bottom of the list, if the bottom of the list is not visible.
        /// </summary>
        public readonly string? bottomContinueIndicator;
        /// <summary>
        /// The indicator at the top of the list, if the top of the list is visible.
        /// </summary>
        public readonly string? topEndIndicator;
        /// <summary>
        /// The indicator at the bottom of the list, if the bottom of the list is visible.
        /// </summary>
        public readonly string? bottomEndIndicator;
        #endregion

        #region Public constructors
        /// <summary>
        /// <inheritdoc cref="ScrollIcon"/>
        /// </summary>
        /// <param name="topContinueIndicator"><inheritdoc cref="topContinueIndicator" path="//summary"/></param>
        /// <param name="bottomContinueIndicator"><inheritdoc cref="bottomContinueIndicator" path="//summary"/></param>
        /// <param name="topEndIndicator"><inheritdoc cref="topEndIndicator" path="//summary"/></param>
        /// <param name="bottomEndIndicator"><inheritdoc cref="bottomEndIndicator" path="//summary"/></param>
        public ScrollIcon(string? topContinueIndicator = null, string? bottomContinueIndicator = null, string? topEndIndicator = null, string? bottomEndIndicator = null)
        {
            this.topContinueIndicator = topContinueIndicator;
            this.bottomContinueIndicator = bottomContinueIndicator;
            this.topEndIndicator = topEndIndicator;
            this.bottomEndIndicator = bottomEndIndicator;
        }
        #endregion
    }
}
