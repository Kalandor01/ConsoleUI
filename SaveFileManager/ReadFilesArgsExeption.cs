namespace SaveFileManager
{
    /// <summary>
    /// This exeption is raised if "fileName" and "seed" are both null in ReadFiles
    /// </summary>
    public class ReadFilesArgsExeption : Exception
    {
        /// <summary>
        /// This exeption is raised if "fileName" and "seed" are both null or fileName doesn't contain the special character ("*") in ReadFiles.
        /// </summary>
        public ReadFilesArgsExeption()
            : this($"\"fileName\" and \"seed\" can\'t both be null at the same time, and \"fileName\" must contain at least one \"*\"") { }

        /// <summary>
        /// This exeption is raised if "fileName" and "seed" are both null or fileName doesn't contain the special character ("*") in ReadFiles.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public ReadFilesArgsExeption(string message) : base(message) { }
    }
}
