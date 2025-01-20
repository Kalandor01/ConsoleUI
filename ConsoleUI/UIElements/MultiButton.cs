using ConsoleUI.Keybinds;
using ConsoleUI.UIElements.EventArgs;
using System.Text;

namespace ConsoleUI.UIElements
{
    /// <summary>
    /// Object for the <c>OptionsUI</c> method.<br/>
    /// When used as input in the <c>OptionsUI</c> function, it draws multiple buttons, with all buttons visible at the same time.<br/>
    /// Structure: [<c>preText</c>][<c>active/inactiveButton name</c>][<c>splitter</c>][<c>active/inactiveButton name</c>]...[<c>preValue</c>][<c>value</c>][<c>postValue</c>]
    /// </summary>
    public class MultiButton : BaseUI
    {
        #region Private fields
        /// <summary>
        /// The list of buttons the user can press. The action is the same as <see cref="Button.action"/>
        /// </summary>
        public IList<MultiButtonElement> buttons;

        /// <summary>
        /// The strings to put between the buttons.
        /// </summary>
        public IList<string>? splitters;

        /// <summary>
        /// The splitter string that will be displayed if there aren't enough splitters in the list.
        /// </summary>
        public string defaultSplitter;

        /// <summary>
        /// If its true, and the action invokes a function, it will get a the <c>MultiButton</c> object as its first argument (and can modify it) when the function is called.
        /// </summary>
        public readonly bool modifyList;
        #endregion

        #region Override properties
        /// <inheritdoc cref="BaseUI.IsClickable"/>
        public override bool IsClickable { get => true; }
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="MultiButton"/>
        /// </summary>
        /// <param name="buttons"><inheritdoc cref="buttons" path="//summary"/></param>
        /// <param name="defaultSplitter"><inheritdoc cref="defaultSplitter" path="//summary"/></param>
        /// <param name="splitters"><inheritdoc cref="splitters" path="//summary"/></param>
        /// <param name="value"><inheritdoc cref="BaseUI.Value" path="//summary"/></param>
        /// <param name="preValue"><inheritdoc cref="BaseUI.preValue" path="//summary"/></param>
        /// <param name="postValue"><inheritdoc cref="BaseUI.postValue" path="//summary"/></param>
        /// <param name="modifyList"><inheritdoc cref="modifyList" path="//summary"/></param>
        /// <param name="multiline"><inheritdoc cref="BaseUI.multiline" path="//summary"/></param>
        public MultiButton(
            IList<MultiButtonElement> buttons,
            string defaultSplitter,
            IList<string>? splitters = null,
            int value = 0,
            string preValue = "",
            string postValue = "",
            bool modifyList = false,
            bool multiline = false
        )
            : base(value, preValue, "", false, postValue, multiline)
        {
            this.buttons = buttons;
            this.defaultSplitter = defaultSplitter;
            this.splitters = splitters;
            this.modifyList = modifyList;
        }
        #endregion

        #region Override methods
        /// <inheritdoc cref="BaseUI.MakeSpecial(string, OptionsUI?)"/>
        protected override string MakeSpecial(string icons, OptionsUI? optionsUI = null)
        {
            var splitterCount = splitters?.Count ?? 0;
            var txt = new StringBuilder();
            for (var x = 0; x < buttons.Count; x++)
            {
                txt.Append(x == Value ? buttons[x].activeText : buttons[x].inactiveText);
                if (x < buttons.Count - 1)
                {
                    txt.Append(splitterCount > x ? splitters?[x] : defaultSplitter);
                }
            }
            return multiline ? txt.Replace("\n", icons).ToString() : txt.ToString();
        }

        /// <inheritdoc cref="BaseUI.HandleActionProtected(UIKeyPressedEventArgs)"/>
        protected override object HandleActionProtected(UIKeyPressedEventArgs args)
        {
            if (
                args.pressedKey.Equals(args.keybinds.ElementAt((int)Key.RIGHT)) ||
                args.pressedKey.Equals(args.keybinds.ElementAt((int)Key.LEFT))
            )
            {
                Value += args.pressedKey.Equals(args.keybinds.ElementAt((int)Key.RIGHT)) ? 1 : -1;

                Value %= buttons.Count;
                if (Value < 0)
                {
                    Value = buttons.Count - 1;
                }
                return args.UpdateScreen ?? true;
            }
            else if (!args.pressedKey.Equals(args.keybinds.ElementAt((int)Key.ENTER)))
            {
                return args.UpdateScreen ?? false;
            }

            var (actionType, returned) = buttons[Value].action.InvokeAction(modifyList ? this : null, args.keybinds, args.getKeyFunction);
            // function
            if (actionType is UIActionType.FUNCTION)
            {
                return returned is null ? (args.UpdateScreen ?? true) : returned;
            }
            return args.UpdateScreen ?? true;
        }
        #endregion
    }
}
