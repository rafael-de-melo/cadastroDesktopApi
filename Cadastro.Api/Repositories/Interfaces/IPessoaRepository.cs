using Cadastro.Api.Models.Entities;

namespace Cadastro.Api.Repositories.Interfaces
{
    public interface IPessoaRepository
    {
        Task<IEnumerable<Pessoa>> GetAllAsync(CancellationToken cancellation = default);
        Task<Pessoa?> GetByIdAsync(Guid Id, CancellationToken cancellation = default);
        Task<Pessoa> PostAsync(Pessoa pessoa, CancellationToken cancellation = default);
        Task UpdateAsync(Pessoa pessoa, CancellationToken cancellation = default);
        Task DeleteAsync(Guid Id, CancellationToken cancellation = default);
    }
}
