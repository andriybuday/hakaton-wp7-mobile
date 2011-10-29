using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls.Primitives;

namespace Allscripts.Homecare.Mobile.Device.WinPhone7UI.Controls
{
    public class TimeSpanDataSource : ILoopingSelectorDataSource
    {
        public TimeSpanDataSource(int minValue, int maxValue, int step, string stringFormat)
        {
            Initialize(minValue, maxValue, step, stringFormat);
        }

        private void Initialize(int minValue, int maxValue, int step, string stringFormat)
        {
            MinValue = new TimeSpanItem(stringFormat) { Value = minValue };
            MaxValue = new TimeSpanItem(stringFormat) { Value = maxValue };
            Step = step;
            StringFormat = stringFormat;
            _selectedItem = MinValue;
        }

        public string StringFormat { get; set; }

        #region Properties

        public TimeSpanItem MinValue { get; set; }

        public TimeSpanItem MaxValue { get; set; }

        public int Step { get; set; }

        #endregion

        #region ILoopingSelectorDataSource Members

        public object GetNext(object current)
        {
            var next = new TimeSpanItem(StringFormat) { Value = (current as TimeSpanItem).Value + Step };
            return next.Value > MaxValue.Value ? MinValue : next;
        }

        public object GetPrevious(object current)
        {
            var prev = new TimeSpanItem(StringFormat) { Value = (current as TimeSpanItem).Value - Step };
            return prev.Value < MinValue.Value ? MaxValue : prev;
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        private TimeSpanItem _selectedItem;
        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                TimeSpanItem newValue = value as TimeSpanItem;
                if (_selectedItem.Value != newValue.Value)
                {
                    //remember previous value
                    TimeSpanItem previousSelectedItem = _selectedItem;
                    _selectedItem = newValue;

                    //raise event
                    if (SelectionChanged != null)
                    {
                        SelectionChanged(this,
                                new SelectionChangedEventArgs(new object[] {previousSelectedItem},
                                                              new object[] {_selectedItem}));
                    }
                }
            }
        }

        #endregion
    }

    public class TimeSpanItem
    {
        public TimeSpanItem(string stringFormat)
        {
            StringFormat = stringFormat;
        }

        public string StringFormat { get; private set; }

        public int Value { get; set; }

        public string HoursText
        {
            get { return Value == 1 ? "Hour" : "Hours"; }
        }

        public string MinutesText
        {
            get { return Value == 1 ? "Minute" : "Minutes"; }
        }

        public override string ToString()
        {
            return Value.ToString(StringFormat);
        }
    }
}
