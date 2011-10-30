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

namespace WCEmergency.Common
{
    public class CurrentUserContext
    {
        private static CurrentUserContext _context;
        
        public static CurrentUserContext Instance
        {
            get { return _context ?? (_context = new CurrentUserContext()); }
        }

        public TimeSpan CanWait { get; set; }
        public int Speed { get; set; }

        public double Distance
        {
            get { return Speed * CanWait.TotalHours * 1000; }
        }
    }
}
