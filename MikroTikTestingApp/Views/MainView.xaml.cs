﻿using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logika interakcji dla klasy MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private bool isDBclear;
        public MainView()
        {
            InitializeComponent();
        }

        //private void TestServer_Click(object sender, RoutedEventArgs e)
        //{
        //    string test = "1000Mbs\n10:10:10";
        //    //string test = "1000Mbs\n";
        //    MessageBox.Show(test);
        //    if(test.Length-12 > 0)
        //        MessageBox.Show(test.Substring(0,test.Length-12));
        //}

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            if (MVVMmanager.isTesting)
                return;
            try { int.Parse(CyclesCount.Text); } catch (Exception ex) { MessageBox.Show(ex.ToString()); return; }
            if(int.Parse(CyclesCount.Text) < 2) { MessageBox.Show("Minimal number of surveys is 2"); return; }
            if (!isDBclear) { MessageBox.Show("You have to clear DB before next Test!"); return; }
            isDBclear = false;
            MVVMmanager.isTesting = true;
            MVVMmanager.TS.StartTestAndTimer(int.Parse(CyclesCount.Text));
        }

        private void StopTest_Click(object sender, RoutedEventArgs e)
        {
            if (!MVVMmanager.isTesting)
                return;
            MVVMmanager.TS.StopTestAndTimer();
        }

        private void ClearDB_Click(object sender, RoutedEventArgs e)
        {
            SQLiteClass.ClearTables();
            isDBclear = true;
        }

        private void ExportDB_Click(object sender, RoutedEventArgs e)
        {
            DataTable DT = SQLiteClass.ExportToFile();
            SaveAFile(DT);
        }

        //exporting data to file
        private void SaveAFile(DataTable dt)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{FileName.Text}.txt";
            if (File.Exists(path))
            {
                MessageBox.Show("File already Exists!");
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(row["CurTime"]+" "+ row["EthStatus"] + " "+ row["WirStatus"]);
                }
            }
        }
    }
}
