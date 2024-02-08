using System;
using System.Windows;

namespace MineSweeperDemo
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        public void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new GameTable());
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SettingsPage(mainFrame));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
