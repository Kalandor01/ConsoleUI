using System.Collections;
using System.Text;

namespace SaveFileManager
{
    /// <summary>
    /// Object for displaying a terminal UI using the <c>Display</c> function.<br/>
    /// Prints the <c>question</c> and then the list of answers that the user can cycle between with the selected keys (arrow keys by default) and select them.<br/>
    /// Returns a number acording to the index of the selcected element in the <c>answers</c> list (influenced by <c>excludeNulls</c>), or -1 if the user exited. (Or a list, if the action for thar answer was a specific function.)
    /// </summary>
    public class UIList
    {
        #region Public fields
        /// <summary>
        /// A list of answers, the user can select.<br/>
        /// If an element in the list is null the line will be blank and cannot be selected.
        /// </summary>
        public IEnumerable<string?> answers;
        /// <summary>
        /// The string to print before the answers.
        /// </summary>
        public string? question;
        /// <summary>
        /// The <c>CursorIcon</c> to use.
        /// </summary>
        public CursorIcon cursorIcon;
        /// <summary>
        /// Makes the "cursor" draw at every line if the text is multiline.
        /// </summary>
        public bool multiline;
        /// <summary>
        /// Allows the user to press the key associated with escape, to exit the menu. In this case the <c>display</c> function returns -1.
        /// </summary>
        public bool canEscape;
        /// <summary>
        /// If the list is not emptiy or null, each element coresponds to an element in the <c>answers</c> list.<br/>
        /// - If the action type is function, and it returns -1 the <c>display</c> function will instantly exit.<br/>
        /// - If the function returns a list where the first element is -1 the <c>Display</c> function will instantly return that list with the first element replaced by the selected answer's number.
        /// </summary>
        public IEnumerable<UIAction?> actions;
        /// <summary>
        /// If true, the selected option will not see non-selectable elements as part of the list. This also makes it so you don't have to put a placeholder value in the <c>actions</c> list for every null value in the <c>answers</c> list.
        /// </summary>
        public bool excludeNulls;
        /// <summary>
        /// If true, any function in the <c>actions</c> list will get the <c>UIList</c> as its first argument (and can modify it) when the function is called.
        /// </summary>
        public bool modifyList;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="UIList"/>
        /// </summary>
        /// <param name="answers"><inheritdoc cref="answers" path="//summary"/></param>
        /// <param name="question"><inheritdoc cref="question" path="//summary"/></param>
        /// <param name="cursorIcon"><inheritdoc cref="cursorIcon" path="//summary"/></param>
        /// <param name="multiline"><inheritdoc cref="multiline" path="//summary"/></param>
        /// <param name="canEscape"><inheritdoc cref="canEscape" path="//summary"/></param>
        /// <param name="actions"><inheritdoc cref="actions" path="//summary"/></param>
        /// <param name="excludeNulls"><inheritdoc cref="excludeNulls" path="//summary"/></param>
        /// <param name="modifiableUIList"><inheritdoc cref="modifyList" path="//summary"/></param>
        /// <exception cref="UINoSelectablesExeption"></exception>
        public UIList(IEnumerable<string?> answers, string? question = null, CursorIcon? cursorIcon = null, bool multiline = false, bool canEscape = false, IEnumerable<UIAction?>? actions = null, bool excludeNulls = false, bool modifiableUIList = false)
        {
            if (!answers.Any() || answers.All(answer => answer is null))
            {
                throw new UINoSelectablesExeption();
            }
            this.answers = answers;
            this.question = question;
            this.cursorIcon = cursorIcon ?? new CursorIcon();
            this.multiline = multiline;
            this.canEscape = canEscape;
            this.actions = actions ?? new List<UIAction?>();
            this.excludeNulls = excludeNulls;
            this.modifyList = modifiableUIList;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// SEE OBJECT FOR A MORE DETAILED DOCUMENTATION!<br/>
        /// Prints the <c>question</c> and then the list of answers from the <c>answers</c> list that the user can cycle between and select with the keys in the <c>keybinds</c>.
        /// </summary>
        /// <param name="keybinds">The list of <c>KeyAction</c> objects to use, if the selected action is a <c>UIList</c>.</param>
        /// <param name="keyResults">The list of posible results returned by pressing a key.<br/>
        /// The order of the elements in the list should be:<br/>
        /// - escape, up, down, left, right, enter<br/>
        ///If it is null, the default value is either returned from the <c>keybinds</c> or:<br/>
        /// - { Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER }</param>
        public object Display(IEnumerable<KeyAction>? keybinds = null, IEnumerable<object>? keyResults = null)
        {
            if (keyResults is null || keyResults.Count() < 6)
            {
                if (keybinds is null)
                {
                    keyResults = new List<object> { Key.ESCAPE, Key.UP, Key.DOWN, Key.LEFT, Key.RIGHT, Key.ENTER };
                }
                else
                {
                    keyResults = Utils.GetResultsList(keybinds);
                }
            }

            var selected = SetupSelected(0);
            while (true)
            {
                selected = SetupSelected(selected);
                var key = keyResults.ElementAt((int)Key.ESCAPE);
                while (!key.Equals(keyResults.ElementAt((int)Key.ENTER)))
                {
                    // clear screen + render
                    var txt = new StringBuilder("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");

                    // question
                    if (question is not null)
                    {
                        txt.Append(question + "\n\n");
                    }

                    // answers
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
        #endregion

        #region Private methods
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
                    text.Append('\n');
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
            if (
                actions.Any() &&
                selected < actions.Count() &&
                actions.ElementAt(selected) is not null)
            {
                var selectedAction = actions.ElementAt(selected);
                var (actionType, returned) = selectedAction.InvokeAction(modifyList ? this : null, keybinds, keyResults);
                if (actionType == UIActionType.UILIST)
                {
                    return null;
                }
                else
                {
                    if (returned is int funcInt && funcInt == -1)
                    {
                        return selected;
                    }
                    else if (
                        returned is not null &&
                        returned.GetType() != typeof(string) &&
                        typeof(IEnumerable).IsAssignableFrom(returned.GetType()) &&
                        ((IEnumerable<object>)returned).Any() &&
                        ((IEnumerable<object>)returned).ElementAt(0) is int funcArgsInt &&
                        funcArgsInt == -1
                    )
                    {
                        var funcRetList = ((IEnumerable<object>)returned).ToList();
                        funcRetList[0] = selected;
                        return funcRetList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return selected;
            }
        }

        /// <summary>
        /// Returns a selected until its not on an empty space.
        /// </summary>
        /// <param name="selected">The selected answer's number.</param>
        /// <returns></returns>
        private int SetupSelected(int selected)
        {
            selected = Math.Clamp(selected, 0, answers.Count() - 1);
            while (answers.ElementAt(selected) is null)
            {
                selected = (selected + 1) % answers.Count();
            }
            return selected;
        }
        #endregion
    }
}
