using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WCEmergency.Extentions;

namespace WCEmergency.UserControls
{
    public class BindableApplicationBarBuilder : ApplicationBarBuilder
    {
        protected override void CreateApplicationBar()
        {
            this.ApplicationBar = new BindableApplicationBar();
            this.ItemsBuilder = new BindableItemsBuilder(this.ApplicationBar);
        }

        public override ApplicationBarBuilder WithButton(string buttonName, Uri uri, Binding enabledBinding, bool isEnabled, Action<object, System.EventArgs> action, string text)
        {
            if (uri == null) { throw new ArgumentNullException("uri", "The provided URI was provided as null, this is not valid"); }

            BindableApplicationBarIconButton barIconButton = ItemsBuilder.CreateButton(buttonName, uri, isEnabled, action, text)
                as BindableApplicationBarIconButton;

            if (enabledBinding != null && barIconButton != null)
            {
                barIconButton.SetBinding(BindableApplicationBarMenuItem.IsEnabledProperty, enabledBinding);
            }

            return this;
        }

        public override ApplicationBarBuilder WithMenuItem(string menuName, string displayText, Binding enabledBinding, bool isEnabled, Action<object, System.EventArgs> action)
        {
            BindableApplicationBarMenuItem barMenuItem = ItemsBuilder.CreateMenuItem(menuName, displayText, isEnabled, action)
                as BindableApplicationBarMenuItem;

            if (enabledBinding != null && barMenuItem != null)
            {
                 barMenuItem.SetBinding(BindableApplicationBarMenuItem.IsEnabledProperty, enabledBinding);
            }

            return this;
        }

        public override ApplicationBar CompletedAppBar()
        {
            return (this.ApplicationBar as BindableApplicationBar).GetApplicationBar();;
        }

        private class BindableItemsBuilder : ItemsBuilder, IItemsBuilder
        {
            public BindableItemsBuilder(IApplicationBar applicationBar)
                : base(applicationBar)
            {
            }

            public override IApplicationBarIconButton CreateApplicationBarIconButton()
            {
                return new BindableApplicationBarIconButton();
            }

            public override IApplicationBarMenuItem CreateApplicationBarMenuItem()
            {
                return new BindableApplicationBarMenuItem();
            }
        }
    }

    /// <summary>
    /// Source code was found here http://www.maxpaulousky.com/blog/archive/2011/01/10/bindable-application-bar-extensions-for-windows-phone-7.aspx
    /// </summary>
    [ContentProperty("Buttons")]
    public class BindableApplicationBar : ItemsControl, IApplicationBar
    {
        private ApplicationBar _applicationBar;

        public BindableApplicationBar()
        {
            _applicationBar = new ApplicationBar();

            this.Loaded += new RoutedEventHandler(BindableApplicationBar_Loaded);
        }

        public ApplicationBar GetApplicationBar()
        {
            return _applicationBar;
        }

        private void SetApplicationBar(IApplicationBar bar)
        {
            var page = this.GetVisualAncestors().Where(c => c is PhoneApplicationPage).LastOrDefault() as PhoneApplicationPage;
            if (page != null)
            {
                page.ApplicationBar = bar;

                if (bar != null)
                    page.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(page_BackKeyPress);
                else
                    page.BackKeyPress -= page_BackKeyPress;
            }
        }

        void page_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetApplicationBar(null);
        }

        void BindableApplicationBar_Loaded(object sender, RoutedEventArgs e)
        {
            SetApplicationBar(_applicationBar);
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            _applicationBar.Buttons.Clear();
            _applicationBar.MenuItems.Clear();

            foreach (BindableApplicationBarIconButton button in Items.Where(c => c is BindableApplicationBarIconButton))
                _applicationBar.Buttons.Add(button.Button);

            foreach (BindableApplicationBarMenuItem button in Items.Where(c => c is BindableApplicationBarMenuItem && !(c is BindableApplicationBarIconButton)))
                _applicationBar.MenuItems.Add(button.Item);
        }

