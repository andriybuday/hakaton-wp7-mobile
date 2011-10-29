using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WCEmergency.Common;

namespace WCEmergency.ViewModel
{
    public class WcStartPageViewModel : NotifyPropertyChanged
    {
        private int _hours;
        public int Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                 OnPropertyChanged("Hours");
            }
        }

        private int _minutes;
        public int Minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                OnPropertyChanged("Minutes");
            }
        }
    }
}
