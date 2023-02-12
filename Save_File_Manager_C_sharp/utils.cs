using System;
using System.Collections.Generic;
using System.Linq;

namespace Save_File_Manager
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

        /// <summary>
        /// Function for detecting one key from a list of keypresses.<br/>
        /// Depending on the mode, it ignores some keys.<br/>
        /// Returns the <c>response</c> of the <c>KeyAction</c> object that maches the key the user pressed.
        /// </summary>
        /// <param name="mode">The GetKeyMode to use.</param>
        /// <param name="keybinds">The list of KeyActions.</param>
        /// <returns></returns>
        public static object GetKey(GetKeyMode mode=GetKeyMode.NO_IGNORE, IEnumerable<KeyAction> keybinds=null)
        {
            return GetKey(new List<GetKeyMode> { mode }, keybinds);
        }

        /// <summary>
        /// Function for detecting one key from a list of keypresses.<br/>
        /// Depending on the mode, it ignores some keys.<br/>
        /// Returns the <c>response</c> of the <c>KeyAction</c> object that maches the key the user pressed.
        /// </summary>
        /// <param name="modeList">The list of GetKeyMode to use.</param>
        /// <param name="keybinds">The list of KeyActions.</param>
        /// <returns></returns>
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
    }
}
