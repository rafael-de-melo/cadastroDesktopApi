using Cadastro.Api.Data;
using Cadastro.Api.Models.Entities;
using Cadastro.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Api.Repositories
{
    public class PessoaRepository(AppDbContext context) : IPessoaRepository
    {
        public async Task<IEnumerable<Pessoa>> GetAllAsync(CancellationToken cancellation = default)
        {
            return await context.Pessoas.AsNoTracking().ToListAsync(cancellation);
        }

        public async Task<Pessoa?> GetByIdAsync(Guid id, CancellationToken cancellation = default)
        {
            return await context.Pessoas.FindAsync(new object[] { id }, cancellation);
        }

        public async Task<Pessoa> PostAsync(Pessoa pessoa, CancellationToken cancellation = default)
        {
            pessoa.Id = Guid.NewGuid();
            await context.Pessoas.AddAsync(pessoa, cancellation);
            await context.SaveChangesAsync(cancellation);
            return pessoa;
        }

        public async Task UpdateAsync(Pessoa pessoa, CancellationToken cancellation = default)
        {
            context.Pessoas.Update(pessoa);
            await context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var entity = await GetByIdAsync(id, cancellation);
            if (entity is null) return;
            context.Pessoas.Remove(entity);
            await context.SaveChangesAsync(cancellation);
        }
    }
}
