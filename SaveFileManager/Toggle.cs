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
        /// <param name="symbol"><inheritdoc cref="symbol" path="//summary"/></param>
        /// <param name="symbolOff"><inheritdoc cref="symbolOff" path="//summary"/></param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
        public Toggle(int value = 0, string preText = "", string symbol = "on", string symbolOff = "off", string postValue = "", bool multiline = false)
            : base(Math.Clamp(value, 0, 1), preText, "", false, postValue, multiline)
        {
            this.symbol = symbol;
            this.symbolOff = symbolOff;
        }
        #endregion

        #region Override methods
        /// <inheritdoc cref="BaseUI.MakeSpecial"/>
        protected override string MakeSpecial(string icons)
        {
            return value == 1 ? symbol : symbolOff;
        }

        /// <inheritdoc cref="BaseUI.HandleAction"/>
        public override object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null)
        {
            if (key.Equals(keyResults.ElementAt((int)Key.ENTER)))
            {
                value = !(value == 1) ? 1 : 0;
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
