using System.Linq;

namespace MapsApiLibrary.Helpers.MethodExtensions
{
    internal static class StringToUpperFirstSymbol
    {
        public static string ToUpperFirstSymbol(this string input)
        {
            var firstChar = input.First();
            var upperFirstChar = firstChar.ToString().ToUpper();
            var result = upperFirstChar + input.Substring(1);
            return result;
        }
    }
}
