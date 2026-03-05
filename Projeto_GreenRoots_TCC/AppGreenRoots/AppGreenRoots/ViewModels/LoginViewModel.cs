using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AppGreenRoots.Commands;
using AppGreenRoots.Data; // ← adicionar esse using

namespace AppGreenRoots.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private string _email    = string.Empty;
    private string _senha    = string.Empty;
    private string _nome     = string.Empty;
    private string _mensagem = string.Empty;
    private bool   _isCadastro;

    public string Email    { get => _email;    set { _email    = value; OnPropertyChanged(); } }
    public string Senha    { get => _senha;    set { _senha    = value; OnPropertyChanged(); } }
    public string Nome     { get => _nome;     set { _nome     = value; OnPropertyChanged(); } }
    public string Mensagem { get => _mensagem; set { _mensagem = value; OnPropertyChanged(); } }

    public bool IsCadastro
    {
        get => _isCadastro;
        set { _isCadastro = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsLogin)); Mensagem = ""; }
    }
    public bool IsLogin => !_isCadastro;

    public ICommand LoginCommand        { get; }
    public ICommand CadastrarCommand    { get; }
    public ICommand AlternarModoCommand { get; }

    public LoginViewModel()
    {
        LoginCommand        = new RelayCommand(_ => ExecutarLogin());
        CadastrarCommand    = new RelayCommand(_ => ExecutarCadastro());
        AlternarModoCommand = new RelayCommand(_ => IsCadastro = !IsCadastro);
    }

    // ✅ IMPLEMENTADO
    private void ExecutarLogin()
    {
        Mensagem = string.Empty;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
        {
            Mensagem = "Preencha o e-mail e a senha.";
            return;
        }

        var usuario = DatabaseHelper.AutenticarUsuario(Email, Senha);

        if (usuario != null)
        {
            var dashboard = new Views.DashboardView();
            dashboard.DataContext = new DashboardViewModel(usuario);
            dashboard.Show();

            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
                if (w is Views.LoginView) { w.Close(); break; }
        }
        else
        {
            Mensagem = "E-mail ou senha inválidos.";
        }
    }

    // ✅ IMPLEMENTADO
    private void ExecutarCadastro()
    {
        Mensagem = string.Empty;

        if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
        {
            Mensagem = "Preencha todos os campos.";
            return;
        }

        var sucesso = DatabaseHelper.CadastrarUsuario(Nome, Email, Senha);

        if (sucesso)
        {
            Mensagem = "Cadastro realizado! Faça o login.";
            IsCadastro = false; // volta para a tela de login
            Nome  = string.Empty;
            Senha = string.Empty;
        }
        else
        {
            Mensagem = "Erro ao cadastrar. Tente outro e-mail.";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}