using ConsoleUI.Keybinds;

namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>KeyPressed</c> event.
    /// </summary>
    public class UIKeyPressedEventArgs
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
        /// The function used to get the next valid key the user pressed.
        /// </summary>
        public readonly Utils.GetKeyFunctionDelegate getKeyFunction;

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
        /// <inheritdoc cref="UIKeyPressedEventArgs"/>
        /// </summary>
        /// <param name="pressedKey"><inheritdoc cref="pressedKey" path="//summary"/></param>
        /// <param name="keybinds"><inheritdoc cref="keybinds" path="//summary"/></param>
        /// <param name="getKeyFunction"><inheritdoc cref="getKeyFunction" path="//summary"/></param>
        /// <param name="optionsUI"><inheritdoc cref="optionsUI" path="//summary"/></param>
        /// <param name="cancelKeyHandling"><inheritdoc cref="CancelKeyHandling" path="//summary"/></param>
        /// <param name="updateScreen"><inheritdoc cref="UpdateScreen" path="//summary"/></param>
        public UIKeyPressedEventArgs(
            KeyAction pressedKey,
            IEnumerable<KeyAction> keybinds,
            Utils.GetKeyFunctionDelegate getKeyFunction,
            OptionsUI? optionsUI = null,
            bool cancelKeyHandling = false,
            bool? updateScreen = null
        )
        {
            this.pressedKey = pressedKey;
            this.keybinds = keybinds;
            this.getKeyFunction = getKeyFunction;
            this.optionsUI = optionsUI;
            CancelKeyHandling = cancelKeyHandling;
            UpdateScreen = updateScreen;
        }
    }
}
