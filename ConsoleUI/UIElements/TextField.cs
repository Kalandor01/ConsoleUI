﻿using ConsoleUI.Keybinds;
using ConsoleUI.UIElements.EventArgs;
using System.Text;

namespace ConsoleUI.UIElements
{
    /// <summary>
    /// Object for the <c>OptionsUI</c> method.<br/>
    /// When used as input in the <c>OptionsUI</c> function, it draws a field for a string, that can be selected to edit it's value in place, with the enter action.<br/>
    /// Structure: [<c>preText</c>][<c>value</c>][<c>postValue</c>]
    /// </summary>
    public class TextField : BaseUI
    {
        #region Public fields
        /// <summary>
        /// The current value of the object.
        /// </summary>
        public new string Value { get; set; }
        /// <summary>
        /// Wether it should have an emplty string, or the old value, when editing the value.
        /// </summary>
        public bool oldValueAsStartingValue;
        /// <summary>
        /// The maximum length of the input. By default it's the width of the console window. Set to -1 to set it to unlimited.
        /// </summary>
        public int? maxInputLength;
        /// <summary>
        /// Wether to interpret string lengths as the length of the string as it will be displayed in the terminal, or just the string.Length.
        /// </summary>
        public bool lengthAsDisplayLength;
        /// <summary>
        /// The function to run on the input.
        /// </summary>
        public TextValidatorDelegate? textValidatorFunction;
        /// <summary>
        /// The function to run at every keypress.
        /// </summary>
        public KeyValidatorDelegate? keyValidatorFunction;
        /// <summary>
        /// Whether to run the the default key validator function, before the one passed in as a parameter, or not.<br/>
        /// (true = don't run the default validator)
        /// </summary>
        public bool overrideDefaultKeyValidatorFunction;
        /// <summary>
        /// Whether ANSI escape codes are enabled.
        /// </summary>
        public bool escapeCodesEnabled;
        /// <summary>
        /// The function used to read the input text from the user.<br/>
        /// Uses <see cref="IConsoleProxy.ReadKey(bool)"/> by default.
        /// </summary>
        public ReadKeyDelegate? readKeyFunction;
        #endregion

        #region Public Properties
        /// <summary>
        /// <inheritdoc cref="BaseUI.preText"/>
        /// </summary>
        public string PreText
        {
            get => preText;
            set
            {
                preText = value;
            }
        }
        /// <summary>
        /// <inheritdoc cref="BaseUI.postValue"/>
        /// </summary>
        public string PostValue
        {
            get => postValue;
            set
            {
                postValue = value;
            }
        }
        #endregion

        #region Override properties
        /// <inheritdoc cref="BaseUI.IsClickable"/>
        public override bool IsClickable { get => true; }

        /// <inheritdoc cref="BaseUI.IsOnlyClickable"/>
        public override bool IsOnlyClickable { get => true; }
        #endregion

        #region Public delegates
        /// <summary>
        /// A function to return the status of the value, the user inputed.
        /// </summary>
        /// <param name="inputValue">The value that the user inputed.</param>
        public delegate (TextFieldValidatorStatus status, string? message) TextValidatorDelegate(string inputValue);

        /// <summary>
        /// A function to return if the key the user inputed is valid or not.
        /// </summary>
        /// <param name="currentValue">The currently typed value (not including the current key).</param>
        /// <param name="inputKey">The key that the user inputed.<br/>
        /// null if a key will be removed instead of added. (can only happen if "overrideDefaultKeyValidatorFunction" is false)</param>
        /// <param name="cursorPosition">The position of the cursor before the inputKey was inserted.<br/>
        /// If a key was removed, this value is the position of the character that will be removed instead.</param>
        public delegate bool KeyValidatorDelegate(StringBuilder currentValue, ConsoleKeyInfo? inputKey, int cursorPosition);

