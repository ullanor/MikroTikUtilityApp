using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MikroTikTestingApp
{
    class StatusManager : INotifyPropertyChanged
    {
        public void Clear()
        {
            TextT = "empty";
            OnProperyChanged("TextT");
        }

        public void SetTextT(string text)
        {
            TextT = text;
            OnProperyChanged("TextT");
        }

        public string TextT { get; set; }
        //propertychanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnProperyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