        public static readonly DependencyProperty IsVisibleProperty =
                DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(BindableApplicationBar), new PropertyMetadata(true, OnVisibleChanged));

        private static void OnVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ((BindableApplicationBar)d)._applicationBar.IsVisible = (bool)e.NewValue;
        }

        public static readonly DependencyProperty IsMenuEnabledProperty =
             DependencyProperty.RegisterAttached("IsMenuEnabled", typeof(bool), typeof(BindableApplicationBar), new PropertyMetadata(true, OnEnabledChanged));

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ((BindableApplicationBar)d)._applicationBar.IsMenuEnabled = (bool)e.NewValue;
        }

        public bool IsVisible
        {
            get
            {
                return (bool)GetValue(IsVisibleProperty);
            }
            set
            {
                SetValue(IsVisibleProperty, value);
            }
        }

        public double BarOpacity
        {
            get
            {
                return _applicationBar.Opacity;
            }
            set
            {
                _applicationBar.Opacity = value;
            }
        }

        public bool IsMenuEnabled
        {
            get
            {
                return (bool)GetValue(IsMenuEnabledProperty);
            }
            set
            {
                SetValue(IsMenuEnabledProperty, value);
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return _applicationBar.BackgroundColor;
            }
            set
            {
                _applicationBar.BackgroundColor = value;
            }
        }

        public Color ForegroundColor
        {
            get
            {
                return _applicationBar.ForegroundColor;
            }
            set
            {
                _applicationBar.ForegroundColor = value;
            }
        }

        public IList Buttons
        {
            get
            {
                return this.Items;
            }

        }

        public IList MenuItems
        {
            get
            {
                return this.Items;
            }
        }

        public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;


        public double DefaultSize
        {
            get { return 72; }
        }

        public double MiniSize
        {
            get { return 30; }
        }

        public ApplicationBarMode Mode { get; set; }
    }

    public class BindableApplicationBarMenuItem : FrameworkElement, IApplicationBarMenuItem
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(BindableApplicationBarMenuItem), new PropertyMetadata(true, OnEnabledChanged));

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string), typeof(BindableApplicationBarMenuItem), new PropertyMetadata(OnTextChanged));

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ((BindableApplicationBarMenuItem)d).Item.IsEnabled = (bool)e.NewValue;
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ((BindableApplicationBarMenuItem)d).Item.Text = e.NewValue.ToString();
        }

        public IApplicationBarMenuItem Item
        {
            get;
            set;
        }

        public BindableApplicationBarMenuItem()
        {
            Item = CreateItem();
            if (DesignerProperties.IsInDesignTool)
                Item.Text = "Text";

            Item.Click += (s, e) =>
            {
                if (Click != null)
                    Click(s, e);
            };
        }

        protected virtual IApplicationBarMenuItem CreateItem()
        {
            return new ApplicationBarMenuItem();
        }

        public bool IsEnabled
        {
            get
            {
                return (bool)GetValue(IsEnabledProperty);
            }
            set
            {
                SetValue(IsEnabledProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public event EventHandler Click;

    }

    public class BindableApplicationBarIconButton : BindableApplicationBarMenuItem, IApplicationBarIconButton
    {
        public BindableApplicationBarIconButton()
            : base()
        {
        }

        public ApplicationBarIconButton Button
        {
            get
            {
                return (ApplicationBarIconButton)Item;
            }
        }

        protected override IApplicationBarMenuItem CreateItem()
        {
            return new ApplicationBarIconButton();
        }

        public Uri IconUri
        {
            get
            {
                return Button.IconUri;
            }
            set
            {
                Button.IconUri = value;
            }
        }

    }

    /// <summary>
    /// Behavior to handle connecting a <see cref="IApplicationBarMenuItem"/> to a Command.
    /// </summary>
    /// <typeparam name="T">The target object must derive from IApplicationBarMenuItem</typeparam>
    public class AppBarItemCommandBehavior<T>
            where T : IApplicationBarMenuItem
    {
        private ICommand command;
        private object commandParameter;
        private readonly WeakReference targetObject;
        private readonly EventHandler commandCanExecuteChangedHandler;

        /// <summary>
        /// Constructor specifying the target object.
        /// </summary>
        /// <param name="targetObject">The target object the behavior is attached to.</param>
        public AppBarItemCommandBehavior(T targetObject)
        {
            this.targetObject = new WeakReference(targetObject);
            commandCanExecuteChangedHandler = new EventHandler(this.CommandCanExecuteChanged);
            ((T)this.targetObject.Target).Click += (s, e) => ExecuteCommand();
        }

        /// <summary>
        /// Corresponding command to be execute and monitored for <see cref="ICommand.CanExecuteChanged"/>
        /// </summary>
        public ICommand Command
        {
            get
            {
                return command;
            }
            set
            {
                if (this.command != null)
                    this.command.CanExecuteChanged -= this.commandCanExecuteChangedHandler;

                this.command = value;
                if (this.command != null)
                {
                    this.command.CanExecuteChanged += this.commandCanExecuteChangedHandler;
                    UpdateEnabledState();
                }
            }
        }

        /// <summary>
        /// The parameter to supply the command during execution
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return this.commandParameter;
            }
            set
            {
                if (this.commandParameter != value)
                {
                    this.commandParameter = value;
                    this.UpdateEnabledState();
                }
            }
        }

        /// <summary>
        /// Object to which this behavior is attached.
        /// </summary>
        protected T TargetObject
        {
            get
            {
                return (T)targetObject.Target;
            }
        }

        /// <summary>
        /// Updates the target object's IsEnabled property based on the commands ability to execute.
        /// </summary>
        protected virtual void UpdateEnabledState()
        {
            if (TargetObject == null)
            {
                this.Command = null;
                this.CommandParameter = null;
            }
            else if (this.Command != null)
                TargetObject.IsEnabled = this.Command.CanExecute(this.CommandParameter);
        }

        private void CommandCanExecuteChanged(object sender, System.EventArgs e)
        {
            this.UpdateEnabledState();
        }

        /// <summary>
        /// Executes the command, if it's set, providing the <see cref="CommandParameter"/>
        /// </summary>
        protected virtual void ExecuteCommand()
        {
            if (this.Command != null)
                this.Command.Execute(this.CommandParameter);
        }
    }

    /// <summary>
    /// Static Class that holds all Dependency Properties and Static methods to allow 
    /// the Click event of the IApplicationBarMenuItem interface to be attached to a Command. 
    /// </summary>
    /// <remarks>
    /// This class is required, because Silverlight for WinPhone doesn't have native support for Commands. 
    /// </remarks>
    public static class AppBarItemClick
    {
        private static readonly DependencyProperty ClickCommandBehaviorProperty = DependencyProperty.RegisterAttached(
                "ClickCommandBehavior",
                typeof(AppBarItemCommandBehavior<IApplicationBarMenuItem>),
                typeof(AppBarItemClick),
                null);


        /// <summary>
        /// Command to execute on click event.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(AppBarItemClick),
                new PropertyMetadata(OnSetCommandCallback));

        /// <summary>
        /// Command parameter to supply on command execution.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
                "CommandParameter",
                typeof(object),
                typeof(AppBarItemClick),
                new PropertyMetadata(OnSetCommandParameterCallback));


        /// <summary>
        /// Sets the <see cref="ICommand"/> to execute on the click event.
        /// </summary>
        /// <param name="item">AppBarItem dependency object to attach command</param>
        /// <param name="command">Command to attach</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for IApplicationBarMenuItem")]
        public static void SetCommand(IApplicationBarMenuItem item, ICommand command)
        {
            (item as FrameworkElement).SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Retrieves the <see cref="ICommand"/> attached to the <see cref="IApplicationBarMenuItem"/>.
        /// </summary>
        /// <param name="item">IApplicationBarMenuItem containing the Command dependency property</param>
        /// <returns>The value of the command attached</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for IApplicationBarMenuItem")]
        public static ICommand GetCommand(IApplicationBarMenuItem item)
        {
            return (item as FrameworkElement).GetValue(CommandProperty) as ICommand;
        }

        /// <summary>
        /// Sets the value for the CommandParameter attached property on the provided <see cref="IApplicationBarMenuItem"/>.
        /// </summary>
        /// <param name="item">IApplicationBarMenuItem to attach CommandParameter</param>
        /// <param name="parameter">Parameter value to attach</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for IApplicationBarMenuItem")]
        public static void SetCommandParameter(IApplicationBarMenuItem item, object parameter)
        {
            (item as FrameworkElement).SetValue(CommandParameterProperty, parameter);
        }

        /// <summary>
        /// Gets the value in CommandParameter attached property on the provided <see cref="IApplicationBarMenuItem"/>
        /// </summary>
        /// <param name="item">IApplicationBarMenuItem that has the CommandParameter</param>
        /// <returns>The value of the property</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Only works for IApplicationBarMenuItem")]
        public static object GetCommandParameter(IApplicationBarMenuItem item)
        {
            return (item as FrameworkElement).GetValue(CommandParameterProperty);
        }

        private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            IApplicationBarMenuItem item = dependencyObject as IApplicationBarMenuItem;
            if (item != null)
            {
                AppBarItemCommandBehavior<IApplicationBarMenuItem> behavior = GetOrCreateBehavior(item);
                behavior.Command = e.NewValue as ICommand;
            }
        }

        private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            IApplicationBarMenuItem item = dependencyObject as IApplicationBarMenuItem;
            if (item != null)
            {
                AppBarItemCommandBehavior<IApplicationBarMenuItem> behavior = GetOrCreateBehavior(item);
                behavior.CommandParameter = e.NewValue;
            }
        }

        private static AppBarItemCommandBehavior<IApplicationBarMenuItem> GetOrCreateBehavior(IApplicationBarMenuItem item)
        {
            AppBarItemCommandBehavior<IApplicationBarMenuItem> behavior = (item as FrameworkElement).GetValue(ClickCommandBehaviorProperty) as AppBarItemCommandBehavior<IApplicationBarMenuItem>;
            if (behavior == null)
            {
                behavior = new AppBarItemCommandBehavior<IApplicationBarMenuItem>(item);
                (item as FrameworkElement).SetValue(ClickCommandBehaviorProperty, behavior);
            }

            return behavior;
        }
    }
}
