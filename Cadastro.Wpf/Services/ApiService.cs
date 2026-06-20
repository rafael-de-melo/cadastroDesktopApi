using Cadastro.Wpf.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Cadastro.Wpf.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5042/api/");
        }

        public async Task<List<Pessoa>> GetPessoasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Pessoa>>("Pessoa") ?? new List<Pessoa>();
        }

        public async Task<Pessoa?> GetPessoaAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Pessoa>($"Pessoa/{id}");
        }

        public async Task CreatePessoaAsync(Pessoa pessoa)
        {
            await _httpClient.PostAsJsonAsync("Pessoa", pessoa);
        }

        public async Task UpdatePessoaAsync(Pessoa pessoa)
        {
            await _httpClient.PutAsJsonAsync($"Pessoa/{pessoa.Id}", pessoa);
        }

        public async Task DeletePessoaAsync(Guid id)
        {
            await _httpClient.DeleteAsync($"Pessoa/{id}");
        }
    }
}
