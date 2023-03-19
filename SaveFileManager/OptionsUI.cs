using System.Diagnostics.Metrics;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace SaveFileManager
{
    public class OptionsUI
    {
        /// <summary>
        /// Exeption raised when there are no values in the selectables list in the <c>OptionsUI</c> function.
        /// </summary>
        public class UINoSelectablesExeption : Exception { }

        /// <summary>
        /// Prints the title and then a list of elements that the user can cycle between with the up and down arrows, and adjust with either the left and right arrow keys or the enter key depending on the input object type, and exit with Escape.<br/>
        /// Accepts mainly a list of objects(Slider, Choice, Toggle (and UI_list)).<br/>
        /// if an element in the list is not one of these objects, the value will be printed, (or if it's None, the line will be blank) and cannot be selected.<br/>
        /// If `allow_buffered_inputs` is `False`, if the user pressed some buttons before this function was called the function will not register those button presses.<br/>
        /// If `result_list` is not None, it will use the values in that list to match with the return value of the `get_key_with_obj()`.<br/>
        /// The order of the elements in the tuple should be:<br/>
        /// \t(escape, up, down, left, right, enter)<br/>
        /// If it is None, the default value is:<br/>
        /// \t`(Keys.ESCAPE, Keys.UP, Keys.DOWN, Keys.LEFT, Keys.RIGHT, Keys.ENTER)`
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="title"></param>
        /// <param name="cursorIcon"></param>
        /// <param name="keybinds"></param>
        /// <param name="keyResults"></param>
        /// <returns></returns>
        // public static object OptionsUI(IEnumerable<BaseUI/*, UIList*/> elements, string? title=null, CursorIcon? cursorIcon=null, IEnumerable<KeyAction>? keybinds=null, IEnumerable<object>? keyResults=null)
        // {
        //     if result_list is None:
        //         if keybinds is None:
        //             result_list = (Keys.ESCAPE, Keys.UP, Keys.DOWN, Keys.LEFT, Keys.RIGHT, Keys.ENTER)
        //         else:
        //             result_list = keybinds.get_result_list()
        //     if cursor_icon is None:
        //         cursor_icon = Cursor_icon()

        //     # is enter needed?
        //     no_enter = True
        //     for element in elements:
        //         if isinstance(element, (Toggle, UI_list)) :
        //             no_enter = False
        //             break
        //     # put selected on selectable
        //     selected = 0
        //     while not isinstance(elements[selected], (Base_UI, UI_list)):
        //         selected += 1
        //         if selected >= len(elements) :
        //             raise UINoSelectablesError("No selectable element in the elements list.")
        //     # render/getkey loop
        //     key = None
        //     while key != result_list[Keys.ESCAPE.value]:
        //         # render
        //         # clear screen
        //         txt = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"
        //         if title is not None:
        //             txt += title + "\n\n"
        //         for x, element in enumerate(elements) :
        //             # UI elements
        //             if isinstance(element, Base_UI) :
        //                 txt += element.make_text(
        //                     (cursor_icon.s_icon if selected == x else cursor_icon.icon),
        //                     (cursor_icon.s_icon_r if selected == x else cursor_icon.icon_r))
        //             # UI_list
        //             elif isinstance(element, UI_list):
        //                 # txt += element.__make_text(selected)
        //                 # render
        //                 if element.answer_list[0] is not None:
        //                     if selected == x:
        //                         curr_icon = element.cursor_icon.s_icon
        //                         curr_icon_r = element.cursor_icon.s_icon_r
        //                     else:
        //                         curr_icon = element.cursor_icon.icon
        //                         curr_icon_r = element.cursor_icon.icon_r
        //                     txt += curr_icon + (element.answer_list[0].replace("\n", f"{curr_icon_r}\n{curr_icon}") if element.multiline else element.answer_list[0]) +f"{curr_icon_r}\n"
        //                 else:
        //                     txt += "\n"
        //             elif element is None:
        //                 txt += "\n"
        //             else:
        //                 txt += str(element) + "\n"
        //         print(txt)
        //         # move selection/change value
        //         actual_move = False
        //         while not actual_move:
        //         # to prevent useless screen re-render at slider
        //         actual_move = True
        //             # get key
        //             key = result_list[Keys.ENTER.value]
        //             selected_e = elements[selected]
        //             if isinstance(selected_e, (Toggle, Button, UI_list)) :
        //                 key = get_key_with_obj(Get_key_modes.IGNORE_HORIZONTAL, keybinds, allow_buffered_inputs)
        //             else:
        //                 while key == result_list[Keys.ENTER.value]:
        //                     key = get_key_with_obj(Get_key_modes.NO_IGNORE, keybinds, allow_buffered_inputs)
        //                     if key == result_list[Keys.ENTER.value] and no_enter:
        //                         key = result_list[Keys.ESCAPE.value]
        //             # move selection
        //             if key == result_list[Keys.UP.value] or key == result_list[Keys.DOWN.value]:
        //                 while True:
        //                     if key == result_list[Keys.DOWN.value]:
        //                         selected += 1
        //                         if selected > len(elements) - 1:
        //                             selected = 0
        //                     else:
        //                         selected -= 1
        //                         if selected < 0:
        //                             selected = len(elements) - 1
        //                     if isinstance(elements[selected], (Base_UI, UI_list)) :
        //                         break
        //             # change value Base_UI
        //             elif isinstance(selected_e, Base_UI) and(key in [result_list[Keys.LEFT.value], result_list[Keys.RIGHT.value], result_list[Keys.ENTER.value]]) :
        //                 actual_move = bool(selected_e._handle_action(key, result_list, keybinds,))
        //             # change value UI_list
        //             elif isinstance(selected_e, UI_list) and key == result_list[Keys.ENTER.value]:
        //                 action = selected_e._handle_action(0, keybinds, allow_buffered_inputs, result_list)
        //                 if action is not None:
        //                     return action
        // }
    }
}
