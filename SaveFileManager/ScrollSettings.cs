namespace SaveFileManager
{
    /// <summary>
    /// Object for storing the scroll settings for UI objects.
    /// </summary>
    public class ScrollSettings
    {
        #region Public fields
        /// <summary>
        /// The maximum number of elements in the list, before it gets cut of.
        /// </summary>
        public readonly int maxElements;
        /// <summary>
        /// The scroll icons to use.
        /// </summary>
        public readonly ScrollIcon scrollIcon;
        /// <summary>
        /// The number of elements from the top before scrolling up.
        /// </summary>
        public readonly int scrollUpMargin;
        /// <summary>
        /// The number of elements from the bottom before scrolling down.
        /// </summary>
        public readonly int scrollDownMargin;
        #endregion

        #region Public constructors
        /// <summary>
        /// <inheritdoc cref="ScrollSettings" path="//summary"/>
        /// </summary>
        /// <param name="maxElements"><inheritdoc cref="maxElements" path="//summary"/></param>
        /// <param name="scrollIcon"><inheritdoc cref="scrollIcon" path="//summary"/></param>
        /// <param name="scrollUpMargin"><inheritdoc cref="scrollUpMargin" path="//summary"/></param>
        /// <param name="scrollDownMargin"><inheritdoc cref="scrollDownMargin" path="//summary"/></param>
        public ScrollSettings(int maxElements = -1, ScrollIcon? scrollIcon = null, int scrollUpMargin = 0, int scrollDownMargin = 0)
        {
            this.maxElements = maxElements < 0 ? -1 : maxElements;
            this.scrollIcon = scrollIcon ?? new ScrollIcon();
            this.scrollUpMargin = scrollUpMargin;
            this.scrollDownMargin = scrollDownMargin;
        }
        #endregion
    }
}
