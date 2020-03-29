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
        private int testCount = 0;
        public MainWindow()
        {
            InitializeComponent();
            SQLiteClass.CreateTables();
            MVVMmanager.TS = new TestingClass();
            MVVMmanager.TS.ElapsedTime += CheckTestStatus;
        }

        private void WindowIsLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

        private void MainView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

        private void StatusView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new StatusViewModel();
        }

        private void SettingsView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new SettingsViewModel();
        }

        private void WirelessStatusView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new StatusWirViewModel();
        }

        private void CheckTestStatus()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                testCount++;
                testTime.Text = testCount.ToString();
                testStatus.Text = MVVMmanager.isTesting.ToString();
                if (MVVMmanager.isTesting)
                    testStatus.Background = Brushes.Green;
                else testStatus.Background = Brushes.White;
            });
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            testCount = 0;
            testTime.Text = string.Empty;
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
