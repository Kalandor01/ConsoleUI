using System;

namespace Save_File_Manager
{
    public class CursorIcon
    {
        public readonly string sIcon;
        public readonly string sIconR;
        public readonly string icon;
        public readonly string iconR;

        /// <summary>
        /// Cursor icon object for UI objects.
        /// </summary>
        /// <param name="selectedIcon">Icon displayed on the left of the option when it is selected in the UI.</param>
        /// <param name="selectedIconRight">Icon displayed on the right of the option when it is selected in the UI.</param>
        /// <param name="notSelectedIcon">Icon displayed on the left of the option when it is not selected in the UI.</param>
        /// <param name="notSelectedIconRight">Icon displayed on the right of the option when it is not selected in the UI.</param>
        public CursorIcon(string selectedIcon=">", string selectedIconRight="", string notSelectedIcon=" ", string notSelectedIconRight="")
        {
            this.sIcon = selectedIcon ?? throw new ArgumentNullException(nameof(selectedIcon));
            this.sIconR = selectedIconRight ?? throw new ArgumentNullException(nameof(selectedIconRight));
            this.icon = notSelectedIcon ?? throw new ArgumentNullException(nameof(notSelectedIcon));
            this.iconR = notSelectedIconRight ?? throw new ArgumentNullException(nameof(notSelectedIconRight));
        }
    }
}
