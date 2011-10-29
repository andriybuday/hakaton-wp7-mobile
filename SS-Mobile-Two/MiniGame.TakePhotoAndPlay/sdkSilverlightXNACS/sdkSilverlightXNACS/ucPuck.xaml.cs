using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ShuffleBoard
{
    public partial class ucPuck : UserControl
    {
        public ucPuck()
        {
            InitializeComponent();
        }


        public Color ColorHighlight
        {
            get
            {
                return colorHighlight.Color;
            }
            set
            {

                colorHighlight.Color = value;
            }
        }

        public Color ColorMain
        {
            get
            {
                return colorMain.Color;
            }
            set
            {

                colorMain.Color = value;
            }
        }
    }
}
