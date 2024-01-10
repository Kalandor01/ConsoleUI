using ConsoleUI.Keybinds;

namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>KeyPressed</c> event.
    /// </summary>
    public class KeyPressedEvenrArgs
    {
        /// <summary>
        /// The key action, the user triggered.
        /// </summary>
        public readonly KeyAction pressedKey;
        /// <summary>
        /// The keybinds used.
        /// </summary>
        public readonly IEnumerable<KeyAction> keybinds;

        /// <summary>
        /// Whether to immedietly exit the key handling function.
        /// </summary>
        public bool CancelKeyHandling { get; set; }
        /// <summary>
        /// Whether to update the screen after exiting the key handling function.
        /// </summary>
        public bool? UpdateScreen { get; set; }

        /// <summary>
        /// <inheritdoc cref="KeyPressedEvenrArgs"/>
        /// </summary>
        /// <param name="pressedKey"><inheritdoc cref="pressedKey" path="//summary"/></param>
        /// <param name="keybinds"><inheritdoc cref="keybinds" path="//summary"/></param>
        /// <param name="cancelKeyHandling"><inheritdoc cref="CancelKeyHandling" path="//summary"/></param>
        /// <param name="updateScreen"><inheritdoc cref="UpdateScreen" path="//summary"/></param>
        public KeyPressedEvenrArgs(
            KeyAction pressedKey,
            IEnumerable<KeyAction> keybinds,
            bool cancelKeyHandling = false,
            bool? updateScreen = null
        )
        {
            this.pressedKey = pressedKey;
            this.keybinds = keybinds;
            CancelKeyHandling = cancelKeyHandling;
            UpdateScreen = updateScreen;
        }
    }
}
