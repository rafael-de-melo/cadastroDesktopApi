namespace Cadastro.Wpf.Helpers
{
    public static class TextHelper
    {
        public static string OnlyDigits(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            return new(value.Where(char.IsDigit).ToArray());
        }
    }
}
