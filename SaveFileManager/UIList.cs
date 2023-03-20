using System.Collections;
using System.Text;

namespace SaveFileManager
{
    public class UIList
    {
        public IEnumerable<string?> answers;
        public string question;
        public CursorIcon cursorIcon;
        public bool multiline;
        public bool canEscape;
        public IEnumerable<object?> actions;
        public bool excludeNulls;
        public bool modifyList;

        /// <summary>
        /// Object for displaying a terminal UI using the <c>display</c> function.<br/>
        /// Prints the <c>question</c> and then the list of answers from that the user can cycle between with the selected keys (arrow keys by default) and select them.<br/>
        /// Returns a number acording to the index of the selcected element in the <c>answers</c> list (influenced by <c>excludeNulls</c>), or -1 if the user exited. (Or a list, if the action for thar answer was a specific function.)<br/>
        /// </summary>
        /// <param name="answers">A list of answers, the user ca select.<br/>
        /// If an element in the list is null the line will be blank and cannot be selected.</param>
        /// <param name="question">The string to print before the answers.</param>
        /// <param name="cursorIcon">The cursor icon style to use.</param>
        /// <param name="multiline">Makes the "cursor" draw at every line if the text is multiline.</param>
        /// <param name="canEscape">Allows the user to press the key associated with escape, to exit the menu. In this case the <c>display</c> function returns -1.</param>
        /// <param name="actions">If the list is not emptiy or null, each element coresponds to an element in the <c>answers</c> list, and if the value is a function (or a list with a function as the 1. element, and arguments as the 2-n.element), it will run that function.<br/>
        /// - If the function returns -1 the <c>display</c> function will instantly exit.<br/>
        /// - If the function returns a list where the first element is -1 the <c>display</c> function will instantly return that list with the first element replaced by the selected answer's number.<br/>
        /// - If it is a <c>UIList</c> object, the object's <c>display</c> function will be automaticly called, allowing for nested menus.</param>
        /// <param name="excludeNulls">If true, the selected option will not see non-selectable elements as part of the list. This also makes it so you don't have to put a placeholder value in the <c>actions</c> list for every null value in the <c>answers</c> list.</param>
        /// <param name="modifiableUIList">If true, any function in the <c>actions</c> list will get the <c>UIList</c> as it's first argument (and can modify it) when the function is called.</param>
        public UIList(IEnumerable<string?> answers, string? question = null, CursorIcon? cursorIcon = null, bool multiline = false, bool canEscape = false, IEnumerable<object?>? actions = null, bool excludeNulls = false, bool modifiableUIList = false)
        {
            this.answers = answers;
            this.question = question ?? "";
            this.cursorIcon = cursorIcon ?? new CursorIcon();
            this.multiline = multiline;
            this.canEscape = canEscape;
            this.actions = actions ?? new List<object>();
            this.excludeNulls = excludeNulls;
            this.modifyList = modifiableUIList;
        }

        /// <summary>
        /// Returns the text that represents the UI of this object, without the question.
        /// </summary>
        /// <param name="selected">The index of the currently selected answer.</param>
        /// <returns></returns>
        private string MakeText(int selected)
        {
            var text = new StringBuilder();
            for (var x = 0; x < answers.Count(); x++)
            {
                var answer = answers.ElementAt(x);
                if (answer is not null)
                {
                    string currIcon;
                    string currIconR;
                    if (selected == x)
                    {
                        currIcon = cursorIcon.sIcon;
                        currIconR = cursorIcon.sIconR;
                    }
                    else
                    {
                        currIcon = cursorIcon.icon;
                        currIconR = cursorIcon.iconR;
                    }
                    text.Append(currIcon + (multiline ? answer.Replace("\n", $"{currIconR}\n{currIcon}") : answer) + $"{currIconR}\n");
                }
                else
                {
                    text.Append("\n");
                }
            }
            return text.ToString();
        }

