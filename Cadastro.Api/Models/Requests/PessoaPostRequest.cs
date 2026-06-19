using System.ComponentModel.DataAnnotations;

namespace Cadastro.Api.Models.Requests
{
    public class PessoaPostRequest
    {
        [Required, MaxLength(25)]
        public string Nome { get; set; } = null!;

        [MaxLength(100)]
        public string? Sobrenome { get; set; }

        [Required, MaxLength(11)]
        public string Telefone { get; set; } = null!;
    }
}
