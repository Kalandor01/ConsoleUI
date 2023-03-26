namespace SaveFileManager
{
    /// <summary>
    /// Cursor icon object for UI objects.
    /// </summary>
    public class CursorIcon
    {
        #region Public fields
        /// <summary>
        /// Icon displayed on the left of the option when it is selected in the UI.
        /// </summary>
        public readonly string sIcon;
        /// <summary>
        /// Icon displayed on the right of the option when it is selected in the UI.
        /// </summary>
        public readonly string sIconR;
        /// <summary>
        /// Icon displayed on the left of the option when it is not selected in the UI.
        /// </summary>
        public readonly string icon;
        /// <summary>
        /// Icon displayed on the right of the option when it is not selected in the UI.
        /// </summary>
        public readonly string iconR;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="CursorIcon"/>
        /// </summary>
        /// <param name="selectedIcon"><inheritdoc cref="sIcon" path="//summary"/></param>
        /// <param name="selectedIconRight"><inheritdoc cref="sIconR" path="//summary"/></param>
        /// <param name="notSelectedIcon"><inheritdoc cref="icon" path="//summary"/></param>
        /// <param name="notSelectedIconRight"><inheritdoc cref="iconR" path="//summary"/></param>
        public CursorIcon(string selectedIcon=">", string selectedIconRight="", string notSelectedIcon=" ", string notSelectedIconRight="")
        {
            sIcon = selectedIcon ?? throw new ArgumentNullException(nameof(selectedIcon));
            sIconR = selectedIconRight ?? throw new ArgumentNullException(nameof(selectedIconRight));
            icon = notSelectedIcon ?? throw new ArgumentNullException(nameof(notSelectedIcon));
            iconR = notSelectedIconRight ?? throw new ArgumentNullException(nameof(notSelectedIconRight));
        }
        #endregion
    }
}
