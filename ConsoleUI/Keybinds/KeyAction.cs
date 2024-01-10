namespace ConsoleUI.Keybinds
{
    /// <summary>
    /// Object for <c>keybinds</c> for the <c>GetKey</c> function.
    /// </summary>
    public class KeyAction
    {
        #region Public fields
        /// <summary>
        /// The value that will be returned by the <c>GetKey</c> function, if one of the keys associated with this action is pressed.
        /// </summary>
        public readonly object response;
        /// <summary>
        /// If the current <c>GetKeyMode</c> is in this list when calling <c>GetKey</c>, the keypress will be ignored.
        /// </summary>
        public readonly IEnumerable<GetKeyMode> ignoreModes;
        #endregion

        #region Private fields
        /// <summary>
        /// The keys that can be pressed to trigger this action.
        /// </summary>
        private IEnumerable<ConsoleKeyInfo> keys;
        #endregion

        #region Public properties
        /// <summary>
        /// The keys that can be pressed to trigger this action.
        /// </summary>
        public IEnumerable<ConsoleKeyInfo> Keys
        {
            get { return keys; }
            protected set
            {
                keys = value;
            }
        }
        #endregion

        #region Constructors
        ///<param name="ignoreMode">If the current <c>GetKeyMode</c> is the <c>ignoreMode</c> when calling <c>GetKey</c>, the keypress will be ignored.</param>
        /// <inheritdoc cref="KeyAction(object, ConsoleKeyInfo, IEnumerable{GetKeyMode})"/>
        public KeyAction(object response, ConsoleKeyInfo key, GetKeyMode ignoreMode) :
            this(response, key, new List<GetKeyMode> { ignoreMode })
        { }

        /// <param name="key">The key that can be pressed to trigger this action.</param>
        /// <inheritdoc cref="KeyAction(object, IEnumerable{ConsoleKeyInfo}, IEnumerable{GetKeyMode})"/>
        public KeyAction(object response, ConsoleKeyInfo key, IEnumerable<GetKeyMode> ignoreModes) :
            this(response, new List<ConsoleKeyInfo> { key }, ignoreModes)
        { }

        /// <summary>
        /// <inheritdoc cref="KeyAction"/>
        /// </summary>
        /// <param name="response"><inheritdoc cref="response" path="//summary"/></param>
        /// <param name="keys"><inheritdoc cref="keys" path="//summary"/></param>
        /// <param name="ignoreModes"><inheritdoc cref="ignoreModes" path="//summary"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public KeyAction(object response, IEnumerable<ConsoleKeyInfo> keys, IEnumerable<GetKeyMode> ignoreModes)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));
            this.keys = keys ?? throw new ArgumentNullException(nameof(keys));
            this.ignoreModes = ignoreModes ?? throw new ArgumentNullException(nameof(ignoreModes));
        }
        #endregion
    }
}
