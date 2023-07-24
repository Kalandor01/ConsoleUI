using System.Text;

namespace SaveFileManager
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
        /// <param name="minValue"><inheritdoc cref="minValue" path="//summary"/></param>
        /// <param name="maxValue"><inheritdoc cref="maxValue" path="//summary"/></param>
        /// <param name="step"><inheritdoc cref="step" path="//summary"/></param>
        /// <param name="symbol"><inheritdoc cref="symbol" path="//summary"/></param>
        /// <param name="symbolEmpty"><inheritdoc cref="symbolEmpty" path="//summary"/></param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
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
        /// <inheritdoc cref="BaseUI.MakeSpecial"/>
        protected override string MakeSpecial(string icons, IEnumerable<BaseUI?>? elementList = null)
        {
            var txt = new StringBuilder();
            for (var x = minValue; x < maxValue; x += step)
            {
                txt.Append(x >= value ? symbolEmpty : symbol);
            }
            return txt.ToString();
        }

        /// <inheritdoc cref="BaseUI.HandleAction"/>
        public override object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null, OptionsUI? optionsUI = null)
        {
            if (key.Equals(keyResults.ElementAt((int)Key.RIGHT)))
            {
                if (value + step <= maxValue)
                {
                    value += step;
                }
                else
                {
                    return false;
                }
            }
            else if (key.Equals(keyResults.ElementAt((int)Key.LEFT)))
            {
                if (value - step >= minValue)
                {
                    value -= step;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
