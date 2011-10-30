using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WCEmergency.UserControls
{
    public partial class CommandButton : System.Windows.Controls.Button
    {
        public CommandButton()
        {
            Click += (sender, e) =>
            {
                if (Command != null && Command.CanExecute(CommandParameter))
                    Command.Execute(CommandParameter);
            };
        }

        public new static DependencyProperty CommandProperty =
            DependencyProperty.Register("Command",
                                        typeof(ICommand), typeof(CommandButton),
                                        new PropertyMetadata(null, CommandChanged));

        private static void CommandChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var button = source as CommandButton;
            if (button == null) return;

            button.RegisterCommand(args.OldValue as ICommand, args.NewValue as ICommand);
        }

        private void RegisterCommand(ICommand oldCommand, ICommand newCommand)
        {
            if (oldCommand != null)
                oldCommand.CanExecuteChanged -= HandleCanExecuteChanged;

            if (newCommand != null)
                newCommand.CanExecuteChanged += HandleCanExecuteChanged;

            HandleCanExecuteChanged(newCommand, System.EventArgs.Empty);
        }

        private void HandleCanExecuteChanged(object sender, System.EventArgs args)
        {
            if (Command != null)
                IsEnabled = Command.CanExecute(CommandParameter);
        }

        public new ICommand Command
        {
            get { return GetValue(CommandProperty) as ICommand; }
            set { SetValue(CommandProperty, value); }
        }

        public new static DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter",
                                        typeof(object), typeof(CommandButton),
                                        new PropertyMetadata(null));

        public new object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
    }
}
