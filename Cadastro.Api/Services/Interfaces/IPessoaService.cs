using Cadastro.Api.Models.Entities;
using Cadastro.Api.Models.Requests;

namespace Cadastro.Api.Services.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<Pessoa>> GetAllAsync(CancellationToken cancellation = default);
        Task<Pessoa?> GetByIdAsync(Guid id, CancellationToken cancellation = default);
        Task<Pessoa> PostAsync(PessoaPostRequest request, CancellationToken cancellation = default);
        Task UpdateAsync(PessoaPutRequest request, CancellationToken cancellation = default);
        Task DeleteAsync(Guid id, CancellationToken cancellation = default);
    }
}
