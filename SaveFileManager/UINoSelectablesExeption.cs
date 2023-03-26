namespace SaveFileManager
{
    /// <summary>
    /// Exeption raised when there are no selectable values in the selectables list.
    /// </summary>
    public class UINoSelectablesExeption : Exception
    {
        #region Constructors
        /// <summary>
        /// <inheritdoc cref="UINoSelectablesExeption"/>
        /// </summary>
        public UINoSelectablesExeption()
            : base("No selectable element in the list.") { }

        /// <summary>
        /// <inheritdoc cref="UINoSelectablesExeption"/>
        /// </summary>
        /// <param name="message">The message to display.</param>
        public UINoSelectablesExeption(string message)
            : base(message) { }
        #endregion
    }
}
