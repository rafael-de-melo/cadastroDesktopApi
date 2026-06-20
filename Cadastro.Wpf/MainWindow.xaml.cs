using Cadastro.Wpf.Models;
using Cadastro.Wpf.ViewModels;
using Cadastro.Wpf.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
        try
        {
            await _viewModel.CarregarPessoas();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                ex.Message,
                "Erro ao carregar dados",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            Loaded -= MainWindow_Loaded;
        }
    }

    private async void OnNovoCadastro_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new PessoaDialog { Owner = this };
        if (dialog.ShowDialog() != true || dialog.ResultPessoa is null)
            return;

        try
        {
            await _viewModel.CreatePessoaAsync(dialog.ResultPessoa);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                ex.Message,
                "Erro",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async void OnEditar_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem { Tag: Pessoa pessoa })
            return;

        var dialog = new PessoaDialog(new Pessoa
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Sobrenome = pessoa.Sobrenome,
            Telefone = pessoa.Telefone
        });

        dialog.Owner = this;
        if (dialog.ShowDialog() != true || dialog.ResultPessoa is null)
            return;

        try
        {
            await _viewModel.UpdatePessoaAsync(dialog.ResultPessoa);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                ex.Message,
                "Erro",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async void OnExcluir_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem { Tag: Pessoa pessoa })
            return;

        var result = MessageBox.Show(
            this,
            $"Deseja excluir {pessoa.Nome} {pessoa.Sobrenome}?\n\nEsta ação não poderá ser desfeita.",
            "Confirmar exclusão",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        try
        {
            await _viewModel.DeletePessoaAsync(pessoa.Id);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                ex.Message,
                "Erro",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void OnMenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { ContextMenu: ContextMenu menu } button)
        {
            menu.PlacementTarget = button;
            menu.Placement = PlacementMode.Bottom;
            menu.IsOpen = true;
        }
    }
}