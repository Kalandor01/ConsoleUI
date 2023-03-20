namespace SaveFileManager
{
    /// <summary>
    /// Exeption raised when there are no values in the selectables list in the <c>OptionsUI</c> function.
    /// </summary>
    public class UINoSelectablesExeption : Exception
    {
        public UINoSelectablesExeption()
            : base("No selectable element in the list.") { }
        public UINoSelectablesExeption(string message)
            : base(message) { }
    }
}
