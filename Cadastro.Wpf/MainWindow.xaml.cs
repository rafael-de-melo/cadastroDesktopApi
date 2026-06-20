using Cadastro.Wpf.Models;
using Cadastro.Wpf.ViewModels;
using Cadastro.Wpf.Views;
using System.Windows;

namespace Cadastro.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel = new();

    public MainWindow()
    {
        InitializeComponent();

        DataContext = _viewModel;
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        await _viewModel.CarregarPessoas();
    }

    private async void OnNovoCadastro_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new PessoaDialog();
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.ResultPessoa is not null)
        {
            var pessoa = dialog.ResultPessoa;
            // novo id local caso API não gere um (ajuste se API retorna id)
            if (pessoa.Id == Guid.Empty) pessoa.Id = Guid.NewGuid();
            await _viewModel.CreatePessoaAsync(pessoa);
        }
    }

    private async void OnEditar_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.MenuItem menu && menu.Tag is Pessoa p)
        {
            var copy = new Pessoa { Id = p.Id, Nome = p.Nome, Sobrenome = p.Sobrenome, Telefone = p.Telefone };
            var dialog = new PessoaDialog(copy);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true && dialog.ResultPessoa is not null)
            {
                await _viewModel.UpdatePessoaAsync(dialog.ResultPessoa);
            }
        }
    }

    private async void OnExcluir_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.MenuItem menu && menu.Tag is Pessoa p)
        {
            var res = MessageBox.Show(this, "Deseja realmente excluir este cadastro?", "Confirmar exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                await _viewModel.DeletePessoaAsync(p.Id);
            }
        }
    }
}