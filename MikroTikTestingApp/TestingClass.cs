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

        public void StartTestAndTimer(int maxCycles)
        {
            this.maxCycles = maxCycles;
            counter = 0;
            timer = new System.Timers.Timer();
            timer.Interval = 600;
            timer.Elapsed += OnTimerEllapsed;
            timer.Start();
            CallEventElapsedTime();
        }

        public void StopTestAndTimer()
        {
            if (timer != null)
                timer.Dispose();
            MVVMmanager.isTesting = false;
            CallEventElapsedTime();
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
            SQLiteClass.FillTables(DateTime.Now.ToString("hh:mm:ss"), "100Mbs", "-67dB/-77dB");
            ElapsedTime?.Invoke();
        }
    }
}
