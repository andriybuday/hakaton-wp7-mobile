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
using WCEmergency.ViewModel;

namespace WCEmergency.Common
{
    public class ToiletContext
    {
        private static ToiletContext _context;

        public static ToiletContext Instance
        {
            get { return _context ?? (_context = new ToiletContext()); }
        }

        public ToiletViewItem Item { get; set; }
    }
}
