using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace SaveFileManager
{
    public class UIList
    {
        IEnumerable<string?> answers;
        string question;
        CursorIcon cursorIcon;
        bool multiline;
        bool canEscape;
        IEnumerable<object?> actions;
        bool excludeNulls;
        bool modifyList;

        /// <summary>
        /// Object for displaying a terminal UI using the <c>display</c> function.<br/>
        /// Prints the <c>question</c> and then the list of answers from that the user can cycle between with the selected keys (arrow keys by default) and select them.<br/>
        /// Gives back a number acording to the index of the selcected element in the <c>answers</c> list (influenced by <c>excludeNulls</c>), or -1 if the user exited.<br/>
        /// </summary>
        /// <param name="answers">A list of answers, the user ca select.<br/>
        /// If an element in the list is null the line will be blank and cannot be selected.</param>
        /// <param name="question">The string to print before the answers.</param>
        /// <param name="cursorIcon">The cursor icon style to use.</param>
        /// <param name="multiline">Makes the "cursor" draw at every line if the text is multiline.</param>
        /// <param name="canEscape">Allows the user to press the key associated with escape, to exit the menu. In this case the <c>display</c> function returns -1.</param>
        /// <param name="actions">If the list is not emptiy or null, each element coresponds to an element in the <c>answers</c> list, and if the value is a function (or a list with a function as the 1. element, and arguments as the 2-n.element, including 1 or more dictionaries as **kwargs), it will run that function.<br/>
        /// - If the function returns -1 the <c>display</c> function will instantly exit.<br/>
        /// - If the function returns a list where the first element is -1 the <c>display</c> function will instantly return that list with the first element replaced by the selected answer's number.<br/>
        /// - If it is a <c>UIList</c> object, the object's <c>display</c> function will be automaticly called, allowing for nested menus.</param>
        /// <param name="excludeNulls">If true, the selected option will not see non-selectable elements as part of the list. This also makes it so you don't have to put a placeholder value in the <c>actions</c> list for every null value in the <c>answers</c> list.</param>
        /// <param name="modifyList">If true, any function in the <c>actions</c> list will get a list containing the <c>answers</c> list and the <c>actions</c> list as it's first argument (and can modify it) when the function is called.</param>
        public UIList(IEnumerable<string?> answers, string? question = null, CursorIcon? cursorIcon = null, bool multiline = false, bool canEscape = false, IEnumerable<object?>? actions = null, bool excludeNulls = false, bool modifyList = false)
        {
            this.answers = answers;
            this.question = question ?? "";
            this.cursorIcon = cursorIcon ?? new CursorIcon();
            this.multiline = multiline;
            this.canEscape = canEscape;
            this.actions = actions ?? new List<object>();
            this.excludeNulls = excludeNulls;
            this.modifyList = modifyList;
        }

        /// <summary>
        /// Short version of <c>UIList</c>.<br/>
        /// UIList(answers, question, cursorIcon=null, multiline, canEscape, actions=null, excludeNulls, modifyList=false)
        /// </summary>
        /// <inheritdoc cref="UIList(IEnumerable{string?}, string?, CursorIcon?, bool, bool, IEnumerable{object?}?, bool, bool)"/>
        public UIList(IEnumerable<string?> answers, string? question, bool multiline = false, bool canEscape = false, bool excludeNulls = false) :
            this(answers, question, null, multiline, canEscape, null, excludeNulls, false) { }

        /// <summary>
        /// A version of <c>UIList</c> for use in <c>optionsUI</c>.<br/>
        /// UIList(text, question=null, cursorIcon, multiline, canEscape=false, action, excludeNulls=false, modify)
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="action">The action when selecting the button.</param>
        /// <inheritdoc cref="UIList(IEnumerable{string?}, string?, CursorIcon?, bool, bool, IEnumerable{object?}?, bool, bool)"/>
        public UIList(string text, object? action, bool multiline = false, bool modify = false, CursorIcon? cursorIcon = null) :
            this(new List<string> { text }, null, cursorIcon, multiline, false, action is null ? null : new List<object?> { action }, false, modify) { }

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
            var moveAmount = keyResult == keyResults.ElementAt((int)Key.DOWN) ? 1 : -1;
            if (keyResult != keyResults.ElementAt((int)Key.ENTER))
            {
                while (true)
                {
                    selected += moveAmount;
                    selected %= answers.Count();
                    if (selected < 0)
                    {
                        selected = 0;
                    }
                    if (answers.ElementAt(selected) is not null)
                    {
                        break;
                    }
                }
            }
            return selected;
        }

    //    /// <summary>
    //    /// Handles what to return for the selected answer.
    //    /// </summary>
    //    /// <param name="selected">The selected answer's number.</param>
    //    /// <param name="keybinds">The list of <c>KeyAction</c> objects to use, if the selected action is a <c>UIList</c>.</param>
    //    /// <param name="keyResults">The list of posible results returned by pressing a key. Used, if the selected action is a <c>UIList</c>.</param>
    //    /// <returns></returns>
    //    private object HandleAction(int selected, IEnumerable<KeyAction>? keybinds = null, IEnumerable<object>? keyResults = null)
    //    {
    //        if (actions.Count() == 0 && selected < actions.Count() && actions.ElementAt(selected) is not null)
    //        {
    //            var selectedAction = actions.ElementAt(selected);
    //            // list
    //            if (selectedAction is not null && selectedAction.GetType() is IEnumerable && ((IEnumerable<object>)selectedAction).Count() >= 2)
    //            {
    //                var selectedActionList = (IEnumerable<object>)selectedAction;
    //                var lis = new List<object>();
    //                var dict = new Dictionary<string, object>();
    //                foreach (var elem in selectedActionList)
    //                {
    //                    if (elem.GetType() is IDictionary)
    //                    {
    //                        dict.Add(elem);
    //                    }
    //                    else
    //                    {
    //                        lis.Add(elem);
    //                    }
    //                }
    //                if self.modify_list:
    //                    func_return = lis[0]([self.answer_list, self.action_list], *lis[1:], **dict)
    //                else:
    //                    func_return = lis[0](*lis[1:], **dict)
    //                if func_return == -1:
    //                    return selected
    //                elif type(func_return) is list and func_return[0] == -1:
    //                    func_return[0] = selected
    //                    return func_return
    //            }
    //            // normal function
    //            else if (callable(selected_action))
    //            {
    //                if self.modify_list:
    //                    func_return = selected_action([self.answer_list, self.action_list])
    //                else:
    //                    func_return = selected_action()
    //                if func_return == -1:
    //                    return selected
    //                elif type(func_return) is list and func_return[0] == -1:
    //                    func_return[0] = selected
    //                    return func_return
    //            }
    //            // ui
    //            else if (isinstance(selected_action, UI_list))
    //            {
    //                selected_action.display(keybinds, allow_buffered_inputs, result_list);
    //            }
    //            else
    //            {
    //                Console.WriteLine("Option is not a UIList object!");
    //                return selected;
    //            }
    //        }
    //        else
    //        {
    //            return selected;
    //        }
    //    }


    //def _setup_selected(self, selected:int) :
    //    """Returns a selected until it's not on an empty space."""
    //    if selected > len(self.answer_list) - 1:
    //        selected = len(self.answer_list) - 1
    //    while self.answer_list[selected] is None:
    //        selected += 1
    //        if selected > len(self.answer_list) - 1:
    //            selected = 0
    //    return selected


    //def display(self, keybinds:Keybinds|None= None, allow_buffered_inputs= False, result_list:tuple[Any, Any, Any, Any, Any, Any]|None= None) :
    //    """
    //    Prints the `question` and then the list of answers from the `answer_list` that the user can cycle between with the arrow keys and select with enter.\n
    //    Gives back a number from 0-n acording to the size of the list that was passed in.\n
    //    If `exclude_nones` is `True`, the selected option will not see non-selectable elements as part of the list. This also makes it so you don't have to put a placeholder value in the `action_list` for every `None` value in the `answer_list`.\n
    //    if the answer is None the line will be blank and cannot be selected. \n
    //    `multiline` makes the "cursor" draw at every line if the text is multiline.\n
    //    `can_esc` allows the user to press esc to exit the menu. In this case the function returns -1.\n
    //    If the `action_list` is not empty, each element coresponds to an element in the `answer_list`, and if the value is a function (or a list with a function as the 1. element, and arguments as the 2-n. element, including 1 or more dictionaries as **kwargs), it will run that function.\n
    //    - If the function returns -1 the `display` function will instantly exit.\n
    //    - If the function returns a list where the first element is -1 the `display` function will instantly return that list with the first element replaced by the selected element number of that `UI_list` object.\n
    //    - If it is a `UI_list` object, the object's `display` function will be automaticly called, allowing for nested menus.\n
    //    - If `modify_list` is `True`, any function (that is not a `UI_list` object) that is in the `action_list` will get a list containing the `answer_list` and the `action_list` as it's first argument (and can modify it) when the function is called.\n
    //    If `allow_buffered_inputs` is `False`, if the user pressed some buttons before this function was called the function will not register those button presses.
    //    If `result_list` is not None, it will use the values in that list to match with the return value of the `get_key_with_obj()`.\n
    //    The order of the elements in the tuple should be:\n
    //    \t(escape, up, down, left, right, enter)\n
    //    If it is None, the default value is:\n
    //    \t`(Keys.ESCAPE, Keys.UP, Keys.DOWN, Keys.LEFT, Keys.RIGHT, Keys.ENTER)`
    //    """
    //    if result_list is None:
    //        result_list = (Keys.ESCAPE, Keys.UP, Keys.DOWN, Keys.LEFT, Keys.RIGHT, Keys.ENTER)

    //    selected = self._setup_selected(0)
    //    while True:
    //        selected = self._setup_selected(selected)
    //        key = Keys.ESCAPE
    //        while key != Keys.ENTER:
    //            # render
    //            # clear screen
    //            txt = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n"
    //            if self.question is not None:
    //                txt += self.question + "\n\n"
    //            txt += self._make_text(selected)
    //            print(txt)
    //            # answer select
    //            key = get_key_with_obj(Get_key_modes.IGNORE_HORIZONTAL, keybinds, allow_buffered_inputs)
    //            if self.can_esc and key == Keys.ESCAPE:
    //                return -1
    //            while key == Keys.ESCAPE:
    //                key = get_key_with_obj(Get_key_modes.IGNORE_HORIZONTAL, keybinds, allow_buffered_inputs)
    //            selected = self._move_selection(selected, key, result_list)
    //    // menu actions
    //        selected = self._convert_selected(selected)
    //        action = self._handle_action(selected, keybinds, allow_buffered_inputs, result_list)
    //        if action is not None:
    //            return action
    }
}
