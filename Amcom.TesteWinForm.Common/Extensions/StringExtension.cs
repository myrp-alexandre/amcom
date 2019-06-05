namespace Amcom.TesteWinForm.Common.Extension
{
    public static class StringExtension
    {
        public static int ToInt(this string value)
        {
            var output = 0;
            int.TryParse(value, out output);
            return output;
        }

        public static decimal ToDecimal(this string value)
        {
            decimal output = 0;
            decimal.TryParse(value.Replace(',', '.'), out output);
            return output / 100;
        }
    }
}