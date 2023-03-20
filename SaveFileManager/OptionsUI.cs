using System.Text;
using System.Xml.Linq;

namespace SaveFileManager
{
    public class OptionsUI
    {
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
        public static object OptionsUI(IEnumerable<object> elements, string? title = null, CursorIcon? cursorIcon = null, IEnumerable<KeyAction>? keybinds = null, IEnumerable<object>? keyResults = null)
        {
            if (keyResults is null)
            {
                if (keybinds is null)
                {
                    keyResults = new List<object> { Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER };
                }
                else
                {
                    keyResults = Utils.GetResultList(keybinds);
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
                    typeof(Toggle).IsAssignableFrom(element.GetType()) ||
                    typeof(Button).IsAssignableFrom(element.GetType())
                )
                {
                    noEnter = false;
                    break;
                }
            }
            // put selected on selectable
            var selected = 0;
            while (typeof(BaseUI).IsAssignableFrom(elements.ElementAt(selected).GetType()))
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
                    if (typeof(BaseUI).IsAssignableFrom(element.GetType()))
                    {
                        txt.Append(((BaseUI)element).MakeText(
                            selected == x ? cursorIcon.sIcon : cursorIcon.icon,
                            selected == x ? cursorIcon.sIconR : cursorIcon.iconR
                        ));
                    }
                    else if (element is null)
                    {
                        txt.Append("\n");
                    }
                    else
                    {
                        txt.Append(element.ToString() + "\n");
                    }
                }
                Console.WriteLine(txt.ToString());
                // move selection/change value
                bool actual_move;
                do
                {
                    // to prevent useless screen re-render
                    actual_move = true;
                    // get pressedKey
                    pressedKey = keyResults.ElementAt((int)Key.ENTER);
                    var selectedElement = elements.ElementAt(selected);
                    if (
                        typeof(Toggle).IsAssignableFrom(selectedElement.GetType()) ||
                        typeof(Button).IsAssignableFrom(selectedElement.GetType())
                    )
                    {
                        pressedKey = Utils.GetKey(GetKeyMode.IGNORE_HORIZONTAL, keybinds);
                    }
                    else
                    {
                        while (pressedKey.Equals(keyResults.ElementAt((int)Key.ENTER)))
                        {
                            pressedKey = Utils.GetKey(GetKeyMode.NO_IGNORE, keybinds);
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
                            selected += pressedKey.Equals(keyResults.ElementAt((int)Key.DOWN)) ? 1 : -1
                            selected %= elements.Count();
                            if (selected < 0)
                            {
                                selected = elements.Count() - 1;
                            }
                            if (typeof(BaseUI).IsAssignableFrom(elements.ElementAt(selected).GetType()))
                            {
                                break;
                            }
                        }
                    }
                    // change value Base_UI
                    else if (isinstance(selected_e, Base_UI) and(pressedKey in [result_list[Keys.LEFT.value], result_list[Keys.RIGHT.value], result_list[Keys.ENTER.value]]))
                    {
                        actual_move = bool(selected_e._handle_action(pressedKey, result_list, keybinds,))
                    }
                    // change value UI_list
                    else if (isinstance(selected_e, UI_list) and pressedKey == result_list[Keys.ENTER.value])
                    {
                        action = selected_e._handle_action(0, keybinds, allow_buffered_inputs, result_list)
                        if (action is not null)
                        {
                            return action;
                        }
                    }
                }
                while (!actual_move);
            }
            while (!pressedKey.Equals(keyResults.ElementAt((int)Key.ESCAPE)));
         }
    }
}
