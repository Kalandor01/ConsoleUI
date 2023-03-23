namespace SaveFileManager
{
    public class Button : BaseUI
    {
        bool modifyList;
        UIList? actionUIList;
        Delegate? actionFunction;
        object?[]? actionParameters;

        /// <inheritdoc cref="Button(UIList, Delegate, IEnumerable{object?}, bool, string, bool)"/>
        public Button(UIList uiList, string text = "",  bool multiline = false)
            : this(uiList, null, null, text:text, multiline:multiline) { }

        /// <inheritdoc cref="Button(UIList, Delegate, IEnumerable{object?}, bool, string, bool)"/>
        public Button(Delegate function, bool modifyList = false, string text = "", bool multiline = false)
            : this(null, function, null, modifyList, text, multiline) { }

        /// <inheritdoc cref="Button(UIList, Delegate, IEnumerable{object?}, bool, string, bool)"/>
        public Button(Delegate function, IEnumerable<object?> args, bool modifyList = false, string text = "", bool multiline = false)
            : this(null, function, args, modifyList, text, multiline) { }

        /// <summary>
        /// Object for the <c>OptionsUI</c> method<br/>
        /// When used as input in the <c>OptionsUI</c> function, it text that is pressable with the enter key.<br/>
        /// Structure: [text]
        /// </summary>
        /// <param name="text">The text to write out.</param>
        /// <param name="uiList">The UIList object to display when the button is pressed.</param>
        /// <param name="function">The function to run when the button is pressed.<br/>
        /// - If the function returns false the UI will not update.<br/>
        /// - If it return anything other than a bool, the <c>OptionsUI</c> will instantly return that value.</param>
        /// <param name="args">The list of arguments to run the function with.</param>
        /// <param name="modifyList">If it's true, the function will get a the <c>Button</c> object as it's first argument (and can modify it) when the function is called.</param>
        /// <inheritdoc cref="BaseUI(int, string, string, bool, string, bool)"/>
        private Button(UIList? uiList, Delegate? function, IEnumerable<object?>? args, bool modifyList = false, string text = "", bool multiline = false)
            : base(-1, text, "", false, "", multiline)
        {
            this.modifyList = modifyList;
            SetAction(uiList, function, args);
        }

        /// <inheritdoc cref="HandleAction(object, IEnumerable{object}, IEnumerable{KeyAction}?)"/>
        public override object HandleAction(object key, IEnumerable<object> keyResults, IEnumerable<KeyAction>? keybinds = null)
        {
            if (key.Equals(keyResults.ElementAt((int)Key.ENTER)))
            {
                // function
                if (actionFunction is not null)
                {
                    object? funcReturn;
                    // args
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

        private void SetAction(UIList? uiList, Delegate? function, IEnumerable<object?>? args)
        {
            actionUIList = null;
            actionFunction = null;
            actionParameters = null;
            // list
            if (function is not null)
            {
                actionFunction = function;
                //function with args
                if (args is not null)
                {
                    var paramNum = args.Count() + (modifyList ? 1 : 0);
                    actionParameters = new object[paramNum];
                    var index = 0;
                    if (modifyList)
                    {
                        actionParameters[index] = this;
                        index++;
                    }
                    for (var x = 1; x < args.Count(); x++)
                    {
                        actionParameters[index] = args.ElementAt(x);
                        index++;
                    }
                }
                //function
                else
                {
                    if (modifyList)
                    {
                        actionParameters = new object[] { this };
                    }
                }
            }
            // ui
            else if (uiList is not null)
            {
                actionUIList = uiList;
            }
        }
    }
}
