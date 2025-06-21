namespace ConsoleUI
{
    /// <summary>
    /// Class for a proxy to the console.
    /// </summary>
    public class ConsoleProxy : IConsoleProxy
    {
        #region Properties
        /// <inheritdoc/>
        public int ConsoleWidth => Console.BufferWidth;

        /// <inheritdoc/>
        public int ConsoleHeight => Console.BufferHeight;

        /// <inheritdoc/>
        public int CursorColumn
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        /// <inheritdoc/>
        public int CursorRow
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public void Write(object? value)
        {
            Console.Write(value);
        }

        /// <inheritdoc/>
        public void Write(string? text)
        {
            Console.Write(text);
        }

        /// <inheritdoc/>
        public void WriteLine(object? value)
        {
            Console.WriteLine(value);
        }

        /// <inheritdoc/>
        public void WriteLine(string? text)
        {
            Console.WriteLine(text);
        }

        /// <inheritdoc/>
        public void WriteLine()
        {
            Console.WriteLine();
        }

        /// <inheritdoc/>
        public ConsoleKeyInfo ReadKey(bool displayKey = true)
        {
            return Console.ReadKey(!displayKey);
        }

        /// <inheritdoc/>
        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        /// <inheritdoc/>
        public (int column, int row) GetCursorPosition()
        {
            return Console.GetCursorPosition();
        }

        /// <inheritdoc/>
        public void SetCursorPosition(int column, int row)
        {
            Console.SetCursorPosition(column, row);
        }

        /// <inheritdoc/>
        public void PressKey(string text, bool displayKey = true)
        {
            Write(text);
            ReadKey(displayKey);
            WriteLine();
        }

        /// <inheritdoc/>
        public void MoveCursor(int columnOffset, int rowOffset)
        {
            SetCursorPosition(
                Math.Clamp(CursorColumn + columnOffset, 0, ConsoleWidth - 1),
                Math.Clamp(CursorRow - rowOffset, 0, ConsoleHeight - 1)
            );
        }

        /// <inheritdoc/>
        public void WriteAtPosition(string text, int column, int row, bool returnCursor = false)
        {
            var prewColumn = 0;
            var prewRow = 0;
            if (returnCursor)
            {
                (prewColumn, prewRow) = GetCursorPosition();
            }

            SetCursorPosition(column, row);
            Write(text);

            if (returnCursor)
            {
                SetCursorPosition(prewColumn, prewRow);
            }
        }
        #endregion
    }
}
