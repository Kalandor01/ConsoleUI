using ConsoleUI.Keybinds;
using static ConsoleUI.Utils;

namespace ConsoleUI
{
    /// <summary>
    /// Stores an action, used in <see cref="UIElements.Button"/> and <see cref="UIList"/>.
    /// </summary>
    public class UIAction
    {
        #region Private fields
        /// <summary>
        /// The type of the UIAction.
        /// </summary>
        private readonly UIActionType actionType;

        /// <summary>
        /// The UIList object to display when the action is triggered.
        /// </summary>
        private readonly UIList? actionUIList;

        /// <summary>
        /// The OptionsUI object to display when the action is triggered.
        /// </summary>
        private readonly OptionsUI? actionOptionsUI;

        /// <summary>
        /// The function to run when the action is triggered.<br/>
        /// - If the function returns false the UI will not update.<br/>
        /// - If it returns anything other than a bool, the <c>OptionsUI</c> will instantly return that value.
        /// </summary>
        private readonly Delegate? actionDelegate;

        /// <summary>
        /// The list of arguments to run the function with.
        /// </summary>
        private readonly object?[]? actionParameters;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="actionType"><inheritdoc cref="actionType" path="//summary"/></param>
        /// <param name="uiList"><inheritdoc cref="actionUIList" path="//summary"/></param>
        /// <param name="optionsUI"><inheritdoc cref="actionOptionsUI" path="//summary"/></param>
        /// <param name="function"><inheritdoc cref="actionDelegate" path="//summary"/></param>
        /// <param name="args"><inheritdoc cref="actionParameters" path="//summary"/></param>
        private UIAction(
            UIActionType actionType,
            UIList? uiList,
            OptionsUI? optionsUI,
            Delegate? function,
            params object?[]? args
        )
        {
            this.actionType = actionType;
            actionUIList = uiList;
            actionOptionsUI = optionsUI;
            actionDelegate = function;
            actionParameters = args;
        }
        #endregion

