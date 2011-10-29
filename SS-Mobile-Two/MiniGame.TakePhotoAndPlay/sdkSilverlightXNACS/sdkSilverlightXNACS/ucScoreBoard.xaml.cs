using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ShuffleBoard
{
	public partial class ucScoreBoard : UserControl
	{
		public ucScoreBoard()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        public int BlueScore
        {
            get
            { 
                return Convert.ToInt32(tbBlueScore.Text); 
            }

            set
            {
                tbBlueScore.Text = value.ToString();
            }
        }

        public int RedScore
        {
            get
            {
                return Convert.ToInt32(tbRedScore.Text);
            }

            set
            {
                tbRedScore.Text = value.ToString();
            }
        }

    }
}