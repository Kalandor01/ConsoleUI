using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace SaveFileManager
{
    public class Button : BaseUI
    {
        object? action;
        bool modify;

        /// <summary>
        /// Object for the options_ui method<br/>
        /// When used as input in the options_ui function, it text that is pressable with the enter key.<br/>
        /// If `action` is a function(or a list with a function as the 1. element, and arguments as the 2-n.element, including 1 or more dictionaries as **kwargs), it will run that function, if the button is clicked.<br/>
        /// - If the function returns False the screen will not rerender.<br/>
        /// - If it is a `UI_list` object, the object's `display` function will be automaticly called, allowing for nested menus.<br/>
        /// - If `modify` is `True`, the function (if it's not a `UI_list` object) will get a the `Button` object as it's first argument (and can modify it) when the function is called.<br/>
        /// Structure: [text]
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        /// <param name="modify"></param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
        public Button(string text = "", object? action = null, bool multiline = false, bool modify = false)
            : base(-1, text, "", false, "", multiline)
        {
            this.action = action;
            this.modify = modify;
        }


        // def _handle_action(self, key:Any, result_list:tuple[Any, Any, Any, Any, Any, Any], keybinds:Keybinds|None= None):
        //     if key == result_list[Keys.ENTER.value]:
        //         # list
        //         if type(self.action) is list and len(self.action) >= 2:
        //             lis = []
        //     di = dict()
        //             for elem in self.action:
        //                 if type(elem) is dict:
        //                     di.update(elem)
        //                 else:
        //                     lis.append(elem)
        //             if self.modify:
        //                 func_return = lis[0] (self, * lis[1:], **di)
        //             else:
        //                 func_return = lis[0] (* lis[1:], **di)
        //             if func_return is None:
        //                 return True
        //             else:
        //                 return bool (func_return)
        //     # normal function
        //         elif callable(self.action) :
        //             if self.modify:
        //                 func_return = self.action(self)
        //             else:
        //                 func_return = self.action()
        //             if func_return is None:
        //                     return True
        //             else:
        //                 return bool (func_return)
        //         # ui
        //         else:
        //             # display function or lazy back button
        //             if isinstance(self.action, UI_list) :
        //                 self.action.display(keybinds=keybinds, result_list=result_list)
        //             else:
        //                 # print("Option is not a UI_list object!")
        //                 pass
        //             return True
        //     else:
        //         return True
    }
}
