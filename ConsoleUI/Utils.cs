using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleUI
{
    /// <summary>
    /// Contains miscellaneous functions.
    /// </summary>
    public static partial class Utils
    {
        #region Public functions
        /// <summary>
        /// Writes out text, and then waits for a key press.
        /// </summary>
        /// <param name="text">The text to write out.</param>
        public static void PressKey(string text)
        {
            Console.Write(text);
            Console.ReadKey(true);
            Console.WriteLine();
        }

        /// <inheritdoc cref="GetKey(IEnumerable{GetKeyMode}, IEnumerable{KeyAction}?)"/>
        /// <param name="mode">The GetKeyMode to use.</param>
        public static object GetKey(GetKeyMode mode = GetKeyMode.NO_IGNORE, IEnumerable<KeyAction>? keybinds = null)
        {
            return GetKey(new List<GetKeyMode> { mode }, keybinds);
        }

        /// <summary>
        /// Function for detecting one key from a list of keypresses.<br/>
        /// Depending on the mode, it ignores some keys.<br/>
        /// </summary>
        /// <param name="modeList">The list of GetKeyMode to use.</param>
        /// <param name="keybinds">The list of KeyActions.</param>
        /// <returns>The <c>response</c> of the <c>KeyAction</c> object that maches the key the user pressed.</returns>
        public static object GetKey(IEnumerable<GetKeyMode> modeList, IEnumerable<KeyAction>? keybinds = null)
        {
            keybinds ??= new List<KeyAction> {
                    new KeyAction(Key.ESCAPE, new ConsoleKeyInfo('\u001b', ConsoleKey.Escape, false, false, false), GetKeyMode.IGNORE_ESCAPE),
                    new KeyAction(Key.UP, new ConsoleKeyInfo('\u0000', ConsoleKey.UpArrow, false, false, false), GetKeyMode.IGNORE_VERTICAL),
                    new KeyAction(Key.DOWN, new ConsoleKeyInfo('\u0000', ConsoleKey.DownArrow, false, false, false), GetKeyMode.IGNORE_VERTICAL),
                    new KeyAction(Key.LEFT, new ConsoleKeyInfo('\u0000', ConsoleKey.LeftArrow, false, false, false), GetKeyMode.IGNORE_HORIZONTAL),
                    new KeyAction(Key.RIGHT, new ConsoleKeyInfo('\u0000', ConsoleKey.RightArrow, false, false, false), GetKeyMode.IGNORE_HORIZONTAL),
                    new KeyAction(Key.ENTER, new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false), GetKeyMode.IGNORE_ENTER),
            };

            while (true)
            {
                var key = Console.ReadKey(true);
                foreach (var action in keybinds)
                {
                    var ignore = false;
                    foreach (var mode in modeList)
                    {
                        if (action.ignoreModes.Contains(mode))
                        {
                            ignore = true;
                            break;
                        }
                    }
                    if (!ignore && action.Keys.Contains(key))
                    {
                        return action.response;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the <c>keyResults</c> for <c>UIList</c> or <c>OptionsUI</c> functions.
        /// </summary>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use.</param>
        public static IEnumerable<object> GetResultsList(IEnumerable<KeyAction> keybinds)
        {
            var keyResults = new List<object>();
            foreach (var action in keybinds)
            {
                keyResults.Add(action.response);
            }
            return keyResults;
        }

        /// <summary>
        /// Offsets the coordinates of the cursor.
        /// </summary>
        /// <param name="offset">The offset coordinates.</param>
        public static void MoveCursor((int x, int y) offset)
        {
            (int x, int y) = (Math.Clamp(Console.CursorLeft + offset.x, 0, Console.BufferWidth - 1), Math.Clamp(Console.CursorTop - offset.y, 0, Console.BufferHeight - 1));
            Console.SetCursorPosition(x, y);
        }

        /// <summary>
        /// Removes all ANSI escape codes from the string.
        /// </summary>
        /// <param name="text">The string to clean.</param>
        public static string RemoveAnsiEscapeCodes(string text)
        {
            return EscapeCodeCleanupRegex().Replace(text, "");
        }

        /// <summary>
        /// Calculates the length of the string, as it will be displayed on the screen.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="startingXPos">The starting X position, where the text will be displayed on the terminal. (Important for tabs.)</param>
        /// <param name="escapeCodesEnabled">Whether ANSI escape codes are enabled.</param>
        public static int GetDisplayLen(string text, int startingXPos = 0, bool escapeCodesEnabled = true)
        {
            var maxLen = 0;
            var isPrevEsc = false;
            var len = 0;

            if (escapeCodesEnabled)
            {
                text = RemoveAnsiEscapeCodes(text);
            }

            foreach (var ch in text)
            {
                if (ch.Equals('\t'))
                {
                    len += 8 - (startingXPos + len) % 8;
                }
                else if (ch.Equals('\r'))
                {
                    maxLen = len > maxLen ? len : maxLen;
                    len = 0 - startingXPos;
                }
                else if (ch.Equals('\u001b'))
                {
                    isPrevEsc = true;
                }
                else if (!ch.Equals('\0'))
                {
                    if (isPrevEsc)
                    {
                        isPrevEsc = false;
                    }
                    else
                    {
                        len++;
                    }
                }
            }
            return len > maxLen ? len : maxLen;
        }

        /// <summary>
        /// Regex ued in <c>GetDisplayLen()</c>
        /// </summary>
        [GeneratedRegex("\\x1B\\[[^@-~]*[@-~]")]
        private static partial Regex EscapeCodeCleanupRegex();
        #endregion
    }
}
