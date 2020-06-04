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
            try { GetInterfacesFromMT(); }catch(Exception ex) { outputTest.Text = ex.ToString(); }
        }

        private void SaveMainCredts()
        {
            if (MTisOld.IsChecked == true)
                MToperationClass.isOlderMT = true;
            else MToperationClass.isOlderMT = false;
            if (AlterMT.IsChecked == true)
                MToperationClass.alterMT_APIread = true;
            else MToperationClass.alterMT_APIread = false;
            MToperationClass.IP = MTIP.Text;
            MToperationClass.Login = MTLogin.Text;
            MToperationClass.Password = MTPassword.Text;
            try { MToperationClass.MikrotikGetSerial();}catch(Exception ex) { outputTest.Text = ex.ToString(); }
        }

        private void GetInterfacesFromMT()
        {
            SaveMainCredts();

            List<string> interfaces = null;
            MyEthernet.ItemsSource = interfaces;
            MyEthernet.Items.Clear();
            MyWireless.ItemsSource = interfaces;
            MyWireless.Items.Clear();

            //MToperationClass.MikrotikGetSerial();
            string errOutput = string.Empty;
            interfaces = MToperationClass.MikroTikGetInterfaces(out errOutput);
            outputTest.Text = errOutput;
            if (interfaces == null || interfaces.Count == 0) return;

            MyEthernet.ItemsSource = interfaces;
            MyEthernet.SelectedItem = interfaces[0];
            MyWireless.ItemsSource = interfaces;
            MyWireless.SelectedItem = interfaces[0];
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (MyEthernet.SelectedItem == null)
            {
                MessageBox.Show("Choose interfaces for surveys!");
                return;
            }
            SaveMainCredts();

            if (MThasWlan.IsChecked == true)
                MToperationClass.dontTestWLAN = true;
            else MToperationClass.dontTestWLAN = false;
            MToperationClass.EtherInt = MyEthernet.SelectedItem.ToString();
            MToperationClass.WirelessInt = MyWireless.SelectedItem.ToString();
            SaveToFile();
            quickInfoText.Text = "SAVED";
        }

        private void SaveToFile()
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                string saveSave = MTPassword.Text;
                if (saveSave == string.Empty)
                    saveSave = "none";
                sw.WriteLine(MTIP.Text+"~"+MTLogin.Text+"~"+ saveSave + "~"+MyEthernet.SelectedItem+ "~"+MyWireless.SelectedItem + "~"+MToperationClass.isOlderMT+"~"+MToperationClass.dontTestWLAN+"~" + MToperationClass.alterMT_APIread + "~");
            }
        }

        private void LoadSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(path)) return;
            outputTest.Text = string.Empty;
            LoadData();
        }

        private void LoadData()
        {
            string lines = File.ReadAllText(path);
            string[] credentials = lines.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
            MTIP.Text = credentials[0];
            MTLogin.Text = credentials[1];

            if(credentials[2] == "none")
                MTPassword.Text = string.Empty;
            else MTPassword.Text = credentials[2];

            MToperationClass.isOlderMT = Boolean.Parse(credentials[5]);
            MTisOld.IsChecked = MToperationClass.isOlderMT;
            MToperationClass.dontTestWLAN = Boolean.Parse(credentials[6]);
            MThasWlan.IsChecked = MToperationClass.dontTestWLAN;
            MToperationClass.alterMT_APIread = Boolean.Parse(credentials[7]);
            AlterMT.IsChecked = MToperationClass.alterMT_APIread;

            MToperationClass.IP = MTIP.Text;
            MToperationClass.Login = MTLogin.Text;
            MToperationClass.Password = MTPassword.Text;

            MyEthernet.Items.Add(credentials[3]);
            MyEthernet.SelectedItem = credentials[3];
            MyWireless.Items.Add(credentials[4]);
            MyWireless.SelectedItem = credentials[4];

            MToperationClass.EtherInt = MyEthernet.SelectedItem.ToString();
            MToperationClass.WirelessInt = MyWireless.SelectedItem.ToString();
            quickInfoText.Text = "LOADED";
            try { MToperationClass.MikrotikGetSerial(); } catch (Exception ex) { outputTest.Text = ex.ToString(); }
        }
    }
}
