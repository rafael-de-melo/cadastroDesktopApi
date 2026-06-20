using Cadastro.Wpf.Helpers;

namespace Cadastro.Wpf.Models
{
    public class Pessoa
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = "";

        public string Sobrenome { get; set; } = "";

        public string Telefone { get; set; } = "";

        public string TelefoneFormatado
        {
            get => FormatTelefone(Telefone);
        }

        private static string FormatTelefone(string? raw)
        {
            var digits = TextHelper.OnlyDigits(raw);
            if (string.IsNullOrEmpty(digits)) return string.Empty;

            if (digits.Length == 11)
                return $"({digits[..2]}) {digits[2..7]}-{digits[7..]}";

            if (digits.Length == 10)
                return $"({digits[..2]}) {digits[2..6]}-{digits[6..]}";

            return digits;
        }
    }
}
