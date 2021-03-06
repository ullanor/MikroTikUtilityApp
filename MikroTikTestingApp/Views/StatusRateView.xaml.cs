﻿using System;
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

namespace MikroTikTestingApp.Views
{
    /// <summary>
    /// Logika interakcji dla klasy StatusRateView.xaml
    /// </summary>
    public partial class StatusRateView : UserControl
    {
        List<string> trueList;
        public StatusRateView()
        {
            InitializeComponent();

            //just for testing todo (get data from database)!--
            trueList = SQLiteClass.ReadFromRates();
            FillTable();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.testGrid.Items.Clear();
        }

        private void FillTable()
        {
            List<string> testList = trueList;
            int numberOfRecords = testList.Count;
            int units = 0;
            if (numberOfRecords % 10 == 0) units = numberOfRecords / 10;
            else units = (numberOfRecords / 10) + 1;
            int rest = Math.Abs(numberOfRecords - units * 10);
            for (int i = 0; i < rest; i++)
                testList.Add(string.Empty);

            TableValues TV;
            for (int i = 0; i < units; i++)
            {
                TV = new TableValues
                {
                    lp = i.ToString(),
                    value1 = testList[i * 10],
                    value2 = testList[i * 10 + 1],
                    value3 = testList[i * 10 + 2],
                    value4 = testList[i * 10 + 3],
                    value5 = testList[i * 10 + 4],
                    value6 = testList[i * 10 + 5],
                    value7 = testList[i * 10 + 6],
                    value8 = testList[i * 10 + 7],
                    value9 = testList[i * 10 + 8],
                    value10 = testList[i * 10 + 9],
                };
                this.testGrid.Items.Add(TV);
            }
        }

        public class TableValues
        {
            public string lp { get; set; }
            public string value1 { get; set; }
            public string value2 { get; set; }
            public string value3 { get; set; }
            public string value4 { get; set; }
            public string value5 { get; set; }
            public string value6 { get; set; }
            public string value7 { get; set; }
            public string value8 { get; set; }
            public string value9 { get; set; }
            public string value10 { get; set; }
        }

        private void testGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var cellInfo = testGrid.SelectedCells[int.Parse(testGrid.CurrentCell.Column.Header.ToString())];
                TextBlock tb = cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock;
                //MessageBox.Show(tb.Text);
                myText.Text = tb.Text;
            }
            catch (Exception ex) { myText.Text = ex.ToString(); }
        }
    }
}
