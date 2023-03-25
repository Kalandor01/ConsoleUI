namespace SaveFileManager
{
    /// <summary>
    /// This exeption is raised if a delegate's return type is not what is should be.
    /// </summary>
    public class WrongReturnTypeExeption : Exception
    {
        /// <inheritdoc cref="WrongReturnTypeExeption(string)"/>
        public WrongReturnTypeExeption()
            : this("Delegate's return type in wrong.") { }

        /// <summary>
        /// This exeption is raised if a delegate's return type is not what is should be.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public WrongReturnTypeExeption(string message) : base(message) { }
    }
}
