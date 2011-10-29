using System;
using System.Windows;
using Microsoft.Phone.Controls.Primitives;

namespace Allscripts.Homecare.Mobile.Device.WinPhone7UI.Controls
{
    public class TimeSpanLoopingSelector: LoopingSelector
    {
        private const int MaxDefault = 100;
        private const int MinDefault = 0;
        private const int Increment = 1;

        public TimeSpanLoopingSelector()
        {
            Loaded += TimeSpanLoopingSelectorLoaded;
        }

        private void TimeSpanLoopingSelectorLoaded(object sender, RoutedEventArgs e)
        {
            CreateDataSource();
        }

        /// <summary>
        /// Creates\Recreates DataSource for selector in case it is null or selector is already expanded
        /// </summary>
        public void CreateDataSource()
        {
            if ((IsExpanded && DataSource !=null) || DataSource == null)
            {
                DataSource = new TimeSpanDataSource(MinValue, MaxValue, Step, StringFormat);
            }
            DataSource.SelectionChanged += (sender, args) => { SelectedItem = (args.AddedItems[0] as TimeSpanItem).Value; };
        }

        public int MinValue
        {
            get
            {
                return (int)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public int MaxValue
        {
            get
            {
                return (int)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public int Step
        {
            get
            {
                return (int)GetValue(StepProperty);
            }
            set
            {
                SetValue(StepProperty, value);
            }
        }

        public int SelectedItem
        {
            get
            {
                return (int)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public string StringFormat
        {
            get
            {
                return GetValue(StringFormatProperty).ToString();
            }
            set
            {
                SetValue(StringFormatProperty, value);
            }
        }

        /// <summary>
        /// The MaxValue DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty =
                DependencyProperty.Register("MaxValue", typeof(int), typeof(TimeSpanLoopingSelector), new PropertyMetadata(MaxDefault, DataSourcePropertyChanged));

        /// <summary>
        /// The MinValue DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty MinValueProperty =
                DependencyProperty.Register("MinValue", typeof(int), typeof(TimeSpanLoopingSelector), new PropertyMetadata(MinDefault, DataSourcePropertyChanged));

        /// <summary>
        /// The Step DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty StepProperty =
                DependencyProperty.Register("Step", typeof(int), typeof(TimeSpanLoopingSelector), new PropertyMetadata(Increment, DataSourcePropertyChanged));

        /// <summary>
        /// The StringFormat DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty StringFormatProperty =
                DependencyProperty.Register("StringFormat", typeof(string), typeof(TimeSpanLoopingSelector), new PropertyMetadata(string.Empty, DataSourcePropertyChanged));

        public static readonly DependencyProperty SelectedItemProperty =
                DependencyProperty.Register("SelectedItem", typeof(int), typeof(TimeSpanLoopingSelector)
                , new PropertyMetadata(new PropertyChangedCallback((sender, e) =>
                {
                    var selector = (TimeSpanLoopingSelector)sender;
                    selector.DataSource.SelectedItem = new TimeSpanItem(selector.StringFormat) { Value = (int)e.NewValue };
                })));

        private static void DataSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var selector = (TimeSpanLoopingSelector)obj;
            selector.DataSource = new TimeSpanDataSource(selector.MinValue, selector.MaxValue, selector.Step, selector.StringFormat);
        }
    }
}