        /// <summary>
        /// Converts the selected answer number to the actual number depending on if <c>excludeNulls</c> is true.
        /// </summary>
        /// <param name="selected">The selected answer's number.</param>
        /// <returns></returns>
        private int ConvertSelected(int selected)
        {
            if (excludeNulls)
            {
                var selectedF = selected;
                for (var x = 0; x < answers.Count(); x++)
                {
                    if (answers.ElementAt(x) is null)
                    {
                        selectedF--;
                    }
                    if (x == selected)
                    {
                        selected = selectedF;
                        break;
                    }
                }
            }
            return selected;
        }

        /// <summary>
        /// Moves the selection depending on the input, in a way, where the selection can't land on an empty line.
        /// </summary>
        /// <param name="selected">The selected answer's number.</param>
        /// <param name="keyResult">The result object retrned from presing a key.</param>
        /// <param name="keyResults">The list of posible results returned by pressing a key.</param>
        private int MoveSelection(int selected, object keyResult, IEnumerable<object> keyResults)
        {
            if (!keyResult.Equals(keyResults.ElementAt((int)Key.ENTER)))
            {
                var moveAmount = keyResult.Equals(keyResults.ElementAt((int)Key.DOWN)) ? 1 : -1;
                while (true)
                {
                    selected += moveAmount;
                    selected %= answers.Count();
                    if (selected < 0)
                    {
                        selected = answers.Count() - 1;
                    }
                    if (answers.ElementAt(selected) is not null)
                    {
                        break;
                    }
                }
            }
            return selected;
        }

        /// <summary>
        /// Handles what to return for the selected answer.
        /// </summary>
        /// <param name="selected">The selected answer's number.</param>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use, if the selected action is a <c>UIList</c>.</param>
        /// <param name="keyResults">The list of posible results returned by pressing a key. Used, if the selected action is a <c>UIList</c>.</param>
        /// <returns></returns>
        private object? HandleAction(int selected, IEnumerable<KeyAction>? keybinds = null, IEnumerable<object>? keyResults = null)
        {
            if (actions.Count() > 0 && selected < actions.Count() && actions.ElementAt(selected) is not null)
            {
                var selectedAction = actions.ElementAt(selected);
                // list
                if (selectedAction is not null &&
                    typeof(IEnumerable).IsAssignableFrom(selectedAction.GetType()) &&
                    ((IEnumerable<object>)selectedAction).Count() >= 2 &&
                    ((IEnumerable<object>)selectedAction).ElementAt(0) is Delegate)
                {
                    var selectedActionList = (IEnumerable<object>)selectedAction;
                    var function = (Delegate)selectedActionList.ElementAt(0);
                    var paramNum = selectedActionList.Count() + (modifyList ? 0 : -1);
                    var parameters = new object[paramNum];
                    var index = 0;
                    if (modifyList)
                    {
                        parameters[index] = this;
                        index++;
                    }
                    for (var x = 1; x < selectedActionList.Count(); x++)
                    {
                        parameters[index] = selectedActionList.ElementAt(x);
                        index++;
                    }
                    var funcReturn = function.DynamicInvoke(parameters);
                    if (funcReturn is int && (int)funcReturn == -1)
                    {
                        return selected;
                    }
                    else if (
                        funcReturn is not null &&
                        typeof(IEnumerable).IsAssignableFrom(funcReturn.GetType()) &&
                        ((IEnumerable<object>)funcReturn).Count() >= 1 &&
                        ((IEnumerable<object>)funcReturn).ElementAt(0) is int &&
                        (int)((IEnumerable<object>)funcReturn).ElementAt(0) == -1
                    )
                    {
                        var funcRetList = ((IEnumerable<object>)funcReturn).ToList();
                        funcRetList[0] = selected;
                        return funcRetList;
                    }
                    else
                    {
                        return null;
                    }
                }
                // normal function
                else if (selectedAction is Delegate)
                {
                    var selectedFunc = (Delegate)selectedAction;
                    object? funcReturn;
                    if (modifyList)
                    {
                        funcReturn = selectedFunc.DynamicInvoke(this);
                    }
                    else
                    {
                        funcReturn = selectedFunc.DynamicInvoke();
                    }
                    if (funcReturn is int && (int)funcReturn == -1)
                    {
                        return selected;
                    }
                    else if (
                        funcReturn is not null &&
                        typeof(IEnumerable).IsAssignableFrom(funcReturn.GetType()) &&
                        ((IEnumerable<object>)funcReturn).Count() >= 1 &&
                        ((IEnumerable<object>)funcReturn).ElementAt(0) is int &&
                        (int)((IEnumerable<object>)funcReturn).ElementAt(0) == -1
                    )
                    {
                        var funcRetList = ((IEnumerable<object>)funcReturn).ToList();
                        funcRetList[0] = selected;
                        return funcRetList;
                    }
                    else
                    {
                        return null;
                    }
                }
                // ui
                else if (selectedAction is UIList)
                {
                    ((UIList)selectedAction).Display(keybinds, keyResults);
                    return null;
                }
                else
                {
                    //Console.WriteLine("Option is not a UIList object!");
                    return selected;
                }
            }
            else
            {
                return selected;
            }
        }

