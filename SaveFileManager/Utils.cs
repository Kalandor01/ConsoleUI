using System.Numerics;
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
        /// <returns></returns>
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
        /// Prints the title and then a list of elements that the user can cycle between with the up and down arrows, and adjust with either the left and right arrow keys or the enter pressedKey depending on the input object type, and exit with the pressedKey assigned to escape.<br/>
        /// Accepts a list of objects(Slider, Choice, Toggle, Button).<br/>
        /// if an element in the list is not one of these objects, the value will be printed, (or if its null, the line will be blank) and cannot be selected.<br/>
        /// The order of the elements in the tuple should be:<br/>
        /// - (escape, up, down, left, right, enter)<br/>
        /// If it is None, the default value is:<br/>
        /// - (Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER)
        /// </summary>
        /// <param name="elements">The list of <c>BaseUI</c> objects to use.</param>
        /// <param name="title">The string to print before the <c>elements</c>.</param>
        /// <param name="cursorIcon">The cursor icon style to use.</param>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use, if the selected action is a <c>UIList</c>.</param>
        /// <param name="keyResults">The list of posible results returned by pressing a pressedKey.</param>
        /// <param name="canEscape">Allows the user to press the key associated with escape, to exit the menu.</param>
        /// <exception cref="UINoSelectablesExeption"></exception>
        /// <returns></returns>
        public static object? OptionsUI(IEnumerable<BaseUI?> elements, string? title = null, CursorIcon? cursorIcon = null, bool canEscape = true, IEnumerable<KeyAction>? keybinds = null, IEnumerable<object>? keyResults = null)
        {
            if (elements.All(answer => answer is null || !answer.IsSelectable()))
            {
                throw new UINoSelectablesExeption();
            }

            if (keyResults is null || keyResults.Count() < 6)
            {
                if (keybinds is null)
                {
                    keyResults = new List<object> { Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER };
                }
                else
                {
                    keyResults = GetResultsList(keybinds);
                }
            }
            cursorIcon ??= new CursorIcon();

            // is enter needed?
            var noEnter = true;
            foreach (var element in elements)
            {
                if (
                    element is not null &&
                    (
                        typeof(Toggle).IsAssignableFrom(element.GetType()) ||
                        typeof(Button).IsAssignableFrom(element.GetType())
                    )
                )
                {
                    noEnter = false;
                    break;
                }
            }
            // put selected on selectable
            var selected = 0;
            while (
                elements.ElementAt(selected) is null ||
                !elements.ElementAt(selected).IsSelectable()
            )
            {
                selected++;
            }
            // render/getkey loop
            object pressedKey;
            do
            {
                // prevent infinite loop
                if (elements.All(answer => answer is null || !answer.IsSelectable()))
                {
                    throw new UINoSelectablesExeption();
                }
                // render
                // clear screen
                var txt = new StringBuilder("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                if (title is not null)
                {
                    txt.Append(title + "\n\n");
                }
                for (var x = 0; x < elements.Count(); x++)
                {
                    var element = elements.ElementAt(x);
                    if (element is not null && typeof(BaseUI).IsAssignableFrom(element.GetType()))
                    {
                        txt.Append(element.MakeText(
                            selected == x ? cursorIcon.sIcon : cursorIcon.icon,
                            selected == x ? cursorIcon.sIconR : cursorIcon.iconR
                        ));
                    }
                    else if (element is null)
                    {
                        txt.Append('\n');
                    }
                    else
                    {
                        txt.Append(element.ToString() + "\n");
                    }
                }
                Console.WriteLine(txt.ToString());
                // move selection/change value
                var actualMove = false;
                do
                {
                    // get pressedKey
                    pressedKey = keyResults.ElementAt((int)Key.ENTER);
                    var selectedElement = elements.ElementAt(selected);
                    if (
                        selectedElement is not null &&
                        selectedElement.IsOnlyClickable()
                    )
                    {
                        pressedKey = GetKey(GetKeyMode.IGNORE_HORIZONTAL, keybinds);
                    }
                    else
                    {
                        while (pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)))
                        {
                            pressedKey = GetKey(GetKeyMode.NO_IGNORE, keybinds);
                            if (pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)) && noEnter)
                            {
                                pressedKey = keyResults.ElementAt((int)Key.ESCAPE);
                            }
                        }
                    }
                    // move selection
                    if (
                        pressedKey.Equals(keyResults.ElementAt((int)Key.UP)) ||
                        pressedKey.Equals(keyResults.ElementAt((int)Key.DOWN))
                    )
                    {
                        var prevSelected = selected;
                        while (true)
                        {
                            selected += pressedKey.Equals(keyResults.ElementAt((int)Key.DOWN)) ? 1 : -1;
                            selected %= elements.Count();
                            if (selected < 0)
                            {
                                selected = elements.Count() - 1;
                            }
                            if (
                                elements.ElementAt(selected) is not null &&
                                elements.ElementAt(selected).IsSelectable()
                            )
                            {
                                break;
                            }
                        }
                        if (prevSelected != selected)
                        {
                            actualMove = true;
                        }
                    }
                    // change value
                    else if (
                        selectedElement is not null &&
                        selectedElement.IsSelectable() &&
                        (
                            pressedKey.Equals(keyResults.ElementAt((int)Key.LEFT)) ||
                            pressedKey.Equals(keyResults.ElementAt((int)Key.RIGHT)) ||
                            pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)))
                        )
                    {
                        var returned = selectedElement.HandleAction(pressedKey, keyResults, keybinds);
                        if (returned is not null)
                        {
                            if (returned.GetType() == typeof(bool))
                            {
                                actualMove = (bool)returned;
                            }
                            else
                            {
                                return returned;
                            }
                        }
                    }
                    else if (canEscape && pressedKey.Equals(keyResults.ElementAt((int)Key.ESCAPE)))
                    {
                        actualMove = true;
                    }
                }
                while (!actualMove);
            }
            while (!canEscape || !pressedKey.Equals(keyResults.ElementAt((int)Key.ESCAPE)));
            return null;
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
