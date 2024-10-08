﻿namespace ConsoleUI.UIElements.EventArgs
{
    /// <summary>
    /// Class for storing the arguments of the <c>AfterTextCreated</c> event.
    /// </summary>
    public class AfterTextCreatedEventArgs
    {
        /// <summary>
        /// The text the UI element created.
        /// </summary>
        public readonly string createdText;
        /// <summary>
        /// The OptionsUI containing the UI element.
        /// </summary>
        public readonly OptionsUI? selectedUI;

        /// <summary>
        /// If not null, halts text creartion and returns this text instead.
        /// </summary>
        public string? OverrideText { get; set; }

        /// <summary>
        /// <inheritdoc cref="AfterTextCreatedEventArgs"/>
        /// </summary>
        /// <param name="createdText"><inheritdoc cref="createdText" path="//summary"/></param>
        /// <param name="selectedUI"><inheritdoc cref="selectedUI" path="//summary"/></param>
        /// <param name="overrideText"><inheritdoc cref="OverrideText" path="//summary"/></param>
        public AfterTextCreatedEventArgs(
            string createdText,
            OptionsUI? selectedUI = null,
            string? overrideText = null
        )
        {
            this.createdText = createdText;
            this.selectedUI = selectedUI;
            OverrideText = overrideText;
        }
    }
}
