using System.Windows;
using AppGreenRoots.ViewModels;

namespace AppGreenRoots.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
    }

    // Atualiza a propriedade Senha no ViewModel toda vez que o usuário digita
    private void PbSenha_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
            vm.Senha = PbSenha.Password;
    }

    // Botão X fechar janela
    private void BtnFechar_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    
}