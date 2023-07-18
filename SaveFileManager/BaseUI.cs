using System.Text;

namespace SaveFileManager
{
    /// <summary>
    /// Abstract class for all classes used in the <c>OptionsUI</c> method.<br/>
    /// General structure: [<c>preText</c>][#####][<c>preValue</c>][<c>value</c>][<c>postValue</c>]
    /// </summary>
    public abstract class BaseUI
    {
        #region Public fields
        /// <summary>
        /// The current value of the object.
        /// </summary>
        public int value;
        #endregion

        #region Protected fields
        /// <summary>
        /// The text to display before the class specific text (which might be nothing).
        /// </summary>
        protected string preText;
        /// <summary>
        /// The text to display before the <c>value</c>.
        /// </summary>
        protected string preValue;
        /// <summary>
        /// Whether to display the <c>value</c> or not.
        /// </summary>
        protected bool displayValue;
        /// <summary>
        /// The text to display after the <c>value</c>.
        /// </summary>
        protected string postValue;
        /// <summary>
        /// Makes the "cursor" draw at every line if the text is multiline.
        /// </summary>
        protected bool multiline;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="BaseUI"/>
        /// </summary>
        /// <param name="value"><inheritdoc cref="value" path="//summary"/></param>
        /// <param name="preText"><inheritdoc cref="preText" path="//summary"/></param>
        /// <param name="preValue"><inheritdoc cref="preValue" path="//summary"/></param>
        /// <param name="displayValue"><inheritdoc cref="displayValue" path="//summary"/></param>
        /// <param name="postValue"><inheritdoc cref="postValue" path="//summary"/></param>
        /// <param name="multiline"><inheritdoc cref="multiline" path="//summary"/></param>
        public BaseUI(int value = 0, string preText = "", string preValue = "", bool displayValue = false, string postValue = "", bool multiline = false)
        {
            this.value = value;
            this.preText = preText;
            this.preValue = preValue;
            this.displayValue = displayValue;
            this.postValue = postValue;
            this.multiline = multiline;
        }
        #endregion

        #region Virtual methods
        /// <summary>
        /// Returns the text representation of the UI element.
        /// </summary>
        /// <param name="icon">The left icon string to use for this UI element.</param>
        /// <param name="iconR">The right icon string to use for this UI element.</param>
        /// <param name="elementList">The list of elements passed into <c>OptionsUI</c>.</param>
        public virtual string MakeText(string icon, string iconR, IEnumerable<BaseUI?>? elementList = null)
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
            txt.Append(MakeSpecial(icons, elementList));
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
                txt.Append(MakeValue(elementList));
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
        /// <param name="elementList">The list of elements passed into <c>OptionsUI</c>.</param>
        protected virtual string MakeSpecial(string icons, IEnumerable<BaseUI?>? elementList = null)
        {
            return "";
        }

        /// <summary>
        /// Returns the string representation of the value.
        /// </summary>
        /// <param name="elementList">The list of elements passed into <c>OptionsUI</c>.</param>
        protected virtual string MakeValue(IEnumerable<BaseUI?>? elementList = null)
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
        /// <param name="elementList">The list of elements passed into <c>OptionsUI</c>.</param>
        public virtual object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null, IEnumerable<BaseUI?>? elementList = null)
        {
            return true;
        }

        /// <summary>
        /// Returns if the element is selectable.
        /// </summary>
        public virtual bool IsSelectable()
        {
            return true;
        }

        /// <summary>
        /// Returns if the element can only be clicked.
        /// </summary>
        public virtual bool IsOnlyClickable()
        {
            return false;
        }
        #endregion
    }
}
