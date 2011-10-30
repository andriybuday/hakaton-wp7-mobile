using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace sdkSilverlightXNACS.Controls
{
    public class WaitIndicator : Control
    {
        private Storyboard _storyBoard;

        public WaitIndicator()
        {
            this.DefaultStyleKey = typeof(WaitIndicator);
        }

        public static readonly DependencyProperty IsProgressStartedProperty = DependencyProperty.Register("IsProgressStarted", typeof(bool), typeof(WaitIndicator), new PropertyMetadata(true, IsProgressStartedProperty_Changed));

        public bool IsProgressStarted
        {
            get { return (bool)GetValue(IsProgressStartedProperty); }
            set { SetValue(IsProgressStartedProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _storyBoard = this.GetTemplateChild("Storyboard1") as Storyboard;
            this.StartOrStopAnimation();
        }

        private void StartOrStopAnimation()
        {
            if (_storyBoard == null) return;

            if (this.IsProgressStarted)
                _storyBoard.Begin();
            else
                _storyBoard.Stop();
        }

        private static void IsProgressStartedProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var waitIndicator = (WaitIndicator)d;

            waitIndicator.StartOrStopAnimation();
        }
    }
}


