using ConsoleUI.Keybinds;

namespace ConsoleUI
{
    /// <summary>
    /// Stores an action, used in <c>Button</c> and <c>UIList</c>.
    /// </summary>
    public class UIAction
    {
        #region Private fields
        /// <summary>
        /// The UIList object to display when the button is pressed.
        /// </summary>
        readonly UIList? actionUIList;

        /// <summary>
        /// The function to run when the button is pressed.<br/>
        /// - If the function returns false the UI will not update.<br/>
        /// - If it returns anything other than a bool, the <c>OptionsUI</c> will instantly return that value.
        /// </summary>
        readonly Delegate? actionFunction;

        /// <summary>
        /// The list of arguments to run the function with.
        /// </summary>
        readonly object?[]? actionParameters;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="uiList"><inheritdoc cref="actionUIList" path="//summary"/></param>
        public UIAction(UIList uiList)
            : this(uiList, null, null) { }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="function"><inheritdoc cref="actionFunction" path="//summary"/></param>
        /// <param name="args"><inheritdoc cref="actionParameters" path="//summary"/></param>
        public UIAction(Delegate function, params object?[]? args)
            : this(null, function, args) { }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="uiList"><inheritdoc cref="actionUIList" path="//summary"/></param>
        /// <param name="function"><inheritdoc cref="actionFunction" path="//summary"/></param>
        /// <param name="args"><inheritdoc cref="actionParameters" path="//summary"/></param>
        private UIAction(UIList? uiList, Delegate? function, params object?[]? args)
        {
            actionUIList = uiList;
            actionFunction = function;
            actionParameters = args;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Invokes the action and returns the value that it returned.<br/>
        /// - if the action invokes a function, it gets called, with arguments, and an optional extra parameter as the first argument.<br/>
        /// - if the action invokes a <c>UIList</c>, the object's <c>Display</c> function gets called.
        /// </summary>
        /// <param name="extraParameter">Extra parameter to put at the beginning of the arguments list.</param>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use, if the action is calling a <c>UIList</c>.</param>
        public (UIActionType actionType, object? returned) InvokeAction(object? extraParameter = null, IEnumerable<KeyAction>? keybinds = null)
        {
            // function
            if (actionFunction is not null)
            {
                object?[]? argsArray = null;
                if (extraParameter is not null || (actionParameters is not null && actionParameters.Any()))
                {
                    var args = new List<object?>();
                    if (extraParameter is not null)
                    {
                        args.Add(extraParameter);
                    }
                    if (actionParameters is not null && actionParameters.Any())
                    {
                        args.AddRange(actionParameters);
                    }
                    argsArray = args.ToArray();
                }
                return (UIActionType.FUNCTION, actionFunction.DynamicInvoke(argsArray));
            }
            // UIList
            else
            {
                return (UIActionType.UILIST, actionUIList?.Display(keybinds));
            }
        }
        #endregion
    }
}
