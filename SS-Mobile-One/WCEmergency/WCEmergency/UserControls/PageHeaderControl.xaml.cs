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

namespace WCEmergency.UserControls
{
    public partial class PageHeaderControl : UserControl
    {
        public PageHeaderControl()
        {
            InitializeComponent();
        }

        public Style PatientHeaderStyle
        {
            get { return (Style)GetValue(PatientHeaderStyleProperty); }
            set { SetValue(PatientHeaderStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PatientHeaderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PatientHeaderStyleProperty =
            DependencyProperty.Register("PatientHeaderStyle", typeof(Style), typeof(PageHeaderControl), new PropertyMetadata(PatientHeaderStyleChanged));

        private static void PatientHeaderStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageHeaderControl = d as PageHeaderControl;
            var newStyle = e.NewValue as Style;
            if (pageHeaderControl != null)
            {
               // pageHeaderControl.PatientHeader.Style = newStyle;
            }
        }

        public Visibility AllscriptsLogoVisibility
        {
            get { return (Visibility)GetValue(AllscriptsLogoVisibilityProperty); }
            set { SetValue(AllscriptsLogoVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllscriptsLogoVisibility.
        public static readonly DependencyProperty AllscriptsLogoVisibilityProperty =
            DependencyProperty.Register("AllscriptsLogoVisibility", typeof(Visibility), typeof(PageHeaderControl), new PropertyMetadata(Visibility.Visible, AllscriptsLogoVisibilityChanged));

        private static void AllscriptsLogoVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageHeaderControl = d as PageHeaderControl;
            var newVisibility = (Visibility)e.NewValue;
            if (pageHeaderControl != null)
            {
                //pageHeaderControl.AllscriptsLogo.Visibility = newVisibility;
            }
        }

        public Visibility PatientHeaderVisibility
        {
            get { return (Visibility)GetValue(PatientHeaderVisibilityProperty); }
            set { SetValue(PatientHeaderVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PatientHeaderVisibility.
        public static readonly DependencyProperty PatientHeaderVisibilityProperty =
            DependencyProperty.Register("PatientHeaderVisibility", typeof(Visibility), typeof(PageHeaderControl), new PropertyMetadata(Visibility.Visible, PatientHeaderVisibilityChanged));

        private static void PatientHeaderVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageHeaderControl = d as PageHeaderControl;
            var newVisibility = (Visibility)e.NewValue;
            if (pageHeaderControl != null)
            {
              //  pageHeaderControl.PatientHeader.Visibility = newVisibility;
            }
        }

        public Thickness AllscriptsTitleMargin
        {
            get { return (Thickness)GetValue(AllscriptsTitleMarginProperty); }
            set { SetValue(AllscriptsTitleMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllscriptsTitleMargin.
        public static readonly DependencyProperty AllscriptsTitleMarginProperty =
            DependencyProperty.Register("AllscriptsTitleMargin", typeof(Thickness), typeof(PageHeaderControl), new PropertyMetadata(new Thickness(), AllscriptsTitleMarginChanged));

        private static void AllscriptsTitleMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageHeaderControl = d as PageHeaderControl;
            var newMargin = (Thickness)e.NewValue;
            if (pageHeaderControl != null)
            {
               // pageHeaderControl.TitleBorder.Margin = newMargin;
            }
        }

        public Style PageTitleStyle
        {
            get { return (Style)GetValue(PageTitleStyleProperty); }
            set { SetValue(PageTitleStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageTitleStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageTitleStyleProperty =
            DependencyProperty.Register("PageTitleStyle", typeof(Style), typeof(PageHeaderControl), new PropertyMetadata(PageTitleStyleChanged));


        private static void PageTitleStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageHeaderControl = d as PageHeaderControl;
            var newStyle = e.NewValue as Style;
            if (pageHeaderControl != null)
            {
              //  pageHeaderControl.PageTitle.Style = newStyle;
            }
        }

        public Style PageTitle2LevelStyle
        {
            get { return (Style)GetValue(PageTitle2LevelStyleProperty); }
            set { SetValue(PageTitle2LevelStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageTitle2LevelStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageTitle2LevelStyleProperty =
            DependencyProperty.Register("PageTitle2LevelStyle", typeof(Style), typeof(PageHeaderControl), new PropertyMetadata(PageTitle2LevelStyleChanged));

        private static void PageTitle2LevelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageHeaderControl = d as PageHeaderControl;
            var newStyle = e.NewValue as Style;
            if (pageHeaderControl != null)
            {
                //pageHeaderControl.PageTitle2Level.Style = newStyle;
            }
        }



        public object PatientHeaderDataContext
        {
            get { return GetValue(PatientHeaderDataContextProperty); }
            set { SetValue(PatientHeaderDataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PatientHeaderDataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PatientHeaderDataContextProperty =
            DependencyProperty.Register("PatientHeaderDataContext", typeof(object), typeof(PageHeaderControl), new PropertyMetadata(PatientHeaderDataContextChanged));

        private static void PatientHeaderDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageHeaderControl = d as PageHeaderControl;
            if (pageHeaderControl != null)
            {
               // pageHeaderControl.PatientHeader.DataContext = e.NewValue;
            }
        }

        private void PatientHeader_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnPatientAreaClicked();
        }


        public event EventHandler PatientAreaClicked;

        private void OnPatientAreaClicked()
        {
            if (PatientAreaClicked != null)
            {
                PatientAreaClicked(this, System.EventArgs.Empty);
            }
            else
            {
                // right now this is just to ensure this works.  we will want to put real logic here to determine where we are
                // in the process and then determine where to go
                var viewModelBase = DataContext as ViewModelBase;

                viewModelBase.RaiseNavigateBackToPage("NewDashboard");
            }
        }

    }
}