        /// <summary>
        /// Returns a selected until it's not on an empty space.
        /// </summary>
        /// <param name="selected">The selected answer's number.</param>
        /// <returns></returns>
        private int SetupSelected(int selected)
        {
            if (selected > answers.Count() - 1)
            {
                selected = answers.Count() - 1;
            }
            while (answers.ElementAt(selected) is null)
            {
                selected++;
                if (selected > answers.Count() - 1)
                {
                    selected = 0;
                }
            }
            return selected;
        }

        /// <summary>
        /// SEE OBJECT FOR A MORE DETAILED DOCUMENTATION!
        /// Prints the <c>question</c> and then the list of answers from the <c>answers</c> list that the user can cycle between and select with the keys in the <c>keybinds</c>.<br/>
        ///The order of the elements in the tuple should be:<br/>
        /// - escape, up, down, left, right, enter<br/>
        ///If it is null, the default value is either returned from the <c>keybinds</c> or:<br/>
        /// - { Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER }
        /// </summary>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use, if the selected action is a <c>UIList</c>.</param>
        /// <param name="keyResults">The list of posible results returned by pressing a key. Used, if the selected action is a <c>UIList</c>.</param>
        /// <returns></returns>
        public object Display(IEnumerable<KeyAction>? keybinds = null, IEnumerable<object>? keyResults = null)
        {
            if (keyResults is null)
            {
                if (keybinds is null)
                {
                    keyResults = new List<object> { Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER };
                }
                else
                {
                    keyResults = KeyAction.GetKeyResultsList(keybinds);
                }
            }

            var selected = SetupSelected(0);
            while (true)
            {
                selected = SetupSelected(selected);
                var key = keyResults.ElementAt((int)Key.ESCAPE);
                while (!key.Equals(keyResults.ElementAt((int)Key.ENTER)))
                {
                    // render
                    // clear screen
                    var txt = new StringBuilder("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    if (question is not null)
                    {
                        txt.Append(question + "\n\n");
                    }
                    txt.Append(MakeText(selected));
                    Console.WriteLine(txt);
                    // answer select
                    key = Utils.GetKey(GetKeyMode.IGNORE_HORIZONTAL, keybinds);
                    if (canEscape && key.Equals(keyResults.ElementAt((int)Key.ESCAPE)))
                    {
                        return -1;
                    }
                    while (key.Equals(keyResults.ElementAt((int)Key.ESCAPE)))
                    {
                        key = Utils.GetKey(GetKeyMode.IGNORE_HORIZONTAL, keybinds);
                    }
                    selected = MoveSelection(selected, key, keyResults);
                }
                // menu actions
                selected = ConvertSelected(selected);
                var action = HandleAction(selected, keybinds, keyResults);
                if (action is not null)
                {
                    return action;
                }
            }
        }
    }
}
