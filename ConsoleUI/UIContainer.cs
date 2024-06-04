using System.Text;

namespace ConsoleUI
{
    /// <summary>
    /// W.I.P. Class used to display some text in a way, where it's contained in a apecific rectangle on the console.
    /// </summary>
    public class UIContainer
    {
        #region Private fields
        /// <summary>
        /// If the value of Text was calculated yet for the current value of RawText.
        /// </summary>
        private bool _textCalculated;
        /// <summary>
        /// The origin of where to display the text. If null, when the text is printed, it will be blaced where the cursor currently is.
        /// </summary>
        private (int x, int y)? _origin;
        /// <summary>
        /// The maximum size of the rectangle. If x or y is null, it's not restricted in that direction.
        /// </summary>
        private (int? x, int? y) _maxSize;
        /// <summary>
        /// Wether to replace the last 3 characters of the text with "..." if it's too long.
        /// </summary>
        private bool _truncateText;
        /// <summary>
        /// Whether to try to break up the text into lines only at spaces and tabs.
        /// </summary>
        private bool _linebreakOnlyAtWhitespace;
        /// <summary>
        /// Whether to always overwrite the text that was previously there.
        /// </summary>
        private bool _overwritePreviousText;
        /// <summary>
        /// Whether to use the cursor pos save/restor ANSI commands to move the cursor back after the container has been displayed, or just set the cursor position directly.
        /// </summary>
        private bool _useCursorPositionSaving;
        /// <summary>
        /// The raw text value to turn into the text to display.
        /// </summary>
        private string _rawText;
        /// <summary>
        /// The text to display.
        /// </summary>
        private string _text;
        #endregion

        #region Public properties
        /// <summary>
        /// <inheritdoc cref="_textCalculated" path="//summary"/>
        /// </summary>
        public bool TextCalculated
        {
            get => _textCalculated;
            private set => _textCalculated = value;
        }

        /// <summary>
        /// <inheritdoc cref="_truncateText" path="//summary"/>
        /// </summary>
        public bool TruncateText
        {
            get => _truncateText;
            set
            {
                if (_truncateText != value)
                {
                    TextCalculated = false;
                    _truncateText = value;
                }
            }
        }

        /// <summary>
        /// <inheritdoc cref="_origin" path="//summary"/>
        /// </summary>
        public (int x, int y)? Origin
        {
            get => _origin;
            set
            {
                if (_origin != value)
                {
                    if (_origin?.x != value?.x)
                    {
                        TextCalculated = false;
                    }
                    _origin = value;
                }
            }
        }

        /// <summary>
        /// <inheritdoc cref="_maxSize" path="//summary"/>
        /// </summary>
        public (int? x, int? y) MaxSize
        {
            get => _maxSize;
            set
            {
                if (_maxSize != value)
                {
                    TextCalculated = false;
                    _maxSize = value;
                }
            }
        }

        /// <summary>
        /// <inheritdoc cref="_linebreakOnlyAtWhitespace" path="//summary"/>
        /// </summary>
        public bool LinebreakOnlyAtWhitespace
        {
            get => _linebreakOnlyAtWhitespace;
            set
            {
                if (_linebreakOnlyAtWhitespace != value)
                {
                    TextCalculated = false;
                    _linebreakOnlyAtWhitespace = value;
                }
            }
        }

        /// <summary>
        /// <inheritdoc cref="_overwritePreviousText" path="//summary"/>
        /// </summary>
        public bool OverwritePreviousText
        {
            get => _overwritePreviousText;
            set
            {
                if (_overwritePreviousText != value)
                {
                    TextCalculated = false;
                    _overwritePreviousText = value;
                }
            }
        }

        /// <summary>
        /// <inheritdoc cref="_useCursorPositionSaving" path="//summary"/>
        /// </summary>
        public bool UseCursorPositionSaving
        {
            get => _useCursorPositionSaving;
            set => _useCursorPositionSaving = value;
        }

        /// <summary>
        /// <inheritdoc cref="_rawText" path="//summary"/>
        /// </summary>
        public string RawText
        {
            get => _rawText;
            set
            {
                if (_rawText != value)
                {
                    TextCalculated = false;
                    _rawText = value;
                }
            }
        }

