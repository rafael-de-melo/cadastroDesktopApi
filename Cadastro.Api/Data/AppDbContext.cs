using Cadastro.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    }
}
