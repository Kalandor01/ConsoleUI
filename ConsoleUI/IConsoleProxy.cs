namespace ConsoleUI
{
    /// <summary>
    /// Interface for a proxy to a console or other output.
    /// </summary>
    public interface IConsoleProxy
    {
        #region Properties
        /// <summary>
        /// The width of the console.
        /// </summary>
        public int ConsoleWidth { get; }

        /// <summary>
        /// The height of the console.
        /// </summary>
        public int ConsoleHeight { get; }

        /// <summary>
        /// The column offset of the cursor from left of the console.
        /// </summary>
        public int CursorColumn { get; set; }

        /// <summary>
        /// The row offset of the cursor from top of the console.
        /// </summary>
        public int CursorRow { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Writes the text representation of the value to the output.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Write(object? value);

        /// <summary>
        /// Writes text to the output.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void Write(string? text);

        /// <summary>
        /// Writes the text representation of the value to the output followed by a newline.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLine(object? value);

        /// <summary>
        /// Writes text to the output followed by a newline.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void WriteLine(string? text);

        /// <summary>
        /// Writes newline to the output.
        /// </summary>
        public void WriteLine();

        /// <summary>
        /// Reads a key from the console.
        /// </summary>
        /// <param name="displayKey">True to display the pressed key.</param>
        public ConsoleKeyInfo ReadKey(bool displayKey = true);

        /// <summary>
        /// Reads a string from the console, or null.
        /// </summary>
        public string? ReadLine();

        /// <summary>
        /// Returns the current position of the cursor from the top left corner.
        /// </summary>
        public (int column, int row) GetCursorPosition();

        /// <summary>
        /// Sets the position of the cursor from the top left corner.
        /// </summary>
        public void SetCursorPosition(int column, int row);

        /// <summary>
        /// Writes out text, and then waits for a key press.
        /// </summary>
        /// <param name="text">The text to write out.</param>
        /// <param name="displayKey">True to display the pressed key.</param>
        public void PressKey(string text, bool displayKey = false);

        /// <summary>
        /// Offsets the position of the cursor.
        /// </summary>
        /// <param name="columnOffset">The column offset of the cursor.</param>
        /// <param name="rowOffset">The row offset of the cursor.</param>
        public void MoveCursor(int columnOffset, int rowOffset);

        /// <summary>
        /// Writes test at a specified position and optionaly returns the cusor to it's original position.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="column">The column position to write to.</param>
        /// <param name="row">The row position to write to.</param>
        /// <param name="returnCursor">Whether to return the cursor to it's prewious position after writing.</param>
        public void WriteAtPosition(string text, int column, int row, bool returnCursor = false);
        #endregion
    }
}
