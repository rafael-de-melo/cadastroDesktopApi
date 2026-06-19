using System.ComponentModel.DataAnnotations;

namespace Cadastro.Api.Models.Requests
{
    public class PessoaPutRequest
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required, MaxLength(25)]
        public string Nome { get; set; } = null!;

        [MaxLength(100)]
        public string? Sobrenome { get; set; }

        [Required, MaxLength(11)]
        public string Telefone { get; set; } = null!;
    }
}
