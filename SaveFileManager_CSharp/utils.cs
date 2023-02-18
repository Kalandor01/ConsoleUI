using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SaveFileManager
{
    public static class utils
    {
        /// <summary>
        /// ReadKey but only accepts whole numbers.
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

        /// <param name="mode">The GetKeyMode to use.</param>
        /// <inheritdoc cref="GetKey(IEnumerable{GetKeyMode}, IEnumerable{KeyAction})"/>
        public static object GetKey(GetKeyMode mode=GetKeyMode.NO_IGNORE, IEnumerable<KeyAction> keybinds=null)
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
        public static object GetKey(IEnumerable<GetKeyMode> modeList, IEnumerable<KeyAction> keybinds=null)
        {
            if (keybinds is null)
            {
                keybinds = new List<KeyAction> {
                    new KeyAction(Key.ESCAPE, new ConsoleKeyInfo('\u001b', ConsoleKey.Escape, false, false, false), GetKeyMode.IGNORE_ESCAPE),
                    new KeyAction(Key.UP, new ConsoleKeyInfo('\u0000', ConsoleKey.UpArrow, false, false, false), GetKeyMode.IGNORE_VERTICAL),
                    new KeyAction(Key.DOWN, new ConsoleKeyInfo('\u0000', ConsoleKey.DownArrow, false, false, false), GetKeyMode.IGNORE_VERTICAL),
                    new KeyAction(Key.LEFT, new ConsoleKeyInfo('\u0000', ConsoleKey.LeftArrow, false, false, false), GetKeyMode.IGNORE_HORIZONTAL),
                    new KeyAction(Key.RIGHT, new ConsoleKeyInfo('\u0000', ConsoleKey.RightArrow, false, false, false), GetKeyMode.IGNORE_HORIZONTAL),
                    new KeyAction(Key.ENTER, new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false), GetKeyMode.IGNORE_ENTER),
                };
            }

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
                    if (!ignore && action.keys.Contains(key))
                    {
                        return action.response;
                    }
                }
            }
        }

        private static readonly BigInteger FastSqrtSmallNumber = 4503599761588223UL; // as static readonly = reduce compare overhead

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
    }
}
