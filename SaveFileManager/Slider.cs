using System.Text;

namespace SaveFileManager
{
    public class Slider : BaseUI
    {
        protected int minValue;
        protected int maxValue;
        protected int step;
        protected string symbol;
        protected string symbolEmpty;

        /// <summary>
        /// Object for the options_ui method.<br/>
        /// When used as input in the <c>OptionsUI</c> function, it draws a slider, with the section specifying it's characteristics.<br/>
        /// Structure: [preText][symbol and symbolEmpty][preValue][value][postValue]
        /// </summary>
        /// <param name="minValue">The maximum value of the slider.</param>
        /// <param name="maxValue">The minimum value of the slider.</param>
        /// <param name="step">The amount, the value will change by, when the user changes the value of the slider.</param>
        /// <param name="symbol">The symbol that will represent a filled space in the slider's progress bar.</param>
        /// <param name="symbolEmpty">The symbol that will represent an empty space in the slider's progress bar.</param>
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

        /// <inheritdoc cref="MakeSpecial(string)"/>
        protected string MakeSpecial(string icons)
        {
            var txt = new StringBuilder();
            for (var x = minValue; x < maxValue / step; x += step)
            {
                txt.Append(x >= value ? symbolEmpty : symbol);
            }
            return txt.ToString();
        }

        /// <inheritdoc cref="HandleAction(object, IEnumerable{object}, IEnumerable{KeyAction}?)"/>
        public bool HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null)
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
            else if (key.Equals(keyResults.ElementAt((int)Key.RIGHT)))
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
    }
}
