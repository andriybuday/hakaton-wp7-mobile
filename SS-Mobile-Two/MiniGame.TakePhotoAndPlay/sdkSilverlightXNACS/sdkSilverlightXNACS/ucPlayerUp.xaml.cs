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
    public partial class ucPlayerUp : UserControl
    {
        public ucPlayerUp()
        {
            InitializeComponent();
        }

        bool _isBlueTurn = true;

        public bool IsBlueTurn
        {
            get
            {
                return _isBlueTurn;
            }
            set
            {
                _isBlueTurn = value;
                if (_isBlueTurn)
                {
                    tbBluePlayer.Visibility = Visibility.Visible;
                    tbRedPlayer.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tbBluePlayer.Visibility = Visibility.Collapsed;
                    tbRedPlayer.Visibility = Visibility.Visible;
                }
            }
        }

    }
}
