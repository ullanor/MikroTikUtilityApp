using System;
using System.Collections.Generic;
using System.IO;
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

namespace MikroTikTestingApp.Views
{
    /// <summary>
    /// Logika interakcji dla klasy SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + $"\\MT-pablo.conf";
        public SettingsView()
        {
            InitializeComponent();
        }

        private void GetInterfaces_Click(object sender, RoutedEventArgs e)
        {
            MToperationClass.IP = MTIP.Text;
            MToperationClass.Login = MTLogin.Text;
            MToperationClass.Password = MTPassword.Text;

            List<string> interfaces = null;
            try { interfaces = MToperationClass.MikroTikGetInterfaces(); }catch(Exception ex) { MessageBox.Show(ex.ToString()); }
            if (interfaces == null) return;
            if (interfaces.Count == 0) return;
            MyEthernet.Items.Clear();
            MyEthernet.ItemsSource = interfaces;
            MyEthernet.SelectedItem = interfaces[0];
            MyWireless.Items.Clear();
            MyWireless.ItemsSource = interfaces;
            MyWireless.SelectedItem = interfaces[0];
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();
            if (MyEthernet.SelectedItem == null)
            {
                MessageBox.Show("Choose interfaces for surveys!");
                return;
            }
            MToperationClass.EtherInt = MyEthernet.SelectedItem.ToString();
            MToperationClass.WirelessInt = MyWireless.SelectedItem.ToString();
        }

        private void SaveToFile()
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(MTIP.Text+"~"+MTLogin.Text+"~"+MTPassword.Text+"~"+MyEthernet.SelectedItem+ "~"+MyWireless.SelectedItem + "~");
            }
        }

        private void LoadSettings_Click(object sender, RoutedEventArgs e)
        {
            string lines = File.ReadAllText(path);
            string[] credentials = lines.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
            MTIP.Text = credentials[0];
            MTLogin.Text = credentials[1];
            MTPassword.Text = credentials[2];

            MToperationClass.IP = MTIP.Text;
            MToperationClass.Login = MTLogin.Text;
            MToperationClass.Password = MTPassword.Text;

            if (credentials.Length > 4)
            {
                MyEthernet.Items.Add(credentials[3]);
                MyEthernet.SelectedItem = credentials[3];
                MyWireless.Items.Add(credentials[4]);
                MyWireless.SelectedItem = credentials[4];

                MToperationClass.EtherInt = MyEthernet.SelectedItem.ToString();
                MToperationClass.WirelessInt = MyWireless.SelectedItem.ToString();
            }
        }
    }
}
