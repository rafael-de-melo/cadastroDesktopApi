using Cadastro.Api.Models.Entities;
using Cadastro.Api.Models.Requests;
using Cadastro.Api.Repositories.Interfaces;
using Cadastro.Api.Services.Interfaces;

namespace Cadastro.Api.Services
{
    public class PessoaService(IPessoaRepository repository) : IPessoaService
    {
        public Task<IEnumerable<Pessoa>> GetAllAsync(CancellationToken cancellation = default)
        {
            return repository.GetAllAsync(cancellation);
        }

        public Task<Pessoa?> GetByIdAsync(Guid id, CancellationToken cancellation = default)
        {
            return repository.GetByIdAsync(id, cancellation);
        }

        public async Task<Pessoa> PostAsync(PessoaPostRequest request, CancellationToken cancellation = default)
        {
            var entity = new Pessoa
            {
                Nome = request.Nome,
                Sobrenome = request.Sobrenome,
                Telefone = request.Telefone
            };

            return await repository.PostAsync(entity, cancellation);
        }

        public async Task UpdateAsync(PessoaPutRequest request, CancellationToken cancellation = default)
        {
            var existing = await repository.GetByIdAsync(request.Id, cancellation);
            if (existing is null) throw new KeyNotFoundException("Pessoa não encontrada");

            existing.Nome = request.Nome;
            existing.Sobrenome = request.Sobrenome;
            existing.Telefone = request.Telefone;

            await repository.UpdateAsync(existing, cancellation);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellation = default) =>
            repository.DeleteAsync(id, cancellation);

    }
}
