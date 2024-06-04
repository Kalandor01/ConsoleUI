using ConsoleUI.Keybinds;

namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>KeyPressed</c> event.
    /// </summary>
    public class KeyPressedEventArgs
    {
        /// <summary>
        /// The sender of the event.
        /// </summary>
        public readonly BaseUI sender;
        /// <summary>
        /// The key action, the user triggered.
        /// </summary>
        public readonly KeyAction pressedKey;
        /// <summary>
        /// The keybinds used.
        /// </summary>
        public readonly IEnumerable<KeyAction> keybinds;
        /// <summary>
        /// The OptionsUI containing the UI element.
        /// </summary>
        public readonly OptionsUI? optionsUI;

        /// <summary>
        /// Whether to immedietly exit the key handling function.
        /// </summary>
        public bool CancelKeyHandling { get; set; }
        /// <summary>
        /// Whether to update the screen after exiting the key handling function.
        /// </summary>
        public bool? UpdateScreen { get; set; }

        /// <summary>
        /// <inheritdoc cref="KeyPressedEventArgs"/>
        /// </summary>
        /// <param name="sender"><inheritdoc cref="sender" path="//summary"/></param>
        /// <param name="pressedKey"><inheritdoc cref="pressedKey" path="//summary"/></param>
        /// <param name="keybinds"><inheritdoc cref="keybinds" path="//summary"/></param>
        /// <param name="optionsUI"><inheritdoc cref="optionsUI" path="//summary"/></param>
        /// <param name="cancelKeyHandling"><inheritdoc cref="CancelKeyHandling" path="//summary"/></param>
        /// <param name="updateScreen"><inheritdoc cref="UpdateScreen" path="//summary"/></param>
        public KeyPressedEventArgs(
            BaseUI sender,
            KeyAction pressedKey,
            IEnumerable<KeyAction> keybinds,
            OptionsUI? optionsUI = null,
            bool cancelKeyHandling = false,
            bool? updateScreen = null
        )
        {
            this.sender = sender;
            this.pressedKey = pressedKey;
            this.keybinds = keybinds;
            this.optionsUI = optionsUI;
            CancelKeyHandling = cancelKeyHandling;
            UpdateScreen = updateScreen;
        }
    }
}