        /// <summary>
        /// Reads a key from the user.
        /// </summary>
        public delegate ConsoleKeyInfo ReadKeyDelegate();
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="TextField"/>
        /// </summary>
        /// <param name="value"><inheritdoc cref="Value" path="//summary"/></param>
        /// <param name="preText"><inheritdoc cref="BaseUI.preText" path="//summary"/></param>
        /// <param name="postValue"><inheritdoc cref="BaseUI.postValue" path="//summary"/></param>
        /// <param name="multiline"><inheritdoc cref="BaseUI.multiline" path="//summary"/></param>
        /// <param name="oldValueAsStartingValue"><inheritdoc cref="oldValueAsStartingValue" path="//summary"/></param>
        /// <param name="maxInputLength"><inheritdoc cref="maxInputLength" path="//summary"/></param>
        /// <param name="lengthAsDisplayLength"><inheritdoc cref="lengthAsDisplayLength" path="//summary"/></param>
        /// <param name="textValidatorFunction"><inheritdoc cref="textValidatorFunction" path="//summary"/></param>
        /// <param name="keyValidatorFunction"><inheritdoc cref="keyValidatorFunction" path="//summary"/></param>
        /// <param name="overrideDefaultKeyValidatorFunction"><inheritdoc cref="overrideDefaultKeyValidatorFunction" path="//summary"/></param>
        /// <param name="ansiEscapeCodesEnabled"><inheritdoc cref="escapeCodesEnabled" path="//summary"/>></param>
        /// <param name="readKeyFunction"><inheritdoc cref="readKeyFunction" path="//summary"/></param>
        public TextField(
            string value,
            string preText = "",
            string postValue = "",
            bool multiline = false,
            bool oldValueAsStartingValue = false,
            int? maxInputLength = null,
            bool lengthAsDisplayLength = true,
            TextValidatorDelegate? textValidatorFunction = null,
            KeyValidatorDelegate? keyValidatorFunction = null,
            bool overrideDefaultKeyValidatorFunction = true,
            bool ansiEscapeCodesEnabled = true,
            ReadKeyDelegate? readKeyFunction = null
        )
            : base(-1, preText, "", false, postValue, multiline)
        {
            this.preText = preText;
            Value = value;
            this.postValue = postValue;

            this.oldValueAsStartingValue = oldValueAsStartingValue;
            this.maxInputLength = maxInputLength;
            this.lengthAsDisplayLength = lengthAsDisplayLength;
            this.textValidatorFunction = textValidatorFunction;
            this.keyValidatorFunction = keyValidatorFunction;
            this.overrideDefaultKeyValidatorFunction = overrideDefaultKeyValidatorFunction;
            escapeCodesEnabled = ansiEscapeCodesEnabled;
            this.readKeyFunction = readKeyFunction;
        }
        #endregion

        #region Override methods
        /// <inheritdoc cref="BaseUI.MakeSpecial(string, OptionsUI?)"/>
        protected override string MakeSpecial(string icons, OptionsUI? optionsUI = null)
        {
            return Value;
        }

