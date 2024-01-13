using System.Text.RegularExpressions;

namespace WebApi.VehiclesAuction.Domain.Util
{
    public static class StringExtensions
    {
        public static string OnlyDigits(this string input)
        {
            string pattern = @"\D";
            return Regex.Replace(input, pattern, "");
        }
    }
}
