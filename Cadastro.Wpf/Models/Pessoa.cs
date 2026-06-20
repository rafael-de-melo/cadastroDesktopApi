namespace Cadastro.Wpf.Models
{
    public class Pessoa
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = "";

        public string Sobrenome { get; set; } = "";

        public string Telefone { get; set; } = "";
    }
}
