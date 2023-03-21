namespace SaveFileManager
{
    public class Choice : BaseUI
    {
        IEnumerable<string> choices;

        /// <summary>
        /// Object for the <c>OptionsUI</c> method.<br/>
        /// When used as input in the <c>OptionsUI</c> function, it draws a multiple choice seletion, with the <c>choices</c> list specifying the choice names.<br/>
        /// Structure: [preText][choice name][preValue][value][postValue]
        /// </summary>
        /// <param name="choices">The list of options the user can choose from.</param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
        public Choice(IEnumerable<string> choices, int value = 0, string preText = "", string preValue = "", bool displayValue = false, string postValue = "", bool multiline = false)
            : base(Math.Clamp(value, 0, choices.Count() - 1), preText, preValue, displayValue, postValue, multiline)
        {
            this.choices = choices;
        }

        /// <inheritdoc cref="MakeSpecial(string)"/>
        protected override string MakeSpecial(string icons)
        {
            if (multiline)
            {
                return choices.ElementAt(value).Replace("\n", icons);
            }
            else
            {
                return choices.ElementAt(value);
            }
        }

        /// <inheritdoc cref="MakeValue()"/>
        protected override string MakeValue()
        {
            return $"{value + 1}/{choices.Count()}";
        }

        /// <inheritdoc cref="HandleAction(object, IEnumerable{object}, IEnumerable{KeyAction}?)"/>
        public override object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null)
        {
            var ret = false;
            if (key.Equals(keyResults.ElementAt((int)Key.RIGHT)))
            {
                value++;
                ret = true;
            }
            else if (key.Equals(keyResults.ElementAt((int)Key.LEFT)))
            {
                value--;
                ret = true;
            }
            if (ret)
            {
                value %= choices.Count();
                if (value < 0)
                {
                    value = choices.Count() - 1;
                }
            }
            return ret;
        }
    }
}
