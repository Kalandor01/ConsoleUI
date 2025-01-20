using ConsoleUI.Keybinds;
using ConsoleUI.UIElements.EventArgs;

namespace ConsoleUI.UIElements
{
    /// <summary>
    /// Object for the <c>OptionsUI</c> method.<br/>
    /// When used as input in the <c>OptionsUI</c> function, it draws a field that is toggleable with the key associated with the enter action.<br/>
    /// Structure: [<c>preText</c>][<c>symbol</c> or <c>symbolOff</c>][<c>postValue</c>]
    /// </summary>
    public class Toggle : BaseUI
    {
        #region Event delegates
        /// <summary>
        /// Called when a key is pressed, when the cursor is over this element.
        /// </summary>
        /// <param name="sender">The UI element that called this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void ToggledEventHandler(BaseUI sender, UIKeyPressedEventArgs args);
        #endregion

        #region Events
        /// <summary>
        /// Called when a the toggle key is pressed, when the cursor is over this element.<br/>
        /// Returns if the input handling should continue (and the menu should refresh).
        /// </summary>
        public event ToggledEventHandler Toggled;
        #endregion

        #region Private fields
        /// <summary>
        /// The current value of the object.
        /// </summary>
        public new bool Value { get; set; }
        /// <summary>
        /// The text displayed when the toggle is on.
        /// </summary>
        string symbol;
        /// <summary>
        /// The text displayed when the toggle is off.
        /// </summary>
        string symbolOff;
        #endregion

        #region Override properties
        /// <inheritdoc cref="BaseUI.IsClickable"/>
        public override bool IsClickable { get => true; }

        /// <inheritdoc cref="BaseUI.IsOnlyClickable"/>
        public override bool IsOnlyClickable { get => true; }
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="Toggle"/>
        /// </summary>
        /// <param name="value"><inheritdoc cref="Value" path="//summary"/></param>
        /// <param name="preText"><inheritdoc cref="BaseUI.preText" path="//summary"/></param>
        /// <param name="postValue"><inheritdoc cref="BaseUI.postValue" path="//summary"/></param>
        /// <param name="multiline"><inheritdoc cref="BaseUI.multiline" path="//summary"/></param>
        /// <param name="symbol"><inheritdoc cref="symbol" path="//summary"/></param>
        /// <param name="symbolOff"><inheritdoc cref="symbolOff" path="//summary"/></param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
        public Toggle(bool value = false, string preText = "", string symbol = "on", string symbolOff = "off", string postValue = "", bool multiline = false)
            : base(value ? 1 : 0, preText, "", false, postValue, multiline)
        {
            Value = value;
            this.symbol = symbol;
            this.symbolOff = symbolOff;
        }
        #endregion

        #region EventCallFunctions
        /// <summary>
        /// Calls the <c>Toggled</c> event.
        /// </summary>
        protected void RaiseToggledEvent(UIKeyPressedEventArgs args)
        {
            if (Toggled is not null)
            {
                Toggled(this, args);
            }
        }
        #endregion

        #region Override methods
        /// <inheritdoc cref="BaseUI.MakeSpecial(string, OptionsUI?)"/>
        protected override string MakeSpecial(string icons, OptionsUI? optionsUI = null)
        {
            return Value ? symbol : symbolOff;
        }

        /// <inheritdoc cref="BaseUI.HandleActionProtected(UIKeyPressedEventArgs)"/>
        protected override object HandleActionProtected(UIKeyPressedEventArgs args)
        {
            if (args.pressedKey.Equals(args.keybinds.ElementAt((int)Key.ENTER)))
            {
                RaiseToggledEvent(args);
                Value = !Value;
                base.Value = Value ? 1 : 0;
            }
            return args.UpdateScreen ?? true;
        }
        #endregion
    }
}
