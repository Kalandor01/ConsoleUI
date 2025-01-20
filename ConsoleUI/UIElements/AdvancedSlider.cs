namespace ConsoleUI.UIElements
{
    /// <summary>
    /// A version of the <c>Slider</c>, that allways displays a customizable display name, for each value.<br/>
    /// Structure: [<c>preText</c>][<c>symbol</c> or <c>symbolEmpty</c>][<c>preValue</c>][<c>value</c>][<c>postValue</c>]
    /// </summary>
    public class AdvancedSlider : Slider
    {
        #region Private Fields
        /// <summary>
        /// The array of strings to display, for the actual values of the Slider.
        /// </summary>
        public IList<string> displayValues;
        #endregion

        #region Constructors
        /// <summary>
        /// <inheritdoc cref="AdvancedSlider"/>
        /// </summary>
        /// <param name="displayValues"><inheritdoc cref="displayValues" path="//summary"/></param>
        /// <param name="value"><inheritdoc cref="BaseUI.Value" path="//summary"/></param>
        /// <param name="preText"><inheritdoc cref="BaseUI.preText" path="//summary"/></param>
        /// <param name="symbol"><inheritdoc cref="Slider.symbol" path="//summary"/></param>
        /// <param name="symbolEmpty"><inheritdoc cref="Slider.symbolEmpty" path="//summary"/></param>
        /// <param name="preValue"><inheritdoc cref="BaseUI.preValue" path="//summary"/></param>
        /// <param name="postValue"><inheritdoc cref="BaseUI.postValue" path="//summary"/></param>
        /// <param name="multiline"><inheritdoc cref="BaseUI.multiline" path="//summary"/></param>
        /// <exception cref="ArgumentException">Thrown, if <c>displayValues</c> doesn't contain any values!</exception>
        public AdvancedSlider(IList<string> displayValues, int value = 0, string preText = "", string symbol = "#", string symbolEmpty = "-", string preValue = "", string postValue = "", bool multiline = false)
            : base(0, displayValues.Count - 1, 1, value, preText, symbol, symbolEmpty, preValue, true, postValue, multiline)
        {
            if (!displayValues.Any())
            {
                throw new ArgumentException($"{nameof(displayValues)} doesn't contain any values!");
            }
            this.displayValues = displayValues;

            Value = Math.Clamp(value, 0, displayValues.Count - 1);
        }
        #endregion

        #region Overrides
        /// <inheritdoc cref="BaseUI.MakeValue(OptionsUI?)"/>
        protected override string MakeValue(OptionsUI? optionsUI = null)
        {
            return displayValues[Value];
        }
        #endregion
    }
}
