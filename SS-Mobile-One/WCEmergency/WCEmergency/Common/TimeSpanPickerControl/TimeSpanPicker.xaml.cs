using System;
using System.Windows;
using System.Windows.Controls;

namespace WCEmergency.Common.TimeSpanPickerControl
{
    public partial class TimeSpanPicker : UserControl
    {
        public TimeSpanPicker()
        {
            InitializeComponent();
           // constantCheckBox.Checked += (sender, args) => CollapseSelectors();
          //  constantCheckBox.Unchecked += (sender, args) => CollapseSelectors();
        }
/*
        public override PopupViewModelBase CreateViewModel()
        {
           // var vm = ServiceLocator.Get<TimeSpanPickerViewModel>();
           // vm.ValidationFailed += base.ValidationFailed;
           // return vm;
        }*/

        /// <summary>
        /// Specifies control for notification user about failure
        /// </summary>
        /// <returns> Returns panel with looping selectors inside</returns>
        protected UIElement FindEmptyUIElement()
        {
            //Pass whole grid in order to animate two selectors inside
            return ContentPanel;
        }

        /// <summary>
        /// Collapses and changes enabling of hour and minute selectors.
        /// </summary>
        private void CollapseSelectors()
        {
            //recreate datasource in case selectors are expanded
            hourSelector.CreateDataSource();
            minuteSelector.CreateDataSource();

           // var isConstant = constantCheckBox.IsChecked.GetValueOrDefault();

            //change enabling if selectors
            //hourSelector.IsEnabled = !isConstant;
            //minuteSelector.IsEnabled = !isConstant;

            //Collapse selectors. 
            //It provides dropping down animation in case collapsing performs as last action
            hourSelector.IsExpanded = false;
            minuteSelector.IsExpanded = false;
        }
    }

    public class TimeSpanPickerEventArgs : System.EventArgs
    {
        public bool IsConstantVisible { get; set; }
        public bool IsHoursDisabled { get; set; }
        public bool IsMinutesDisabled { get; set; }
        public bool IsConstant { get; set; }
        public TimeSpan? Value { get; set; }
    }
}