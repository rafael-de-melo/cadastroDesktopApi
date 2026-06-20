using Cadastro.Wpf.Models;
using System.Windows;
using System.Windows.Controls;

namespace Cadastro.Wpf.Views
{
    public partial class PessoaDialog : Window
    {
        public Pessoa? ResultPessoa { get; private set; }

        public PessoaDialog() : this(null) { }

        public PessoaDialog(Pessoa? existing)
        {
            InitializeComponent();

            if (existing is not null)
            {
                NomeBox.Text = existing.Nome;
                SobrenomeBox.Text = existing.Sobrenome;
                TelefoneBox.Text = existing.Telefone;
                ResultPessoa = new Pessoa { Id = existing.Id };
            }
            else
            {
                ResultPessoa = new Pessoa { Id = Guid.Empty };
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            // validação simples
            if (string.IsNullOrWhiteSpace(NomeBox.Text))
            {
                MessageBox.Show(this, "Nome é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Information);
                NomeBox.Focus();
                return;
            }

            ResultPessoa!.Nome = NomeBox.Text.Trim();
            ResultPessoa!.Sobrenome = SobrenomeBox.Text.Trim();
            ResultPessoa!.Telefone = TelefoneBox.Text.Trim();

            DialogResult = true;
            Close();
        }
    }
}
