using System.Windows;

namespace AppGreenRoots.Views;

public partial class DashboardView : Window
{
    public DashboardView()
    {
        InitializeComponent();
    }

    private void BtnFechar_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}