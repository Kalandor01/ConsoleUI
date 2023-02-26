namespace SaveFileManager
{
    /// <summary>
    /// Modes for the <c>GetKey</c> function.
    /// </summary>
    public enum GetKeyMode
    {
        NO_IGNORE,
        IGNORE_HORIZONTAL,
        IGNORE_VERTICAL,
        IGNORE_ESCAPE,
        IGNORE_ENTER,
        IGNORE_ARROWS,
        ONLY_ARROWS,
    }
}
