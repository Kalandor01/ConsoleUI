using System.Text;

namespace SaveFileManager
{
    public class Label : BaseUI
    {
        /// <summary>
        /// Object for the <c>OptionsUI</c> method.<br/>
        /// When used as input in the <c>OptionsUI</c> function, it draws text, that is not selectable.<br/>
        /// Structure: [text]
        /// </summary>
        /// <param name="text">The text to write out.</param>
        public Label(string text)
            : base(-1, text, "", false, "", false) { }

        /// <summary>
        /// Returns the text representation of the UI element.
        /// </summary>
        /// <param name="icon">The left icon string to use for this UI element.</param>
        /// <param name="iconR">The right icon string to use for this UI element.</param>
        /// <returns></returns>
        public override string MakeText(string icon, string iconR)
        {
            return preText + "\n";
        }

        /// <inheritdoc cref="HandleAction(object, IEnumerable{object}, IEnumerable{KeyAction}?)"/>
        public override object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null)
        {
            return false;
        }

        /// <summary>
        /// Returns if the element is selectable.
        /// </summary>
        /// <returns></returns>
        public override bool GetIsSelectable()
        {
            return false;
        }
    }
}
