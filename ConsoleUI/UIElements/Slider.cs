using ConsoleUI.Keybinds;
using ConsoleUI.UIElements.EventArgs;
using System.Text;

namespace ConsoleUI.UIElements
{
    /// <summary>
    /// Object for the <c>OptionsUI</c> method.<br/>
    /// When used as input in the <c>OptionsUI</c> function, it draws a slider, with the section specifying its characteristics.<br/>
    /// Structure: [<c>preText</c>][<c>symbol</c> or <c>symbolEmpty</c>][<c>preValue</c>][<c>value</c>][<c>postValue</c>]
    /// </summary>
    public class Slider : BaseUI
    {
        #region Protected fields
        /// <summary>
        /// The maximum value of the slider.
        /// </summary>
        protected int minValue;
        /// <summary>
        /// The minimum value of the slider.
        /// </summary>
        protected int maxValue;
        /// <summary>
        /// The amount, the value will change by, when the user changes the value of the slider.
        /// </summary>
        protected int step;
        /// <summary>
        /// The symbol that will represent a filled space in the slider's progress bar.
        /// </summary>
        protected string symbol;
        /// <summary>
        /// The symbol that will represent an empty space in the slider's progress bar.
        /// </summary>
        protected string symbolEmpty;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="Slider" path="//summary"/>
        /// </summary>
        /// <param name="value"><inheritdoc cref="BaseUI.Value" path="//summary"/></param>
        /// <param name="displayValue"><inheritdoc cref="BaseUI.displayValue" path="//summary"/></param>
        /// <param name="preText"><inheritdoc cref="BaseUI.preText" path="//summary"/></param>
        /// <param name="preValue"><inheritdoc cref="BaseUI.preValue" path="//summary"/></param>
        /// <param name="postValue"><inheritdoc cref="BaseUI.postValue" path="//summary"/></param>
        /// <param name="multiline"><inheritdoc cref="BaseUI.multiline" path="//summary"/></param>
        /// <param name="minValue"><inheritdoc cref="minValue" path="//summary"/></param>
        /// <param name="maxValue"><inheritdoc cref="maxValue" path="//summary"/></param>
        /// <param name="step"><inheritdoc cref="step" path="//summary"/></param>
        /// <param name="symbol"><inheritdoc cref="symbol" path="//summary"/></param>
        /// <param name="symbolEmpty"><inheritdoc cref="symbolEmpty" path="//summary"/></param>
        public Slider(int minValue, int maxValue, int step = 1, int value = 0, string preText = "", string symbol = "#", string symbolEmpty = "-", string preValue = "", bool displayValue = false, string postValue = "", bool multiline = false)
            : base(Math.Clamp(value, Math.Min(minValue, maxValue), Math.Max(minValue, maxValue)), preText, preValue, displayValue, postValue, multiline)
        {
            this.minValue = Math.Min(minValue, maxValue);
            this.maxValue = Math.Max(minValue, maxValue);
            this.step = step;
            this.symbol = symbol;
            this.symbolEmpty = symbolEmpty;
        }
        #endregion

        #region Override methods
        /// <inheritdoc cref="BaseUI.MakeSpecial(string, OptionsUI?)"/>
        protected override string MakeSpecial(string icons, OptionsUI? optionsUI = null)
        {
            var txt = new StringBuilder();
            for (var x = minValue; x < maxValue; x += step)
            {
                txt.Append(x >= Value ? symbolEmpty : symbol);
            }
            return txt.ToString();
        }

        /// <inheritdoc cref="BaseUI.HandleActionProtected(KeyPressedEventArgs)"/>
        protected override object HandleActionProtected(KeyPressedEventArgs args)
        {
            if (args.pressedKey.Equals(args.keybinds.ElementAt((int)Key.RIGHT)))
            {
                if (Value + step > maxValue)
                {
                    return args.UpdateScreen ?? false;
                }
                Value += step;
            }
            else if (args.pressedKey.Equals(args.keybinds.ElementAt((int)Key.LEFT)))
            {
                if (Value - step < minValue)
                {
                    return args.UpdateScreen ?? false;
                }
                Value -= step;
            }
            return args.UpdateScreen ?? true;
        }
        #endregion
    }
}
