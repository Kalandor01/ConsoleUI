namespace SaveFileManager
{
    public class Toggle : BaseUI
    {
        string symbol;
        string symbolOff;

        /// <summary>
        /// Object for the options_ui method.<br/>
        /// When used as input in the <c>OptionsUI</c> function, it draws a field that is toggleable with the key associated with the enter action.<br/>
        /// Structure: [preText][symbol or symbolOff][postValue]
        /// </summary>
        /// <param name="symbol">The text displayed when the toggle is on.</param>
        /// <param name="symbolOff">The text displayed when the toggle is off.</param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
        public Toggle(int value = 0, string preText = "", string symbol = "on", string symbolOff = "off", string postValue = "", bool multiline = false)
            : base(Math.Clamp(value, 0, 1), preText, "", false, postValue, multiline)
        {
            this.symbol = symbol;
            this.symbolOff = symbolOff;
        }

        /// <inheritdoc cref="MakeSpecial(string)"/>
        protected string MakeSpecial(string icons)
        {
            return value == 1 ? symbol : symbolOff;
        }

        /// <inheritdoc cref="HandleAction(object, IEnumerable{object}, IEnumerable{KeyAction}?)"/>
        public bool HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null)
        {
            if (key.Equals(keyResults.ElementAt((int)Key.ENTER)))
            {
                value = !(value == 1) ? 1 : 0;
            }
            return true;
        }
    }
}
