using System.Text;

namespace SaveFileManager
{
    public abstract class BaseUI
    {
        protected int value;
        protected string preText;
        protected string preValue;
        protected bool displayValue;
        protected string postValue;
        protected bool multiline;

        /// <summary>
        /// Base class for all classes used in the <c>OptionsUI</c> method.<br/>
        /// General structure: [preText][#####][preValue][value][postValue]
        /// </summary>
        /// <param name="value">The current value of the object.</param>
        /// <param name="preText">The text to display before the class specific text (which might be nothing).</param>
        /// <param name="preValue">The text to display after the <c>value</c>.</param>
        /// <param name="displayValue">Whether to display the <c>value</c> or not.</param>
        /// <param name="postValue">The text to display after the <c>value</c>.</param>
        /// <param name="multiline">Makes the "cursor" draw at every line if the text is multiline.</param>
        public BaseUI(int value = 0, string preText = "", string preValue = "", bool displayValue = false, string postValue = "", bool multiline = false)
        {
            this.value = value;
            this.preText = preText;
            this.preValue = preValue;
            this.displayValue = displayValue;
            this.postValue = postValue;
            this.multiline = multiline;
        }

        /// <summary>
        /// Returns the text representation of the UI element.
        /// </summary>
        /// <param name="icon">The left icon string to use for this UI element.</param>
        /// <param name="iconR">The right icon string to use for this UI element.</param>
        /// <returns></returns>
        public string MakeText(string icon, string iconR)
        {
            var txt = new StringBuilder();
            // current icon group
            var icons = $"{iconR}\n{icon}";
            // icon
            txt.Append(icon);
            // pre text
            if (multiline)
            {
                txt.Append(preText.Replace("\n", icons));
            }
            else
            {
                txt.Append(preText);
            }
            // special
            txt.Append(MakeSpecial(icons));
            // pre value
            if (multiline)
            {
                txt.Append(preValue.Replace("\n", icons));
            }
            else
            {
                txt.Append(preValue);
            }
            // value
            if (displayValue)
            {
                txt.Append(MakeValue());
            }
            // post value
            if (multiline)
            {
                txt.Append(postValue.Replace("\n", icons));
            }
            else
            {
                txt.Append(postValue);
            }
            // icon right
            txt.Append(iconR + "\n");
            return txt.ToString();
        }

        /// <summary>
        /// Returns the string representation of the cpecial varable.
        /// </summary>
        /// <param name="icons">The icons string to place if <c>multiline</c> is true.</param>
        /// <returns></returns>
        protected string MakeSpecial(string icons)
        {
            return "";
        }

        /// <summary>
        /// Returns the string representation of the value.
        /// </summary>
        /// <returns></returns>
        protected string MakeValue()
        {
            return value.ToString();
        }

        /// <summary>
        /// Handles what to return for the input key.<br/>
        /// Returns if the screen should update.
        /// </summary>
        /// <param name="key">The result object retrned from the key the user pressed.</param>
        /// <param name="keyResults">The list of posible results returned by pressing a key.</param>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use, if the selected action is a <c>UIList</c>.</param>
        /// <returns></returns>
        public bool HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null)
        {
            return true;
        }
    }
}
