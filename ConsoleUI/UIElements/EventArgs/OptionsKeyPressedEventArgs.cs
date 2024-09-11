using ConsoleUI.Keybinds;

namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>KeyPressed</c> event.
    /// </summary>
    public class OptionsKeyPressedEventArgs
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
        /// Whether to immedietly stop the key handling.
        /// </summary>
        public bool CancelKeyHandling { get; set; }
        /// <summary>
        /// Whether to update the screen after the key handling ends.
        /// </summary>
        public bool? UpdateScreen { get; set; }

        /// <summary>
        /// <inheritdoc cref="OptionsKeyPressedEventArgs"/>
        /// </summary>
        /// <param name="pressedKey"><inheritdoc cref="pressedKey" path="//summary"/></param>
        /// <param name="keybinds"><inheritdoc cref="keybinds" path="//summary"/></param>
        /// <param name="cancelKeyHandling"><inheritdoc cref="CancelKeyHandling" path="//summary"/></param>
        /// <param name="updateScreen"><inheritdoc cref="UpdateScreen" path="//summary"/></param>
        public OptionsKeyPressedEventArgs(
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
