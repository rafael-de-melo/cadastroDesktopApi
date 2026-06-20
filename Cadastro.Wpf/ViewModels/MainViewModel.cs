using Cadastro.Wpf.Helpers;
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
        public bool HasItems => Pessoas.Count > 0;

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
                    Pessoas.Add(pessoa);

                OnPropertyChanged(nameof(HasItems));
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<Pessoa> CreatePessoaAsync(Pessoa pessoa)
        {
            if (pessoa.Id == Guid.Empty)
                pessoa.Id = Guid.NewGuid();

            await _apiService.CreatePessoaAsync(pessoa);
            Pessoas.Insert(0, pessoa);
            OnPropertyChanged(nameof(HasItems));
            return pessoa;
        }

        public async Task UpdatePessoaAsync(Pessoa pessoa)
        {
            await _apiService.UpdatePessoaAsync(pessoa);

            var existente = Pessoas.FirstOrDefault(x => x.Id == pessoa.Id);

            if (existente == null)
                return;

            void Update()
            {
                existente.Nome = pessoa.Nome;
                existente.Sobrenome = pessoa.Sobrenome;
                existente.Telefone = pessoa.Telefone;

                PessoasView.Refresh();
            }

            if (System.Windows.Application.Current.Dispatcher.CheckAccess())
                Update();
            else
                System.Windows.Application.Current.Dispatcher.Invoke(Update);
        }

        public async Task DeletePessoaAsync(Guid id)
        {
            await _apiService.DeletePessoaAsync(id);
            var pessoa = Pessoas.FirstOrDefault(x => x.Id == id);
            if (pessoa != null)
                Pessoas.Remove(pessoa);

            OnPropertyChanged(nameof(HasItems));
        }

        private bool FilterPessoa(object? obj)
        {
            if (obj is not Pessoa p) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            var filtro = SearchText.Trim();
            var nomeCompleto = $"{p.Nome} {p.Sobrenome}";
            var telefonePesquisa= TextHelper.OnlyDigits(filtro);
            
            return
                p.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                || p.Sobrenome.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                || nomeCompleto.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                || (!string.IsNullOrEmpty(telefonePesquisa) && p.Telefone.Contains(telefonePesquisa));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
