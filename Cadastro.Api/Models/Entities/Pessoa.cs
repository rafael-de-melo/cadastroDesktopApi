using System.ComponentModel.DataAnnotations;

namespace Cadastro.Api.Models.Entities
{
    public class Pessoa
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(25)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Sobrenome { get; set; } = null;

        [Required, MaxLength(11)]
        public string Telefone { get; set; } = string.Empty;
    }
}
