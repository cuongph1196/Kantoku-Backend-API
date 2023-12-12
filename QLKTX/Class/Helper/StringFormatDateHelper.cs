using System.Globalization;

namespace QLKTX.Class.Helper
{
    public class StringFormatDateHelper
    {
        public static string ConvertDate(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return null;

                string[] format = { "dd/MM/yyyy", "yyyy/MM/dd", "MM/dd/yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "MM-dd-yyyy", "yyyyMMdd" };
                DateTime date;
                if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    date = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
                }
                else
                {
                    date = Convert.ToDateTime(input);
                }
                return date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
