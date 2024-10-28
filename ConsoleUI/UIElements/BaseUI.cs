using ConsoleUI.Keybinds;
using ConsoleUI.UIElements.EventArgs;
using System.Text;

namespace ConsoleUI.UIElements
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
        public virtual int Value { get; set; }
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

        #region Public properties
        /// <summary>
        /// Returns if the element is selectable.
        /// </summary>
        public virtual bool IsSelectable { get => true; }

        /// <summary>
        /// Returns if the element can be clicked.
        /// </summary>
        public virtual bool IsClickable { get => false; }

        /// <summary>
        /// Returns if the element can only be clicked.
        /// </summary>
        public virtual bool IsOnlyClickable { get => false; }
        #endregion

        #region Event delegates
        /// <summary>
        /// Called before the UI element is displayed.
        /// </summary>
        /// <param name="sender">The UI element that called this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void BeforeTextCreatedEventHandler(BaseUI sender, BeforeTextCreatedEventArgs args);

        /// <summary>
        /// Called after the UI element is displayed.
        /// </summary>
        /// <param name="sender">The UI element that called this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void AfterTextCreatedEventHandler(BaseUI sender, AfterTextCreatedEventArgs args);

        /// <summary>
        /// Called when a key is pressed, when the cursor is over this element.
        /// </summary>
        /// <param name="sender">The UI element that called this event.</param>
        /// <param name="args">The arguments for this event.</param>
        public delegate void KeyPressedEventHandler(BaseUI sender, KeyPressedEventArgs args);
        #endregion

        #region Events
        /// <summary>
        /// Called before the UI element is displayed.
        /// </summary>
        public event BeforeTextCreatedEventHandler BeforeTextCreated;

        /// <summary>
        /// Called after the UI element is displayed.
        /// </summary>
        public event AfterTextCreatedEventHandler AfterTextCreated;

        /// <summary>
        /// Called when a key is pressed, when the cursor is over this element.<br/>
        /// Returns if the input handling should continue (and the menu should refresh).
        /// </summary>
        public event KeyPressedEventHandler KeyPressed;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="BaseUI"/>
        /// </summary>
        /// <param name="value"><inheritdoc cref="Value" path="//summary"/></param>
        /// <param name="preText"><inheritdoc cref="preText" path="//summary"/></param>
        /// <param name="preValue"><inheritdoc cref="preValue" path="//summary"/></param>
        /// <param name="displayValue"><inheritdoc cref="displayValue" path="//summary"/></param>
        /// <param name="postValue"><inheritdoc cref="postValue" path="//summary"/></param>
        /// <param name="multiline"><inheritdoc cref="multiline" path="//summary"/></param>
        public BaseUI(
            int value = 0,
            string preText = "",
            string preValue = "",
            bool displayValue = false,
            string postValue = "",
            bool multiline = false
        )
        {
            Value = value;
            this.preText = preText;
            this.preValue = preValue;
            this.displayValue = displayValue;
            this.postValue = postValue;
            this.multiline = multiline;
        }
        #endregion

        #region EventCallFunctions
        /// <summary>
        /// Calls the <c>BeforeTextCreated</c> event.
        /// </summary>
        protected void RaiseBeforeTextCreatedEvent(BeforeTextCreatedEventArgs args)
        {
            if (BeforeTextCreated is not null)
            {
                BeforeTextCreated(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>AfterTextCreated</c> event.
        /// </summary>
        protected void RaiseAfterTextCreatedEvent(AfterTextCreatedEventArgs args)
        {
            if (AfterTextCreated is not null)
            {
                AfterTextCreated(this, args);
            }
        }

        /// <summary>
        /// Calls the <c>KeyPressed</c> event.
        /// </summary>
        protected void RaiseKeyPressedEvent(KeyPressedEventArgs args)
        {
            if (KeyPressed is not null)
            {
                KeyPressed(this, args);
            }
        }
        #endregion

        #region Virtual methods
        /// <summary>
        /// Returns the text representation of the UI element.
        /// </summary>
        /// <param name="icon">The left icon string to use for this UI element.</param>
        /// <param name="iconR">The right icon string to use for this UI element.</param>
        /// <param name="optionsUI">The <c>OptionsUI</c> containing this object.</param>
        public virtual string MakeText(string icon, string iconR, OptionsUI? optionsUI = null)
        {
            var beforeArgs = new BeforeTextCreatedEventArgs(icon, iconR, optionsUI);
            RaiseBeforeTextCreatedEvent(beforeArgs);
            if (beforeArgs.OverrideText is not null)
            {
                return beforeArgs.OverrideText;
            }

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
            txt.Append(MakeSpecial(icons, optionsUI));

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
                txt.Append(MakeValue(optionsUI));
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
            var createdText = txt.ToString();

            var afterArgs = new AfterTextCreatedEventArgs(createdText, optionsUI);
            RaiseAfterTextCreatedEvent(afterArgs);
            return afterArgs.OverrideText ?? createdText;
        }

        /// <summary>
        /// Handles what to return for the input key.<br/>
        /// </summary>
        /// <param name="key">The triggered key action.</param>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects, that were used.</param>
        /// <param name="getKeyFunction">The function used to get the next valid key the user pressed.</param>
        /// <param name="optionsUI">The <c>OptionsUI</c> containing this object.</param>
        /// <returns>If the screen should update.</returns>
        public virtual object HandleAction(
            KeyAction key,
            IEnumerable<KeyAction> keybinds,
            Utils.GetKeyFunctionDelegate getKeyFunction,
            OptionsUI? optionsUI = null
        )
        {
            var args = new KeyPressedEventArgs(key, keybinds, getKeyFunction, optionsUI);
            RaiseKeyPressedEvent(args);

            if (args.CancelKeyHandling)
            {
                return args.UpdateScreen ?? false;
            }
            return HandleActionProtected(args);
        }

        /// <summary>
        /// Returns the string representation of the cpecial varable.
        /// </summary>
        /// <param name="icons">The icons string to place if <c>multiline</c> is true.</param>
        /// <param name="optionsUI">The <c>OptionsUI</c> containing this object.</param>
        protected virtual string MakeSpecial(string icons, OptionsUI? optionsUI = null)
        {
            return "";
        }

        /// <summary>
        /// Returns the string representation of the value.
        /// </summary>
        /// <param name="optionsUI">The <c>OptionsUI</c> containing this object.</param>
        protected virtual string MakeValue(OptionsUI? optionsUI = null)
        {
            return Value.ToString();
        }

        /// <summary>
        /// Handles what to return for the input key.<br/>
        /// </summary>
        /// <param name="args">The <c>KeyPressedEventArgs</c> containing the arguments for this method.</param>
        /// <returns>If the screen should update.</returns>
        protected virtual object HandleActionProtected(KeyPressedEventArgs args)
        {
            return args.UpdateScreen ?? false;
        }
        #endregion
    }
}