        /// <inheritdoc cref="BaseUI.HandleActionProtected(UIKeyPressedEventArgs)"/>
        protected override object HandleActionProtected(UIKeyPressedEventArgs args)
        {
            if (!args.pressedKey.Equals(args.keybinds.ElementAt((int)Key.ENTER)))
            {
                return args.UpdateScreen ?? false;
            }

            var consoleProxy = args.optionsUI?.consoleProxy ?? new ConsoleProxy();
            if (args.optionsUI == null || !args.optionsUI.elements.Any(element => element == this))
            {
                consoleProxy.WriteLine(preText);
                Value = consoleProxy.ReadLine() ?? "";
                return args.UpdateScreen ?? true;
            }

            var xOffset = GetCurrentLineCharCountBeforeValue(args.optionsUI.cursorIcon);
            var yOffset = GetLineNumberAfterTextFieldValue(args.optionsUI);
            consoleProxy.MoveCursor(xOffset, yOffset);

            bool retry;
            do
            {
                retry = false;
                var newValue = ReadInput(consoleProxy, xOffset, args.optionsUI.cursorIcon);
                if (textValidatorFunction is null)
                {
                    Value = newValue;
                    continue;
                }

                var (status, message) = textValidatorFunction(newValue);
                if (message != null)
                {
                    consoleProxy.MoveCursor(-newValue.Length, 0);
                    consoleProxy.Write(Utils.ClearLineFromCursorPosString() + message);
                    GetKey(consoleProxy);
                    consoleProxy.MoveCursor(-message.Length, 0);
                    consoleProxy.Write(Utils.ClearLineFromCursorPosString() + newValue);
                    var (column, row) = consoleProxy.GetCursorPosition();
                    if (multiline)
                    {
                        consoleProxy.Write(postValue.Replace("\n", args.optionsUI.cursorIcon.sIconR + "\n" + args.optionsUI.cursorIcon.sIcon));
                    }
                    else
                    {
                        consoleProxy.Write(postValue);
                    }
                    consoleProxy.SetCursorPosition(column, row);
                }

                if (status == TextFieldValidatorStatus.VALID)
                {
                    Value = newValue;
                }
                else if (status == TextFieldValidatorStatus.RETRY)
                {
                    retry = true;
                    consoleProxy.MoveCursor(-newValue.Length, 0);
                }
            }
            while (retry);
            return args.UpdateScreen ?? true;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// The default read key function.
        /// </summary>
        private ConsoleKeyInfo GetKey(IConsoleProxy consoleProxy)
        {
            return readKeyFunction is not null
                ? readKeyFunction()
                : consoleProxy.ReadKey(false);
        }

        /// <summary>
        /// Gets the number of lines after the value that is in this object, in the display.
        /// </summary>
        /// <param name="optionsUI">The <c>OptionsUI</c>, that includes this object.</param>
        private int GetLineNumberAfterTextFieldValue(OptionsUI optionsUI)
        {
            var txt = new StringBuilder();

            // current object's line
            if (multiline)
            {
                txt.Append(postValue.Replace("\n", optionsUI.cursorIcon.sIconR + "\n" + optionsUI.cursorIcon.sIcon));
            }
            else
            {
                txt.Append(postValue);
            }
            txt.Append(optionsUI.cursorIcon.sIconR);

            // get displayed range
            int endIndex;
            if (optionsUI.scrollSettings.maxElements == -1 || optionsUI.scrollSettings.maxElements >= optionsUI.elements.Count)
            {
                endIndex = optionsUI.elements.Count;
            }
            else
            {
                endIndex = Math.Clamp(optionsUI.startIndex + optionsUI.scrollSettings.maxElements, 0, optionsUI.elements.Count);
            }

            // lines after current object
            for (var x = optionsUI.selected + 1; x < endIndex; x++)
            {
                var element = optionsUI.elements[x];
                if (element is not null)
                {
                    txt.Append(element.MakeText(
                        optionsUI.cursorIcon.icon,
                        optionsUI.cursorIcon.iconR,
                        optionsUI
                    ));
                }
                else if (element is null)
                {
                    txt.Append('\n');
                }
                else
                {
                    txt.Append(element.ToString() + "\n");
                }
            }
            txt.Append(endIndex == optionsUI.elements.Count ? optionsUI.scrollSettings.scrollIcon.bottomEndIndicator : optionsUI.scrollSettings.scrollIcon.bottomContinueIndicator);
            txt.Append('\n');

            return txt.ToString().Count(c => c == '\n') + 1;
        }

        /// <summary>
        /// Gets the number of characters in this object's display line string, before the value.
        /// </summary>
        /// <param name="cursorIcon">The <c>CursorIcon</c> passed into the <c>OptionsUI</c>, that includes this object.</param>
        private int GetCurrentLineCharCountBeforeValue(CursorIcon cursorIcon)
        {
            var lineText = new StringBuilder();
            lineText.Append(cursorIcon.sIcon);
            if (multiline)
            {
                lineText.Append(preText.Replace("\n", cursorIcon.sIconR + "\n" + cursorIcon.sIcon));
            }
            else
            {
                lineText.Append(preText);
            }
            var lastLine = lineText.ToString().Split("\n").Last();
            return lengthAsDisplayLength ? Utils.GetDisplayLen(lastLine, escapeCodesEnabled: escapeCodesEnabled) : lastLine.Length;
        }

        /// <summary>
        /// Reads user input, like <see cref="IConsoleProxy.ReadLine"/>, but puts the <see cref="PostValue"/> after the text, while typing.
        /// </summary>
        /// <param name="consoleProxy">The <see cref="IConsoleProxy"/>.</param>
        /// <param name="xOffset">The x offset from the left side of the console window, where the input should be placed.</param>
        /// <param name="cursorIcon">The <see cref="CursorIcon"/> passed into the <see cref="OptionsUI"/>, that includes this object.</param>
        private string ReadInput(IConsoleProxy consoleProxy, int xOffset, CursorIcon cursorIcon)
        {
            var fullPostValue = "";
            if (multiline)
            {
                fullPostValue += postValue.Replace("\n", cursorIcon.sIconR + "\n" + cursorIcon.sIcon);
            }
            else
            {
                fullPostValue += postValue;
            }
            fullPostValue += cursorIcon.sIconR;
            fullPostValue = fullPostValue.Split("\n").First();
            var newValue = new StringBuilder(oldValueAsStartingValue ? Value : "");
            var preValuePos = consoleProxy.GetCursorPosition();
            var cursorPos = newValue.Length;

            while (true)
            {
                var postLength = lengthAsDisplayLength ? Utils.GetDisplayLen(fullPostValue, xOffset + newValue.Length, escapeCodesEnabled) : fullPostValue.Length;
                var maxLength = maxInputLength ?? consoleProxy.ConsoleWidth - (xOffset + postLength);
                consoleProxy.WriteAtPosition(
                    Utils.ClearLineFromCursorPosString() + newValue.ToString(),
                    preValuePos.column,
                    preValuePos.row
                );
                var (column, row) = consoleProxy.GetCursorPosition();
                if (multiline)
                {
                    consoleProxy.Write(postValue.Replace("\n", cursorIcon.sIconR + "\n" + cursorIcon.sIcon));
                }
                else
                {
                    consoleProxy.Write(postValue);
                }
                var cursorPosOffset = newValue.Length - cursorPos;
                consoleProxy.SetCursorPosition(column - cursorPosOffset, row);

                var key = GetKey(consoleProxy);
                if (overrideDefaultKeyValidatorFunction && keyValidatorFunction is not null && !keyValidatorFunction(newValue, key, cursorPos))
                {
                    continue;
                }

                // done
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                // backspace
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (
                        newValue.Length > 0 &&
                        cursorPos > 0 &&
                        (overrideDefaultKeyValidatorFunction || keyValidatorFunction is null || keyValidatorFunction(newValue, null, cursorPos - 1))
                    )
                    {
                        newValue.Remove(cursorPos - 1, 1);
                        cursorPos--;
                    }
                }
                // delete
                else if (key.Key == ConsoleKey.Delete)
                {
                    if (
                        newValue.Length > 0 &&
                        cursorPos != newValue.Length &&
                        (overrideDefaultKeyValidatorFunction || keyValidatorFunction is null || keyValidatorFunction(newValue, null, cursorPos))
                    )
                    {
                        newValue.Remove(cursorPos, 1);
                    }
                }
                // cursor left
                else if (
                    key.KeyChar == '\0' &&
                    key.Key == ConsoleKey.LeftArrow
                )
                {
                    if (cursorPos > 0)
                    {
                        cursorPos--;
                    }
                }
                // cursor right
                else if (
                    key.KeyChar == '\0' &&
                    key.Key == ConsoleKey.RightArrow
                )
                {
                    if (cursorPos < newValue.Length)
                    {
                        cursorPos++;
                    }
                }
                // add char
                else if (
                    key.KeyChar != '\0' &&
                    key.Key != ConsoleKey.Escape &&
                    (
                        maxLength < 0 ||
                        (lengthAsDisplayLength ? Utils.GetDisplayLen(newValue.ToString() + key.KeyChar, xOffset, escapeCodesEnabled) : newValue.Length + 1) <= maxLength
                    ) &&
                    (overrideDefaultKeyValidatorFunction || keyValidatorFunction is null || keyValidatorFunction(newValue, key, cursorPos))
                )
                {
                    newValue.Insert(cursorPos, key.KeyChar);
                    cursorPos++;
                }

                consoleProxy.SetCursorPosition(column, row);
            }

            return newValue.ToString();
        }
        #endregion
    }
}
