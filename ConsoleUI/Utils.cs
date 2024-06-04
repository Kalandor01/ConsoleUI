using ConsoleUI.Keybinds;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Returns the default keybinds.
        /// </summary>
        public static IEnumerable<KeyAction> GetDefaultKeybinds()
        {
            return new List<KeyAction>
            {
                new KeyAction(Key.ESCAPE, new ConsoleKeyInfo('\u001b', ConsoleKey.Escape, false, false, false), GetKeyMode.IGNORE_ESCAPE),
                new KeyAction(Key.UP, new ConsoleKeyInfo('\u0000', ConsoleKey.UpArrow, false, false, false), GetKeyMode.IGNORE_VERTICAL),
                new KeyAction(Key.DOWN, new ConsoleKeyInfo('\u0000', ConsoleKey.DownArrow, false, false, false), GetKeyMode.IGNORE_VERTICAL),
                new KeyAction(Key.LEFT, new ConsoleKeyInfo('\u0000', ConsoleKey.LeftArrow, false, false, false), GetKeyMode.IGNORE_HORIZONTAL),
                new KeyAction(Key.RIGHT, new ConsoleKeyInfo('\u0000', ConsoleKey.RightArrow, false, false, false), GetKeyMode.IGNORE_HORIZONTAL),
                new KeyAction(Key.ENTER, new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false), GetKeyMode.IGNORE_ENTER),
            };
        }

        /// <inheritdoc cref="GetKey(IEnumerable{GetKeyMode}, IEnumerable{KeyAction}?)"/>
        /// <param name="mode">The GetKeyMode to use.</param>
        public static KeyAction GetKey(GetKeyMode mode = GetKeyMode.NO_IGNORE, IEnumerable<KeyAction>? keybinds = null)
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
        public static KeyAction GetKey(IEnumerable<GetKeyMode> modeList, IEnumerable<KeyAction>? keybinds = null)
        {
            keybinds ??= GetDefaultKeybinds();

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
                        return action;
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
        /// Returns an ANSI string that saves the position of the cursor when displayed.
        /// </summary>
        public static string SaveCursorPosString()
        {
            return "\u001b[s";
        }

        /// <summary>
        /// Returns an ANSI string that sets the position of the cursor to the last saved position when displayed.
        /// </summary>
        public static string RestoreCursorPosString()
        {
            return "\u001b[u";
        }

        /// <summary>
        /// Returns an ANSI string that sets the position of the cursor in the console when displayed.
        /// </summary>
        /// <param name="x">The horizontal position of the cursor.</param>
        /// <param name="y">The vertical position of the cursor.</param>
        public static string SetCursorPosString(int x, int y)
        {
            return $"\u001b[{y};{x}H";
        }

        /// <summary>
        /// Returns an ANSI string that offsets the position of the cursor in the console when displayed.
        /// </summary>
        /// <param name="x">The horizontal offset of the cursor.</param>
        /// <param name="y">The vertical offset of the cursor.</param>
        public static string MoveCursorString(int x, int y)
        {
            return  (y == 0 ? "" : $"\u001b[{Math.Abs(y)}{(y >= 0 ? "A" : "B")}") +
                    (x == 0 ? "" : $"\u001b[{Math.Abs(x)}{(x >= 0 ? "C" : "D")}");
        }

        /// <summary>
        /// Returns an ANSI string that clears the current line, starting from the position of the cursor in the console when displayed.
        /// </summary>
        public static string ClearLineFromCursorPosString()
        {
            return "\u001b[0K";
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
            return AnsiEscapeCodeRegex().Replace(text, "");
        }

        /// <summary>
        /// Calculates the length of a character, as it will be displayed on the screen.
        /// </summary>
        /// <inheritdoc cref="GetCharDisplayLen(char, int, ref int, ref int, ref bool)"/>
        public static int GetCharDisplayLen(char character, int xPos)
        {
            var maxLen = 0;
            var isPreviousEscape = false;
            var length = 0;

            GetCharDisplayLen(character, xPos, ref length, ref maxLen, ref isPreviousEscape);
            return Math.Max(length, maxLen);
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
                GetCharDisplayLen(ch, startingXPos, ref len, ref maxLen, ref isPrevEsc);
            }
            return Math.Max(len, maxLen);
        }

        /// <summary>
        /// Calculates a substring from a string that has a specified length (maximum) when displayed on the terminal.
        /// </summary>
        /// <param name="text">The text to get the substring from.</param>
        /// <param name="length">The maximum lengh of text to return</param>
        /// <param name="startIndex">The index to start the search from in the text.</param>
        /// <param name="startingXPos">The starting X position, where the text will be displayed on the terminal. (Important for tabs.)</param>
        /// <param name="escapeCodesEnabled">Whether ANSI escape codes are enabled.</param>
        /// <returns>The index of the last character in the substring, or -1, if the text has no characters after ANSI escape codes are removed.</returns>
        public static int GetSpecificLengthString(string text, int length, int startIndex = 0, int startingXPos = 0, bool escapeCodesEnabled = true)
        {
            var maxLen = 0;
            var isPrevEsc = false;
            var displayLen = 0;

            text = text[startIndex..];
            var originalText = text;
            if (escapeCodesEnabled)
            {
                text = RemoveAnsiEscapeCodes(text);
            }

            var lastCharIndex = text.Length - 1;
            for (int x = startIndex; x < text.Length; x++)
            {
                GetCharDisplayLen(text[x], startingXPos, ref displayLen, ref maxLen, ref isPrevEsc);
                var currentLen = Math.Max(displayLen, maxLen);
                if (currentLen >= length)
                {
                    lastCharIndex = currentLen == length ? x : x - 1;
                    break;
                }
            }

            if (
                !escapeCodesEnabled ||
                lastCharIndex == -1 ||
                originalText.Length == text.Length
            )
            {
                return lastCharIndex;
            }

            var matchLengths = AnsiEscapeCodeRegex().Matches(originalText).Select(match => match.Length).ToArray();
            var splitLengths = AnsiEscapeCodeRegex().Split(originalText).Select(split => split.Length).ToArray();

            // get TRUE index
            var includeAnsiIndex = 0;
            var noAnsiLen = 0;
            for (int y = 0; y < matchLengths.Length + splitLengths.LongLength; y++)
            {
                var isEscapeChar = y % 2 == 1;
                var sectionLen = isEscapeChar ? matchLengths[y / 2] : splitLengths[y / 2];
                if (!isEscapeChar && sectionLen > 0)
                {
                    noAnsiLen += sectionLen;
                    if (noAnsiLen > lastCharIndex)
                    {
                        includeAnsiIndex += sectionLen - (noAnsiLen - lastCharIndex);
                        break;
                    }
                }
                includeAnsiIndex += sectionLen;
            }
            return includeAnsiIndex;
        }
        #endregion

        #region Private functions/regex
        /// <summary>
        /// Regex ued in <c>GetDisplayLen()</c>
        /// </summary>
        [GeneratedRegex("\\x1B\\[[^@-~]*[@-~]")]
        private static partial Regex AnsiEscapeCodeRegex();

        /// <summary>
        /// Calculates the length of a character in a text, as it will be displayed on the screen.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="startingXPos">The X position, where the text will be displayed on the terminal. (Important for tabs.)</param>
        /// <param name="length">The length of the text so far.</param>
        /// <param name="maxLen">The maximum length of the text so far. (Important for carriage return.)</param>
        /// <param name="isPreviousEscape">If the previous character was an escape character.</param>
        public static void GetCharDisplayLen(char character, int startingXPos, ref int length, ref int maxLen, ref bool isPreviousEscape)
        {
            if (character.Equals('\t'))
            {
                length += 8 - (startingXPos + length) % 8;
            }
            else if (character.Equals('\r'))
            {
                maxLen = Math.Max(length, maxLen);
                length = 0 - startingXPos;
            }
            else if (character.Equals('\b'))
            {
                length--;
            }
            else if (character.Equals('\u001b'))
            {
                isPreviousEscape = true;
            }
            else if (!character.Equals('\0'))
            {
                if (isPreviousEscape)
                {
                    isPreviousEscape = false;
                }
                else
                {
                    length++;
                }
            }
        }
        #endregion
    }
}
