﻿using System.Numerics;
using System.Text;

namespace SaveFileManager
{
    public static class Utils
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

        /// <summary>
        /// Returns the <c>keyResults</c> for <c>UIList</c> or <c>OptionsUI</c> functions.
        /// </summary>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use.</param>
        /// <returns></returns>
        public static IEnumerable<object> GetResultList(IEnumerable<KeyAction> keybinds)
        {
            var keyResults = new List<object>();
            foreach (var action in keybinds)
            {
                keyResults.Add(action.response);
            }
            return keyResults;
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

        /// <summary>
        /// Prints the title and then a list of elements that the user can cycle between with the up and down arrows, and adjust with either the left and right arrow keys or the enter pressedKey depending on the input object type, and exit with the pressedKey assigned to escape.<br/>
        /// Accepts a list of objects(Slider, Choice, Toggle, Button).<br/>
        /// if an element in the list is not one of these objects, the value will be printed, (or if it's null, the line will be blank) and cannot be selected.<br/>
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
        /// <returns></returns>
        public static object? OptionsUI(IEnumerable<BaseUI?> elements, string? title = null, CursorIcon? cursorIcon = null, IEnumerable<KeyAction>? keybinds = null, IEnumerable<object>? keyResults = null)
        {
            if (keyResults is null)
            {
                if (keybinds is null)
                {
                    keyResults = new List<object> { Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER };
                }
                else
                {
                    keyResults = GetResultList(keybinds);
                }
            }
            if (cursorIcon is null)
            {
                cursorIcon = new CursorIcon();
            }

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
                !typeof(BaseUI).IsAssignableFrom(elements.ElementAt(selected).GetType())
            )
            {
                selected++;
                if (selected >= elements.Count())
                {
                    throw new UINoSelectablesExeption();
                }
            }
            // render/getkey loop
            object pressedKey;
            do
            {
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
                bool actualMove;
                do
                {
                    // to prevent useless screen re-render
                    actualMove = true;
                    // get pressedKey
                    pressedKey = keyResults.ElementAt((int)Key.ENTER);
                    var selectedElement = elements.ElementAt(selected);
                    if (
                        selectedElement is not null &&
                        (
                            typeof(Toggle).IsAssignableFrom(selectedElement.GetType()) ||
                            typeof(Button).IsAssignableFrom(selectedElement.GetType())
                        )
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
                        while (true)
                        {
                            selected += pressedKey.Equals(keyResults.ElementAt((int)Key.DOWN)) ? 1 : -1;
                            selected %= elements.Count();
                            if (selected < 0)
                            {
                                selected = elements.Count() - 1;
                            }
                            if (elements.ElementAt(selected) is not null &&
                                typeof(BaseUI).IsAssignableFrom(elements.ElementAt(selected).GetType()) &&
                                elements.ElementAt(selected).GetIsSelectable()
                            )
                            {
                                break;
                            }
                        }
                    }
                    // change value Base_UI
                    else if (
                        selectedElement is not null &&
                        typeof(BaseUI).IsAssignableFrom(selectedElement.GetType()) &&
                        selectedElement.GetIsSelectable() &&
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
                }
                while (!actualMove);
            }
            while (!pressedKey.Equals(keyResults.ElementAt((int)Key.ESCAPE)));
            return null;
        }
    }
}