        #region Create methods
        #region Custom
        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="uiList"><inheritdoc cref="actionUIList" path="//summary"/></param>
        public static UIAction CreateUIListAction(UIList uiList)
        {
            return new UIAction(UIActionType.UILIST, uiList, null, null, null);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="optionsUI"><inheritdoc cref="actionOptionsUI" path="//summary"/></param>
        public static UIAction CreateOptionsUIAction(OptionsUI optionsUI)
        {
            return new UIAction(UIActionType.OPTIONSUI, null, optionsUI, null, null);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="actionDelegate"><inheritdoc cref="actionDelegate" path="//summary"/></param>
        /// <param name="args"><inheritdoc cref="actionParameters" path="//summary"/></param>
        public static UIAction CreateDelegateAction(Delegate actionDelegate, params object?[]? args)
        {
            return new UIAction(UIActionType.FUNCTION, null, null, actionDelegate, args);
        }
        #endregion

        #region Action with extra
        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        public static UIAction CreateWithExtraArg<TExt>(Action<TExt> action)
        {
            return CreateDelegateAction(action);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg">The 1. argument.</param>
        public static UIAction CreateWithExtraArg<TExt, TArg>(Action<TExt, TArg> action, TArg arg)
        {
            return CreateDelegateAction(action, arg);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        public static UIAction CreateWithExtraArg<TExt, TArg1, TArg2>(Action<TExt, TArg1, TArg2> action, TArg1 arg1, TArg2 arg2)
        {
            return CreateDelegateAction(action, arg1, arg2);
        }
        #endregion

        #region Action
        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action"><inheritdoc cref="actionDelegate" path="//summary"/></param>
        public static UIAction Create(Action action)
        {
            return CreateDelegateAction(action);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg">The 1. argument.</param>
        public static UIAction Create<TArg>(Action<TArg> action, TArg arg)
        {
            return CreateDelegateAction(action, arg);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        public static UIAction Create<TArg1, TArg2>(Action<TArg1, TArg2> action, TArg1 arg1, TArg2 arg2)
        {
            return CreateDelegateAction(action, arg1, arg2);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        /// <param name="arg3">The 3. argument.</param>
        public static UIAction Create<TArg1, TArg2, TArg3>(Action<TArg1, TArg2, TArg3> action, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return CreateDelegateAction(action, arg1, arg2, arg3);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        /// <param name="arg3">The 3. argument.</param>
        /// <param name="arg4">The 4. argument.</param>
        public static UIAction Create<TArg1, TArg2, TArg3, TArg4>(
            Action<TArg1, TArg2, TArg3, TArg4> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4
        )
        {
            return CreateDelegateAction(action, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        /// <param name="arg3">The 3. argument.</param>
        /// <param name="arg4">The 4. argument.</param>
        /// <param name="arg5">The 5. argument.</param>
        public static UIAction Create<TArg1, TArg2, TArg3, TArg4, TArg5>(
            Action<TArg1, TArg2, TArg3, TArg4, TArg5> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5
        )
        {
            return CreateDelegateAction(action, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        /// <param name="arg3">The 3. argument.</param>
        /// <param name="arg4">The 4. argument.</param>
        /// <param name="arg5">The 5. argument.</param>
        /// <param name="arg6">The 6. argument.</param>
        public static UIAction Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(
            Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6
        )
        {
            return CreateDelegateAction(action, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        /// <param name="arg3">The 3. argument.</param>
        /// <param name="arg4">The 4. argument.</param>
        /// <param name="arg5">The 5. argument.</param>
        /// <param name="arg6">The 6. argument.</param>
        /// <param name="arg7">The 7. argument.</param>
        public static UIAction Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(
            Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7
        )
        {
            return CreateDelegateAction(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="action">The function to run when the action is triggered.</param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        /// <param name="arg3">The 3. argument.</param>
        /// <param name="arg4">The 4. argument.</param>
        /// <param name="arg5">The 5. argument.</param>
        /// <param name="arg6">The 6. argument.</param>
        /// <param name="arg7">The 7. argument.</param>
        /// <param name="arg8">The 8. argument.</param>
        public static UIAction Create<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
            Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> action, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8
        )
        {
            return CreateDelegateAction(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        #endregion

        #region Function
        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="function"><inheritdoc cref="actionDelegate" path="//summary"/></param>
        public static UIAction Create<TRet>(Func<TRet> function)
        {
            return CreateDelegateAction(function);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="function"><inheritdoc cref="actionDelegate" path="//summary"/></param>
        /// <param name="arg">The 1. argument.</param>
        public static UIAction Create<TArg, TRet>(Func<TArg, TRet> function, TArg arg)
        {
            return CreateDelegateAction(function, arg);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="function"><inheritdoc cref="actionDelegate" path="//summary"/></param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        public static UIAction Create<TArg1, TArg2, TRet>(Func<TArg1, TArg2, TRet> function, TArg1 arg1, TArg2 arg2)
        {
            return CreateDelegateAction(function, arg1, arg2);
        }

        /// <summary>
        /// <inheritdoc cref="UIAction"/>
        /// </summary>
        /// <param name="function"><inheritdoc cref="actionDelegate" path="//summary"/></param>
        /// <param name="arg1">The 1. argument.</param>
        /// <param name="arg2">The 2. argument.</param>
        /// <param name="arg3">The 3. argument.</param>
        public static UIAction Create<TArg1, TArg2, TArg3, TRet>(Func<TArg1, TArg2, TArg3, TRet> function, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return CreateDelegateAction(function, arg1, arg2, arg3);
        }
        #endregion
        #endregion

        #region Public methods
        /// <summary>
        /// Invokes the action and returns the value that it returned.<br/>
        /// - if the action invokes a function, it gets called, with arguments, and an optional extra parameter as the first argument.<br/>
        /// - if the action invokes a <see cref="UIList"/>, the object's <see cref="UIList.Display(IEnumerable{KeyAction}?, GetKeyFunctionDelegate?)"/> function gets called.
        /// </summary>
        /// <param name="extraParameter">Extra parameter to put at the beginning of the arguments list.</param>
        /// <param name="keybinds">The list of <see cref="KeyAction"/> objects to use, if the action is calling a <see cref="UIList"/>.</param>
        /// <param name="getKeyFunction">The function to get the next valid key the user pressed.</param>
        public (UIActionType actionType, object? returned) InvokeAction(
            object? extraParameter = null,
            IEnumerable<KeyAction>? keybinds = null,
            GetKeyFunctionDelegate? getKeyFunction = null
        )
        {
            switch (actionType)
            {
                case UIActionType.FUNCTION:
                    if (extraParameter is null && (actionParameters is null || actionParameters.Length == 0))
                    {
                        return (UIActionType.FUNCTION, actionDelegate?.DynamicInvoke(null));
                    }

                    var args = new List<object?>();
                    if (extraParameter is not null)
                    {
                        args.Add(extraParameter);
                    }
                    if (actionParameters is not null && actionParameters.Length != 0)
                    {
                        args.AddRange(actionParameters);
                    }
                    return (UIActionType.FUNCTION, actionDelegate?.DynamicInvoke([.. args]));
                case UIActionType.UILIST:
                    return (actionType, actionUIList?.Display(keybinds, getKeyFunction));
                case UIActionType.OPTIONSUI:
                    return (actionType, actionOptionsUI?.Display(keybinds, getKeyFunction));
                default:
                    throw new ArgumentException($"Unknown action type: \"{actionType}\"!");
            }
        }
        #endregion
    }
}
