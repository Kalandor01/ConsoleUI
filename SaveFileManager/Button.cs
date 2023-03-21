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
        /// Object for the <c>OptionsUI</c> method<br/>
        /// When used as input in the <c>OptionsUI</c> function, it text that is pressable with the enter key.<br/>
        /// Structure: [text]
        /// </summary>
        /// <param name="text">The text to write out.</param>
        /// <param name="action">A function (or a list with a function as the 1. element, and arguments as the 2-n.element), that will run if the button is clicked.<br/>
        /// - If the function returns false the UI will not update.<br/>
        /// - If it is a <c>UIList</c> object, the object's <c>Display</c> function will be automaticly called, allowing for nested menus.</param>
        /// <param name="modifyList">If it's true, the function will get a the <c>Button</c> object as it's first argument (and can modify it) when the function is called.</param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
        public Button(string text = "", object? action = null, bool multiline = false, bool modifyList = false)
            : base(-1, text, "", false, "", multiline)
        {
            this.modifyList = modifyList;
            SetAction(action);
        }

        /// <inheritdoc cref="HandleAction(object, IEnumerable{object}, IEnumerable{KeyAction}?)"/>
        public override object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null)
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
                        return funcReturn;
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
                action.GetType() != typeof(string) &&
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
