namespace ConsoleUI
{
    /// <summary>
    /// Object for the <c>OptionsUI</c> method.<br/>
    /// When used as input in the <c>OptionsUI</c> function, it draws text, that is not selectable.<br/>
    /// Structure: [<c>text</c>]
    /// </summary>
    public class Label : BaseUI
    {
        #region Override properties
        /// <inheritdoc cref="BaseUI.IsSelectable"/>
        public override bool IsSelectable { get => false; }
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="Label"/>
        /// </summary>
        /// <param name="text">The text to write out.</param>
        public Label(string text)
            : base(-1, text, "", false, "", false) { }
        #endregion

        #region Override methods
        /// <inheritdoc cref="BaseUI.MakeText"/>
        public override string MakeText(string icon, string iconR, OptionsUI? optionsUI = null)
        {
            return preText + "\n";
        }

        /// <inheritdoc cref="BaseUI.HandleAction"/>
        public override object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null, OptionsUI? optionsUI = null)
        {
            return false;
        }
        #endregion
    }
}
