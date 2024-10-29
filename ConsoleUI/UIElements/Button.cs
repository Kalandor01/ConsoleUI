using ConsoleUI.Keybinds;
using ConsoleUI.UIElements.EventArgs;

namespace ConsoleUI.UIElements
{
    /// <summary>
    /// Object for the <c>OptionsUI</c> method.<br/>
    /// When used as input in the <c>OptionsUI</c> function, it text that is pressable with the enter key.<br/>
    /// Structure: [<c>text</c>]
    /// </summary>
    public class Button : BaseUI
    {
        #region Private fields
        /// <summary>
        /// If its true, and the action invokes a function, it will get a the <c>Button</c> object as its first argument (and can modify it) when the function is called.
        /// </summary>
        readonly bool modifyList;
        /// <summary>
        /// The action to invoke when the button is pressed.<br/>
        /// - If the action invokes a function, and returns false the UI will not update.<br/>
        /// - If the function returns anything other than a bool, the <c>OptionsUI</c> will instantly return that value.
        /// </summary>
        readonly UIAction action;
        #endregion

        #region Override properties
        /// <inheritdoc cref="BaseUI.IsClickable"/>
        public override bool IsClickable { get => true; }

        /// <inheritdoc cref="BaseUI.IsOnlyClickable"/>
        public override bool IsOnlyClickable { get => true; }
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="Button"/>
        /// </summary>
        /// <param name="multiline"><inheritdoc cref="BaseUI.multiline" path="//summary"/></param>
        /// <param name="text">The text to write out.</param>
        /// <param name="action"><inheritdoc cref="action" path="//summary"/></param>
        /// <param name="modifyList"><inheritdoc cref="modifyList" path="//summary"/></param>
        public Button(UIAction action, bool modifyList = false, string text = "", bool multiline = false)
            : base(-1, text, "", false, "", multiline)
        {
            this.modifyList = modifyList;
            this.action = action;
        }
        #endregion

        #region Override methods
        /// <inheritdoc cref="BaseUI.HandleActionProtected(UIKeyPressedEventArgs)"/>
        protected override object HandleActionProtected(UIKeyPressedEventArgs args)
        {
            if (!args.pressedKey.Equals(args.keybinds.ElementAt((int)Key.ENTER)))
            {
                return args.UpdateScreen ?? false;
            }

            var (actionType, returned) = action.InvokeAction(modifyList ? this : null, args.keybinds, args.getKeyFunction);
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
