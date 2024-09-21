using ConsoleUI.Keybinds;

namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>BeforeExiting</c> event.
    /// </summary>
    public class BeforeExitingEventArgs
    {
        /// <summary>
        /// The value that will be returned.
        /// </summary>
        public readonly object? returnValue;

        /// <summary>
        /// If this exit was triggered by a <see cref="BaseUI.HandleAction(KeyAction, IEnumerable{KeyAction}, OptionsUI?)"/>.<br/>
        /// If this value if false, the exiting can't be canceled.
        /// </summary>
        public readonly bool isTriggeredByUI;

        /// <summary>
        /// Whether to cancel the exiting.
        /// </summary>
        public bool CancelExiting { get; set; }
        /// <summary>
        /// Whether to update the screen if the exiting is canceled.
        /// </summary>
        public bool? UpdateScreen { get; set; }

        /// <summary>
        /// <inheritdoc cref="BeforeExitingEventArgs"/>
        /// </summary>
        /// <param name="returnValue"><inheritdoc cref="returnValue" path="//summary"/></param>
        /// <param name="isTriggeredByUI"><inheritdoc cref="isTriggeredByUI" path="//summary"/></param>
        /// <param name="cancelExiting"><inheritdoc cref="CancelExiting" path="//summary"/></param>
        /// <param name="updateScreen"><inheritdoc cref="UpdateScreen" path="//summary"/></param>
        public BeforeExitingEventArgs(
            object? returnValue,
            bool isTriggeredByUI,
            bool cancelExiting = false,
            bool? updateScreen = null
        )
        {
            this.returnValue = returnValue;
            this.isTriggeredByUI = isTriggeredByUI;
            CancelExiting = cancelExiting;
            UpdateScreen = updateScreen;
        }
    }
}
