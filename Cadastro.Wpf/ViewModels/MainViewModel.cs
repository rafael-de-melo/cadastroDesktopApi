using Cadastro.Wpf.Models;
using Cadastro.Wpf.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Cadastro.Wpf.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new();
        private string _searchText = string.Empty;
        private bool _isLoading;

        public ObservableCollection<Pessoa> Pessoas { get; } = new ObservableCollection<Pessoa>();
        public ICollectionView PessoasView { get; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                PessoasView.Refresh();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        public MainViewModel()
        {
            PessoasView = CollectionViewSource.GetDefaultView(Pessoas);
            PessoasView.Filter = FilterPessoa;
        }

        public async Task CarregarPessoas(CancellationToken cancellation = default)
        {
            try
            {
                IsLoading = true;
                Pessoas.Clear();
                var lista = await _apiService.GetPessoasAsync();
                foreach (var pessoa in lista)
                {
                    Pessoas.Add(pessoa);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<Pessoa> CreatePessoaAsync(Pessoa pessoa)
        {
            await _apiService.CreatePessoaAsync(pessoa);
            Pessoas.Insert(0, pessoa);
            return pessoa;
        }

        public async Task UpdatePessoaAsync(Pessoa pessoa)
        {
            await _apiService.UpdatePessoaAsync(pessoa).ConfigureAwait(false);
            var idx = Pessoas.ToList().FindIndex(p => p.Id == pessoa.Id);
            if (idx >= 0)
            {
                Pessoas[idx].Nome = pessoa.Nome;
                Pessoas[idx].Sobrenome = pessoa.Sobrenome;
                Pessoas[idx].Telefone = pessoa.Telefone;
                PessoasView.Refresh();
            }
        }

        public async Task DeletePessoaAsync(Guid id)
        {
            await _apiService.DeletePessoaAsync(id);
            var existente = System.Linq.Enumerable.FirstOrDefault(Pessoas, p => p.Id == id);
            if (existente != null) Pessoas.Remove(existente);
        }

        private bool FilterPessoa(object? obj)
        {
            if (obj is not Pessoa p) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            return p.Nome.Contains(SearchText, StringComparison.OrdinalIgnoreCase) 
                || p.Sobrenome.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || p.Telefone.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
