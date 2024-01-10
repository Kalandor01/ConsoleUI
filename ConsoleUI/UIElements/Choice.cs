using ConsoleUI.Keybinds;
using ConsoleUI.UIElements.EventArgs;

namespace ConsoleUI.UIElements
{
    /// <summary>
    /// Object for the <c>OptionsUI</c> method.<br/>
    /// When used as input in the <c>OptionsUI</c> function, it draws a multiple choice seletion, with the <c>choices</c> list specifying the choice names.<br/>
    /// Structure: [<c>preText</c>][<c>choice name</c>][<c>preValue</c>][<c>value</c>][<c>postValue</c>]
    /// </summary>
    public class Choice : BaseUI
    {
        #region Private fields
        /// <summary>
        /// The list of options the user can choose from.
        /// </summary>
        readonly IEnumerable<string> choices;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="Choice"/>
        /// </summary>
        /// <param name="value"><inheritdoc cref="BaseUI.Value" path="//summary"/></param>
        /// <param name="displayValue"><inheritdoc cref="BaseUI.displayValue" path="//summary"/></param>
        /// <param name="preText"><inheritdoc cref="BaseUI.preText" path="//summary"/></param>
        /// <param name="preValue"><inheritdoc cref="BaseUI.preValue" path="//summary"/></param>
        /// <param name="postValue"><inheritdoc cref="BaseUI.postValue" path="//summary"/></param>
        /// <param name="multiline"><inheritdoc cref="BaseUI.multiline" path="//summary"/></param>
        /// <param name="choices"><inheritdoc cref="choices" path="//summary"/></param>
        public Choice(IEnumerable<string> choices, int value = 0, string preText = "", string preValue = "", bool displayValue = false, string postValue = "", bool multiline = false)
            : base(Math.Clamp(value, 0, choices.Count() - 1), preText, preValue, displayValue, postValue, multiline)
        {
            this.choices = choices;
        }
        #endregion

        #region Override methods
        /// <inheritdoc cref="BaseUI.MakeSpecial(string, OptionsUI?)"/>
        protected override string MakeSpecial(string icons, OptionsUI? optionsUI = null)
        {
            if (multiline)
            {
                return choices.ElementAt(Value).Replace("\n", icons);
            }
            else
            {
                return choices.ElementAt(Value);
            }
        }

        /// <inheritdoc cref="BaseUI.MakeValue(OptionsUI?)"/>
        protected override string MakeValue(OptionsUI? optionsUI = null)
        {
            return $"{Value + 1}/{choices.Count()}";
        }

        /// <inheritdoc cref="BaseUI.HandleAction(KeyAction, IEnumerable{KeyAction}, OptionsUI?)"/>
        public override object HandleAction(KeyAction key, IEnumerable<KeyAction> keybinds, OptionsUI? optionsUI = null)
        {
            var args = new KeyPressedEvenrArgs(key, keybinds);
            RaiseKeyPressedEvent(args);
            if (args.CancelKeyHandling)
            {
                return args.UpdateScreen ?? false;
            }

            var returnValue = false;
            if (
                key.Equals(keybinds.ElementAt((int)Key.RIGHT)) ||
                key.Equals(keybinds.ElementAt((int)Key.LEFT))
            )
            {
                returnValue = true;
                Value += key.Equals(keybinds.ElementAt((int)Key.RIGHT)) ? 1 : -1;
            }
            if (returnValue)
            {
                Value %= choices.Count();
                if (Value < 0)
                {
                    Value = choices.Count() - 1;
                }
            }
            return args.UpdateScreen ?? returnValue;
        }
        #endregion
    }
}
