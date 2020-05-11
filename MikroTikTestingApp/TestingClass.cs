using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MikroTikTestingApp
{
    class TestingClass
    {
        public delegate void TestEventHandler();
        public event TestEventHandler ElapsedTime;
        private System.Timers.Timer timer;

        private int maxCycles = 10;
        private int counter = 0;
        private string wirelessRates = string.Empty;

        public void StartTestAndTimer(int maxCycles)
        {
            this.maxCycles = maxCycles;
            counter = 0;
            timer = new System.Timers.Timer();
            timer.Interval = 60000;//60000
            timer.Elapsed += OnTimerEllapsed;
            timer.Start();
            try { CallEventElapsedTime(); }catch(Exception ex) { StopTimerQuickly(); MessageBox.Show(ex.ToString()); }
        }

        public void StopTestAndTimer()
        {
            if (timer != null)
                timer.Dispose();
            MVVMmanager.isTesting = false;
            CallEventElapsedTime();
        }

        private void StopTimerQuickly()
        {
            if (timer != null)
                timer.Dispose();
            MVVMmanager.isTesting = false;
        }

        private void OnTimerEllapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            counter++;
            if(counter == maxCycles-1)
            {
                StopTestAndTimer();
                return;
            }
            CallEventElapsedTime();
        }

        public void CallEventElapsedTime()
        {
            wirelessRates = string.Empty;
            string EthernetRate = MToperationClass.MikrotikGetEthernetRate();
            string WirelessSignal = MToperationClass.MikrotikGetWirelessSignal(out wirelessRates);
            //string WirelessRates = MToperationClass.MikrotikGetWirelessRates();
            SQLiteClass.FillTables(DateTime.Now.ToString("hh:mm:ss"), EthernetRate, WirelessSignal,wirelessRates);
            ElapsedTime?.Invoke();
        }
    }
}
