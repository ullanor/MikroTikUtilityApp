using System;
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
        //private bool isDBclear;
        //private string NoOfSurveys = string.Empty;
        //private string SurveysInterval = string.Empty;
        private List<string> checkList;
        private string pathDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\MT-PabloTESTs";

        public MainView()
        {
            InitializeComponent();
            FileName.Text = MToperationClass.MTserialNo;
        }

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            //create db
            SQLiteClass.CreateTables();

            if (MVVMmanager.isTesting)
                return;
            try { int.Parse(CyclesCount.Text); } catch (Exception ex) { MessageBox.Show(ex.ToString()); return; }
            if(int.Parse(CyclesCount.Text) < 2) { MessageBox.Show("Minimal number of surveys is 2"); return; }
            try { int.Parse(CyclesInterval.Text); } catch (Exception ex) { MessageBox.Show(ex.ToString()); return; }
            if (int.Parse(CyclesInterval.Text) < 1) { MessageBox.Show("Minimal interval between surveys is 1"); return; }
            checkList = SQLiteClass.ReadFromEther();
            if (/*!MVVMmanager.isDBClear*/checkList.Count != 0) { MessageBox.Show("You have to clear DB before next Test!"); return; }
            if (!CheckCredentials()) return;

            //MVVMmanager.isDBClear = false;
            MVVMmanager.isTesting = true;
            MVVMmanager.TS.StartTestAndTimer(int.Parse(CyclesCount.Text), int.Parse(CyclesInterval.Text));

            MVVMmanager.NoOfCycles = CyclesCount.Text;
            MVVMmanager.CyclesInterval = CyclesInterval.Text;
        }

        private void StopTest_Click(object sender, RoutedEventArgs e)
        {
            if (!MVVMmanager.isTesting)
                return;
            MVVMmanager.TS.StopTestAndTimer();
        }

        private void ClearDB_Click(object sender, RoutedEventArgs e)
        {
            AreUsure();
        }

        private void ExportDB_Click(object sender, RoutedEventArgs e)
        {
            if(FileName.Text == string.Empty || !File.Exists(MVVMmanager.MainDBpath))
            {
                MessageBox.Show("No file to export!");
                return;
            }
            DataTable DT = SQLiteClass.ExportToFile();
            SaveAFile(DT);
        }
        //-------------------------------------------------------------------------------
        private bool CheckCredentials()
        {
            if(MToperationClass.IP == string.Empty || MToperationClass.Login == string.Empty || MToperationClass.EtherInt == string.Empty || MToperationClass.WirelessInt == string.Empty)
            {
                MessageBox.Show("Before testing set up Mikrotik connection credentials!");
                return false;
            }
            return true;
        }

        private void AreUsure()
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are u sure?", "Clearing DB", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                SQLiteClass.ClearTables();
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                //do something else
            }
        }

        //exporting data to file
        private void SaveAFile(DataTable dt)
        {
            //string pathDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\MT-PabloTESTs";
            string pathTXT = pathDir + $"\\{FileName.Text}.txt";
            string pathDB = pathDir + $"\\{FileName.Text}.db";
            if (File.Exists(pathTXT) || File.Exists(pathDB))
            {
                MessageBox.Show("File already Exists!");
                return;
            }
            //save database file NEW------------------
            Directory.CreateDirectory(pathDir);
            File.Copy(MVVMmanager.MainDBpath, pathDB);
            FileName.Text = string.Empty;
            //---------------------------
            //entry file info (date,name,etc)
            using (StreamWriter sw = File.AppendText(pathTXT))
            {
                sw.WriteLine("MT-serial: "+ MToperationClass.MTserialNo);
                sw.WriteLine("SurveyTime: " + DateTime.Now.ToString("dd-MM-yyyy - hh:mm:ss"));
                sw.WriteLine("Surveys: " + MVVMmanager.NoOfCycles + " Interval: "+ MVVMmanager.CyclesInterval + Environment.NewLine);
            }
            foreach (DataRow row in dt.Rows)
            {
                using (StreamWriter sw = File.AppendText(pathTXT))
                {
                    sw.WriteLine("UpTime: "+row["UpTime"]+" || EthLink: "+ row["EthStatus"] + " || WirSignal: "+ row["WirStatus"]+" || WirRates: "+row["Rates"]);
                }
            }
            MessageBox.Show("File was created!");
        }

        private void ImportDB_Click(object sender, RoutedEventArgs e)
        {
            try{ tryImportingDB(); }catch(Exception ex) { MessageBox.Show("Database is currently in use!"); }
        }

        private void tryImportingDB()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".db"; // Default file extension
            dlg.Filter = "MTpablo Sqlite DB (.db)|*.db"; // Filter files by extension
            dlg.InitialDirectory = pathDir;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                //string filename = dlg.FileName;
                File.Delete(MVVMmanager.MainDBpath);
                File.Copy(dlg.FileName, MVVMmanager.MainDBpath);
                FileName.Text = dlg.SafeFileName.Substring(0,dlg.SafeFileName.Length-3);
                MToperationClass.MTserialNo = string.Empty;
            }
        }
    }
}
