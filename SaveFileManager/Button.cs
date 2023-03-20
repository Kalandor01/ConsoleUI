using System.Collections;

namespace SaveFileManager
{
    public class Button : BaseUI
    {
        bool modifyList;
        UIList? actionUIList;
        Delegate? actionFunction;
        object[]? actionParameters;

        /// <summary>
        /// Object for the options_ui method<br/>
        /// When used as input in the options_ui function, it text that is pressable with the enter key.<br/>
        /// If `action` is a function(or a list with a function as the 1. element, and arguments as the 2-n.element, including 1 or more dictionaries as **kwargs), it will run that function, if the button is clicked.<br/>
        /// - If the function returns False the screen will not rerender.<br/>
        /// - If it is a `UI_list` object, the object's `display` function will be automaticly called, allowing for nested menus.<br/>
        /// - If `modifyList` is `True`, the function (if it's not a `UI_list` object) will get a the `Button` object as it's first argument (and can modifyList it) when the function is called.<br/>
        /// Structure: [text]
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        /// <param name="modifyList"></param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
        public Button(string text = "", object? action = null, bool multiline = false, bool modifyList = false)
            : base(-1, text, "", false, "", multiline)
        {
            this.modifyList = modifyList;
            SetAction(action);
        }

        /// <inheritdoc cref="HandleAction(object, IEnumerable{object}, IEnumerable{KeyAction}?)"/>
        public bool HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null)
        {
            if (key.Equals(keyResults.ElementAt((int)Key.ENTER)))
            {
                // function (list)
                if (actionFunction is not null)
                {
                    object? funcReturn;
                    if (actionParameters is not null)
                    {
                        funcReturn = actionFunction.DynamicInvoke(actionParameters);
                    }
                    else
                    {
                        funcReturn = actionFunction.DynamicInvoke();
                    }
                    if (funcReturn is null)
                    {
                        return true;
                    }
                    else
                    {
                        return funcReturn.GetType() == typeof(bool) && (bool)funcReturn;
                    }
                }
                // ui / else
                else
                {
                    // display function
                    if (actionUIList is not null)
                    {
                        actionUIList.Display(keybinds, keyResults);
                    }
                    //else
                    //{
                    //    Console.WriteLine("Option is not a UI_list object!");
                    //}
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void SetAction(object? action)
        {
            actionUIList = null;
            actionFunction = null;
            actionParameters = null;
            // list
            if (action is not null &&
                typeof(IEnumerable).IsAssignableFrom(action.GetType()) &&
                ((IEnumerable<object>)action).Count() >= 2 &&
                ((IEnumerable<object>)action).ElementAt(0) is Delegate)
            {
                var actionList = (IEnumerable<object>)action;
                actionFunction = (Delegate)actionList.ElementAt(0);
                var paramNum = actionList.Count() + (modifyList ? 0 : -1);
                actionParameters = new object[paramNum];
                var index = 0;
                if (modifyList)
                {
                    actionParameters[index] = this;
                    index++;
                }
                for (var x = 1; x < actionList.Count(); x++)
                {
                    actionParameters[index] = actionList.ElementAt(x);
                    index++;
                }
            }
            // normal function
            else if (action is Delegate)
            {
                actionFunction = (Delegate)action;
                if (modifyList)
                {
                    actionParameters = new object[] { this };
                }
            }
            // ui
            else if (action is not null && typeof(UIList).IsAssignableFrom(action.GetType()))
            {
                actionUIList = (UIList)action;
            }
        }
    }
}
