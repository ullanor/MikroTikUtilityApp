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
        //private int cyclesInterval = 60000;
        private int counter = 0;
        private string wirelessRates = string.Empty;

        private string MTupTime = string.Empty;
        private string EthernetRate = string.Empty;
        private string WirelessSignal = string.Empty;

        public void StartTestAndTimer(int maxCycles, int cyclesInterval)
        {
            this.maxCycles = maxCycles;
            //this.cyclesInterval = cyclesInterval * 1000;
            counter = 0;
            timer = new System.Timers.Timer();
            timer.Interval = cyclesInterval*1000;
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
            MTupTime = MToperationClass.MikrotikGetUpTime();
            EthernetRate = MToperationClass.MikrotikGetEthernetRate();
            WirelessSignal = MToperationClass.MikrotikGetWirelessSignal(out wirelessRates);
            //string WirelessRates = MToperationClass.MikrotikGetWirelessRates();
            SQLiteClass.FillTables(DateTime.Now.ToString("hh:mm:ss"), MTupTime, EthernetRate, WirelessSignal,wirelessRates);
            ElapsedTime?.Invoke();
        }
    }
}
