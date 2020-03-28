using MikroTikTestingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MikroTikTestingApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowIsLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

        private void MainView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

        private void CameraView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new SettingsViewModel();
        }

        private void StatusView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new StatusViewModel();
        }

        //private void CameraView_Click(object sender, RoutedEventArgs e)
        //{
        //    DataContext = new CameraViewModel();
        //}

        //private void MainView_Click(object sender, RoutedEventArgs e)
        //{
        //    DataContext = new MainViewModel();
        //}
    }
}
