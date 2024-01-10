namespace ConsoleUI
{
    /// <summary>
    /// Modes for the <c>GetKey</c> function.
    /// </summary>
    public enum GetKeyMode
    {
        /// <summary>
        /// Key mode for not ignoring any keypress.
        /// </summary>
        NO_IGNORE,
        /// <summary>
        /// Key mode for ignoring keypresses that corespond to horizontal movement.
        /// </summary>
        IGNORE_HORIZONTAL,
        /// <summary>
        /// Key mode for ignoring keypresses that corespond to vertical movement actions.
        /// </summary>
        IGNORE_VERTICAL,
        /// <summary>
        /// Key mode for ignoring keypresses that corespond to the escape action.
        /// </summary>
        IGNORE_ESCAPE,
        /// <summary>
        /// Key mode for ignoring keypresses that corespond to the enter action.
        /// </summary>
        IGNORE_ENTER,
        /// <summary>
        /// Key mode for ignoring keypresses that are arrow keys.
        /// </summary>
        IGNORE_ARROWS,
        /// <summary>
        /// Key mode for ignoring keypresses that are not arrow keys.
        /// </summary>
        ONLY_ARROWS,
    }
}
