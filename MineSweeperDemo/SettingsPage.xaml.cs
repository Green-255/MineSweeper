using System;
using System.Windows;
using System.Windows.Controls;

namespace MineSweeperDemo
{
    public partial class SettingsPage : Page
    {
        private readonly Frame _mainFrame;

        public SettingsPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_mainFrame != null && _mainFrame.NavigationService != null)
            {
                _mainFrame.NavigationService.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));
            }
        }
    }
}
