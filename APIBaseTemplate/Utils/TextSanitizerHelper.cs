using APIBaseTemplate.Common;

namespace APIBaseTemplate.Utils
{
    /// <summary>
    /// A little helper to perform text sanitization
    /// </summary>
    public static class TextSanitizerHelper
    {
        public static EnmSimpleTextFilterSanitize GetSanitizationOptionsRemovePercentAndTrim() =>
            EnmSimpleTextFilterSanitize.RemovePercent | EnmSimpleTextFilterSanitize.Trim;

        public static EnmSimpleTextFilterSanitize GetSanitizationOptionsRemovePercentUppercaseTrim() =>
            EnmSimpleTextFilterSanitize.RemovePercent | EnmSimpleTextFilterSanitize.Trim | EnmSimpleTextFilterSanitize.ToUpper;

        public static EnmSimpleTextFilterSanitize GetSanitizationOptionsRemovePercent() =>
            EnmSimpleTextFilterSanitize.RemovePercent;
        public static EnmSimpleTextFilterSanitize GetSanitizationOptionsRemoveUppercaseTrim() =>
            EnmSimpleTextFilterSanitize.Trim | EnmSimpleTextFilterSanitize.ToUpper;

        /// <summary>
        /// Returns a simply sanitized copy of the <paramref name="textToSanitize"/> argument
        /// </summary>
        /// <remarks>Original <paramref name="textToSanitize"/> is not changhed </remarks>
        /// <param name="textToSanitize"></param>
        /// <param name="sanitizingOptions">sanitize options</param>
        /// <returns>a simply sanitized copy of the <paramref name="textToSanitize"/> property</returns>
        public static string? SanitizeTextSimply(string? textToSanitize, EnmSimpleTextFilterSanitize sanitizingOptions)
        {
            string? text = textToSanitize;
            if (string.IsNullOrEmpty(text))
                return text; // nothing to be sanitized

            if (sanitizingOptions.HasFlag(EnmSimpleTextFilterSanitize.RemovePercent)) text = text.Replace("%", string.Empty);
            if (sanitizingOptions.HasFlag(EnmSimpleTextFilterSanitize.Trim)) text = text.Trim();
            if (sanitizingOptions.HasFlag(EnmSimpleTextFilterSanitize.ToUpper)) text = text.ToUpper();
            if (sanitizingOptions.HasFlag(EnmSimpleTextFilterSanitize.ToLower)) text = text.ToLower();
            return text;
        }
    }
}
