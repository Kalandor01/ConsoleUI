﻿using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace SaveFileManager
{
    /// <summary>
    /// Contains miscalenious functions.
    /// </summary>
    public static class Utils
    {
        #region Constants
        internal static readonly string FILE_NAME_SEED_REPLACE_STRING = "*";
        /// <summary>
        /// Helper number for <c>Sqrt</c>.
        /// </summary>
        private static readonly BigInteger FastSqrtSmallNumber = 4503599761588223UL;
        #endregion

        #region Public functions
        /// <summary>
        /// ReadLine, but only accepts whole numbers.
        /// </summary>
        /// <param name="text">Text to write out when requesting the number.</param>
        /// <param name="errorText">Text to write out when the user inputs a wrong value.</param>
        /// <returns></returns>
        public static int ReadInt(string text, string errorText="Not a number!")
        {
            while (true)
            {
                Console.Write(text);
                if (int.TryParse(Console.ReadLine(), out int res))
                {
                    return res;
                }
                else
                {
                    Console.WriteLine(errorText);
                }
            }
        }

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
        /// Writes out text, and then returns what the user inputed.
        /// </summary>
        /// <param name="text">The text to write out.</param>
        public static string? Input(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }

        /// <param name="mode">The GetKeyMode to use.</param>
        /// <inheritdoc cref="GetKey(IEnumerable{GetKeyMode}, IEnumerable{KeyAction})"/>
        public static object GetKey(GetKeyMode mode=GetKeyMode.NO_IGNORE, IEnumerable<KeyAction>? keybinds=null)
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
        public static object GetKey(IEnumerable<GetKeyMode> modeList, IEnumerable<KeyAction>? keybinds=null)
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
        /// Square root calculator for <c>BigInteger</c>s.<br/>
        /// By MaxKlaxx <see href="https://stackoverflow.com/a/63909229">LINK</see>
        /// </summary>
        /// <param name="value">The <c>BigInteger</c>.</param>
        /// <returns>Square root of the value.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static BigInteger Sqrt(BigInteger value)
        {
            if (value <= FastSqrtSmallNumber) // small enough for Math.Sqrt() or negative?
            {
                if (value.Sign < 0) throw new ArgumentException("Negative argument.");
                return (ulong)Math.Sqrt((ulong)value);
            }

            BigInteger root; // now filled with an approximate value
            int byteLen = value.ToByteArray().Length;
            if (byteLen < 128) // small enough for direct double conversion?
            {
                root = (BigInteger)Math.Sqrt((double)value);
            }
            else // large: reduce with bitshifting, then convert to double (and back)
            {
                root = (BigInteger)Math.Sqrt((double)(value >> (byteLen - 127) * 8)) << (byteLen - 127) * 4;
            }

            for (; ; )
            {
                var root2 = value / root + root >> 1;
                if ((root2 == root || root2 == root + 1) && IsSqrt(value, root)) return root;
                root = value / root2 + root2 >> 1;
                if ((root == root2 || root == root2 + 1) && IsSqrt(value, root2)) return root2;
            }
        }

        /// <summary>
        /// Returns if the <c>BigInteger</c>'s square root is equal to the calculated value.<br/>
        /// By MaxKlaxx <see href="https://stackoverflow.com/a/63909229">LINK</see>
        /// </summary>
        /// <param name="root">The calculated square root.</param>
        /// <param name="value">The <c>BigInteger</c>.</param>
        public static bool IsSqrt(BigInteger value, BigInteger root)
        {
            var lowerBound = root * root;

            return value >= lowerBound && value <= lowerBound + (root << 1);
        }

        /// <summary>
        /// Function to sort a list of strings, with numbers correctly.<br/>
        /// by L.B <see href="https://stackoverflow.com/a/10000192">SOURCE</see>
        /// </summary>
        /// <param name="list">The list to sort</param>
        /// <returns></returns>
        public static IEnumerable<string> NaturalSort(IEnumerable<string> list)
        {
            int maxLen = list.Select(s => s.Length).Max();

            static char PaddingChar(string s) => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

            return list
                .Select(s =>
                    new
                    {
                        OrgStr = s,
                        SortStr = Regex.Replace(s, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                    })
                .OrderBy(x => x.SortStr)
                .Select(x => x.OrgStr);
        }
        #endregion
    }
}
