namespace SaveFileManager
{
    /// <summary>
    /// Object for the <c>OptionsUI</c> method.<br/>
    /// When used as input in the <c>OptionsUI</c> function, it draws a field that is toggleable with the key associated with the enter action.<br/>
    /// Structure: [<c>preText</c>][<c>symbol</c> or <c>symbolOff</c>][<c>postValue</c>]
    /// </summary>
    public class Toggle : BaseUI
    {
        #region Private fields
        /// <summary>
        /// The current value of the object.
        /// </summary>
        new bool value;
        /// <summary>
        /// The text displayed when the toggle is on.
        /// </summary>
        string symbol;
        /// <summary>
        /// The text displayed when the toggle is off.
        /// </summary>
        string symbolOff;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="Toggle"/>
        /// </summary>
        /// <param name="value"><inheritdoc cref="value" path="//summary"/></param>
        /// <param name="symbol"><inheritdoc cref="symbol" path="//summary"/></param>
        /// <param name="symbolOff"><inheritdoc cref="symbolOff" path="//summary"/></param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
        public Toggle(bool value = false, string preText = "", string symbol = "on", string symbolOff = "off", string postValue = "", bool multiline = false)
            : base(Math.Clamp(-1, 0, 1), preText, "", false, postValue, multiline)
        {
            this.value = value;
            this.symbol = symbol;
            this.symbolOff = symbolOff;
        }
        #endregion

        #region Override methods
        /// <inheritdoc cref="BaseUI.MakeSpecial"/>
        protected override string MakeSpecial(string icons, OptionsUI? optionsUI = null)
        {
            return value ? symbol : symbolOff;
        }

        /// <inheritdoc cref="BaseUI.HandleAction"/>
        public override object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null, OptionsUI? optionsUI = null)
        {
            if (key.Equals(keyResults.ElementAt((int)Key.ENTER)))
            {
                value = !value;
            }
            return true;
        }

        /// <inheritdoc cref="BaseUI.IsOnlyClickable"/>
        public override bool IsOnlyClickable()
        {
            return true;
        }
        #endregion
    }
}