        /// <summary>
        /// <inheritdoc cref="_text" path="//summary"/>
        /// </summary>
        public string Text
        {
            get
            {
                var text = new StringBuilder();
                var originalCursorPosX = 0;
                var originalCursorPosY = 0;
                if (UseCursorPositionSaving)
                {
                    text.Append(Utils.SaveCursorPosString());
                }
                else
                {
                    var (x, y) = Console.GetCursorPosition();
                    originalCursorPosX = x;
                    originalCursorPosY = y;
                }

                if (Origin is not null)
                {
                    text.Append(Utils.SetCursorPosString(Origin.Value.x, Origin.Value.y));
                }

                if (!_textCalculated)
                {
                    var (x, y) = Console.GetCursorPosition();
                    RecalculateText(x, y);
                }

                text.Append(_text);
                if (UseCursorPositionSaving)
                {
                    text.Append(Utils.RestoreCursorPosString());
                }
                else
                {
                    text.Append(Utils.SetCursorPosString(originalCursorPosX, originalCursorPosY));
                }

                return text.ToString();
            }
            private set => _text = value;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="UIContainer"/>
        /// </summary>
        /// <param name="text"><inheritdoc cref="Text" path="//summary"/></param>
        /// <param name="origin"><inheritdoc cref="Origin" path="//summary"/></param>
        /// <param name="maxSize"><inheritdoc cref="MaxSize" path="//summary"/></param>
        /// <param name="truncateText"><inheritdoc cref="TruncateText" path="//summary"/></param>
        /// <param name="linebreakOnlyAtWhitespace"><inheritdoc cref="LinebreakOnlyAtWhitespace" path="//summary"/></param>
        /// <param name="overwritePreviousText"><inheritdoc cref="OverwritePreviousText" path="//summary"/></param>
        /// <param name="useCursorPositionSaving"><inheritdoc cref="UseCursorPositionSaving" path="//summary"/></param>
        public UIContainer(
            string text,
            (int x, int y)? origin = null,
            (int? x, int? y)? maxSize = null,
            bool truncateText = true,
            bool linebreakOnlyAtWhitespace = false,
            bool overwritePreviousText = true,
            bool useCursorPositionSaving = true
        )
        {
            TextCalculated = false;
            RawText = text;
            Origin = origin;
            MaxSize = maxSize ?? (null, null);
            TruncateText = truncateText;
            LinebreakOnlyAtWhitespace = linebreakOnlyAtWhitespace;
            OverwritePreviousText = overwritePreviousText;
            UseCursorPositionSaving = useCursorPositionSaving;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Recalculates the value of the text to be displayed.
        /// </summary>
        public void RecalculateText()
        {
            var (x, y) = Console.GetCursorPosition();
            RecalculateText(x, y);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Recalculates the value of the text to be displayed.
        /// </summary>
        /// <param name="originalX">The original X position of the cursor.</param>
        /// <param name="originalY">The original Y position of the cursor.</param>
        private void RecalculateText(int originalX, int originalY)
        {
            TextCalculated = true;

            var (maxWidth, maxHeight) = MaxSize;
            if (maxHeight < 1 || maxWidth < 1)
            {
                Text = "";
                return;
            }

            if (!RawText.Contains('\n') && (maxWidth is null || maxWidth >= Utils.GetDisplayLen(RawText, Origin?.x ?? originalX)))
            {
                Text = RawText;
                return;
            }

            var overrideLineText = "";
            if (OverwritePreviousText)
            {
                var overrideSize = MaxSize.x ?? RawText.Split('\n').Max(line => Utils.GetDisplayLen(line, Origin?.x ?? originalX));
                overrideLineText = new string(' ', overrideSize);
            }

            var text = new StringBuilder();
            var textLeft = RawText;
            var currentLineNumber = 0;
            while (maxHeight is null || currentLineNumber < maxHeight)
            {
                if (textLeft == "" && (maxHeight is null || !OverwritePreviousText))
                {
                    break;
                }

                if (OverwritePreviousText)
                {
                    text.Append(overrideLineText);
                    text.Append(Utils.MoveCursorString(-overrideLineText.Length, 0));
                }
                var nextLineBreak = textLeft.IndexOf('\n');
                var lineEnd = nextLineBreak;
                if (MaxSize.x is not null)
                {
                    var maxX = (int)MaxSize.x;
                    if (lineEnd == -1 || lineEnd > maxX)
                    {
                        lineEnd = maxX;
                    }
                    if (LinebreakOnlyAtWhitespace)
                    {
                        lineEnd = GetCutoffIndex(textLeft, lineEnd);
                    }
                }
                var currentLine = lineEnd == -1 ? textLeft : textLeft[..lineEnd];
                var textLeftStart = lineEnd;
                textLeftStart += lineEnd != -1 && lineEnd == nextLineBreak ? 1 : 0;
                textLeft = textLeftStart == -1 ? "" : (textLeftStart == textLeft.Length - 1 ? "" : textLeft[(textLeftStart + 1)..]);

                text.Append(currentLine);
                currentLineNumber++;
                if (maxHeight is not null && currentLineNumber < maxHeight)
                {
                    text.Append(Utils.MoveCursorString(-Utils.GetDisplayLen(currentLine), -1));
                }
            }

            Text = text.ToString();
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Returns the maximum line cutoff at or before the max index.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxIndex"></param>
        private static int GetCutoffIndex(string text, int maxIndex)
        {
            if (text.Length - 1 <= maxIndex)
            {
                return text.Length - 1;
            }

            var index = maxIndex + 1;
            for (; index > 0; index--)
            {
                var ch = text[index];
                if (ch == '\t' || ch == ' ')
                {
                    break;
                }
            }
            return index;
        }
        #endregion
    }
}
