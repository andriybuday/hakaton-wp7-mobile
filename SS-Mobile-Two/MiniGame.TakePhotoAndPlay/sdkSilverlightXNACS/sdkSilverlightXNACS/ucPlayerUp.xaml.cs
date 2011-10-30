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

        bool _isVictory = true;

        public bool IsVictory
        {
            get { return _isVictory; }
            set
            {
                _isVictory = value;
                if (_isVictory)
                {
                    RectangleBackground.Fill = new SolidColorBrush(Colors.Green);
                    TextBlockWon.Visibility = Visibility.Visible;
                    TextBlockWonCaption.Visibility = Visibility.Visible;
                    TextBlockLoose.Visibility = Visibility.Collapsed;
                    TextBlockLooseCaption.Visibility = Visibility.Collapsed;
                }
                else
                {
                    RectangleBackground.Fill = new SolidColorBrush(Colors.Brown);
                    TextBlockWon.Visibility = Visibility.Collapsed;
                    TextBlockWonCaption.Visibility = Visibility.Collapsed;
                    TextBlockLoose.Visibility = Visibility.Visible;
                    TextBlockLooseCaption.Visibility = Visibility.Visible;
                }
            }
        }

    }
}
