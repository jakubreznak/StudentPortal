using System.Globalization;
using System.Text;

namespace API.CustomExtensions
{
    public static class StringExtension
    {
        public static string RemoveAccentsToLower(this string str)
        {
            string result = str.Replace(" ", string.Empty);

            var normalizedString = result.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
        }
    }
}