namespace SaveFileManager
{
    public class KeyAction
    {
        public readonly object response;
        public readonly IEnumerable<ConsoleKeyInfo> keys;
        public readonly IEnumerable<GetKeyMode> ignoreModes;


        /// <param name="key">The key that can be pressed to trigger this action.</param>
        /// <inheritdoc cref="KeyAction(object, IEnumerable{ConsoleKeyInfo}, GetKeyMode)"/>
        public KeyAction(object response, ConsoleKeyInfo key, GetKeyMode ignoreMode):
            this(response, new List<ConsoleKeyInfo> { key }, ignoreMode) { }

        /// <param name="ignoreMode">If the current <c>GetKeyMode</c> is the <c>ignoreMode</c> when calling <c>GetKey</c>, the keypress will be ignored.</param>
        /// <inheritdoc cref="KeyAction(object, IEnumerable{ConsoleKeyInfo}, IEnumerable{GetKeyMode})"/>
        public KeyAction(object response, IEnumerable<ConsoleKeyInfo> keys, GetKeyMode ignoreMode):
            this(response, keys, new List<GetKeyMode> { ignoreMode }) { }

        /// <summary>
        /// Object for <c>keybinds</c> for the <c>GetKey</c> function.<br/>
        /// </summary>
        /// <param name="response">The value that will be returned by the <c>GetKey</c> function, if one of the keys associated with this action is pressed.</param>
        /// <param name="keys">The keys that can be pressed to trigger this action.</param>
        /// <param name="ignoreModes">If the current <c>GetKeyMode</c> is in the <c>ignoreModes</c> list when calling <c>GetKey</c>, the keypress will be ignored.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public KeyAction(object response, IEnumerable<ConsoleKeyInfo> keys, IEnumerable<GetKeyMode> ignoreModes)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));
            this.keys = keys ?? throw new ArgumentNullException(nameof(keys));
            this.ignoreModes = ignoreModes ?? throw new ArgumentNullException(nameof(ignoreModes));
        }
    }
}
