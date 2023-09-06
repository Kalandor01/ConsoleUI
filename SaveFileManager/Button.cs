namespace SaveFileManager
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
        /// <inheritdoc cref="BaseUI.HandleAction"/>
        public override object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null, OptionsUI? optionsUI = null)
        {
            if (key.Equals(keyResults.ElementAt((int)Key.ENTER)))
            {
                var (actionType, returned) = action.InvokeAction(modifyList ? this : null, keybinds, keyResults);
                // function
                if (actionType is UIActionType.FUNCTION)
                {
                    return returned is null ? true : returned;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc cref="BaseUI.IsClickable"/>
        public override bool IsClickable()
        {
            return true;
        }

        /// <inheritdoc cref="BaseUI.IsOnlyClickable"/>
        public override bool IsOnlyClickable()
        {
            return true;
        }
        #endregion
    }
}
