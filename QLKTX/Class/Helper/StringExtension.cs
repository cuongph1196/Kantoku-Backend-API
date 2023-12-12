using System.Text.RegularExpressions;
using System.Text;

namespace QLKTX.Class.Helper
{
    public class StringExtension
    {
        public static string ToUrlSlug(string value)
        {

            // First to lower case 
            value = value.ToLowerInvariant();

            // Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            // Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            // Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            // Trim dashes from end 
            value = value.Trim('-', '_');

            // Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        public static string ConvertToUnSign(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");

            string temp = value.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            temp = regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            // Replace spaces 
            temp = Regex.Replace(temp, @"\s", "-", RegexOptions.Compiled);

            // Remove invalid chars 
            temp = Regex.Replace(temp, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            // Trim dashes from end 
            temp = temp.Trim('-', '_');

            // Replace double occurences of - or \_ 
            temp = Regex.Replace(temp, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return temp;
        }

        public static string ConvertToUnSignName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");

            string temp = value.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            temp = regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            // Replace spaces 
            temp = Regex.Replace(temp, @"\s", " ", RegexOptions.Compiled);

            // Remove invalid chars 
            temp = Regex.Replace(temp, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            // Trim dashes from end 
            temp = temp.Trim('-', '_');

            // Replace double occurences of - or \_ 
            temp = Regex.Replace(temp, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return temp;
        }

        public static string ConvertToSlugName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");

            string temp = value.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            temp = regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            // Replace spaces 
            temp = Regex.Replace(temp, @"\s", "-", RegexOptions.Compiled);

            // Remove invalid chars 
            temp = Regex.Replace(temp, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            // Trim dashes from end 
            temp = temp.Trim('-', '_');

            // Replace double occurences of - or \_ 
            temp = Regex.Replace(temp, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return temp;
        }

        public static string convertToUnSign3(string inputText)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = inputText.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}
