using Cadastro.Wpf.Helpers;
using Cadastro.Wpf.Models;
using System.Windows;

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
                TelefoneBox.Text = existing.TelefoneFormatado;
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
            var telefone = TextHelper.OnlyDigits(TelefoneBox.Text);
            
            if (string.IsNullOrWhiteSpace(NomeBox.Text))
            {
                MessageBox.Show(this, "Nome é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Information);
                NomeBox.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(telefone) && telefone.Length != 10 && telefone.Length != 11)
            {
                MessageBox.Show(this, "O telefone deve possuir 10 ou 11 dígitos.", "Validação", MessageBoxButton.OK, MessageBoxImage.Information);
                TelefoneBox.Focus();
                return;
            }

            ResultPessoa!.Nome = NomeBox.Text.Trim();
            ResultPessoa!.Sobrenome = SobrenomeBox.Text.Trim();
            ResultPessoa!.Telefone = telefone;

            DialogResult = true;
            Close();
        }
    }
}
