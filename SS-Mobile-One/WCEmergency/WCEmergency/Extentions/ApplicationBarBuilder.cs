using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Phone.Shell;

namespace WCEmergency.Extentions
{
    public class ApplicationBarBuilder
    {
        private IApplicationBar _applicationBar;
        private ItemsBuilder _itemsBuilder;

        protected IApplicationBar ApplicationBar
        {
            get { return _applicationBar; }
            set { _applicationBar = value; }
        }

        protected ItemsBuilder ItemsBuilder
        {
            get { return _itemsBuilder; }
            set { _itemsBuilder = value; }
        }

        protected virtual void CreateApplicationBar()
        {
            _applicationBar = new ApplicationBar();
            _itemsBuilder = new ItemsBuilder(_applicationBar);
        }

        public ApplicationBarBuilder Create()
        {
            CreateApplicationBar();
            _applicationBar.ForegroundColor = (Color)Application.Current.Resources["AllscriptsForegroundColor"];
            _applicationBar.BackgroundColor = (Color)Application.Current.Resources["AllscriptsBackgroundColor"];

            return this;
        }

        public ApplicationBarBuilder WithBackground(byte r, byte g, byte b)
        {
            _applicationBar.BackgroundColor = Color.FromArgb(0xFF, r, g, b);

            return this;
        }

        public ApplicationBarBuilder Update()
        {
            if (_applicationBar == null) { throw new NullReferenceException("The Application Bar in memory has not been created, you need to create the application bar prior to updating it."); }

            return this;
        }

        public ApplicationBarBuilder WithButton(string buttonName, Uri uri, bool isEnabled, Action<object, System.EventArgs> action, string text)
        {
            if (uri == null) { throw new ArgumentNullException("uri", "The provided URI was provided as null, this is not valid"); }

            _itemsBuilder.CreateButton(buttonName, uri, isEnabled, action, text);

            return this;
        }

        public virtual ApplicationBarBuilder WithButton(string buttonName, Uri uri, Binding enabledBinding, bool isEnabled, Action<object, System.EventArgs> action, string text)
        {
            return WithButton(buttonName, uri, isEnabled, action, text);
        }

        public ApplicationBarBuilder WithMenuItem(string menuName, string displayText, bool isEnabled, Action<object, System.EventArgs> action)
        {
            _itemsBuilder.CreateMenuItem(menuName, displayText, isEnabled, action);
            return this;
        }

        public virtual ApplicationBarBuilder WithMenuItem(string menuName, string displayText, Binding enabledBinding, bool isEnabled, Action<object, System.EventArgs> action)
        {
            return WithMenuItem(menuName, displayText, isEnabled, action);
        }

        public ApplicationBarBuilder ForButton(string buttonName, bool isEnabled)
        {

            _itemsBuilder.UpdateButtonEnabled(buttonName, isEnabled);

            return this;
        }

        public virtual ApplicationBar CompletedAppBar()
        {
            return _applicationBar as ApplicationBar;
        }

    }

    public interface IItemsBuilder
    {
        IApplicationBarIconButton CreateApplicationBarIconButton();
        IApplicationBarMenuItem CreateApplicationBarMenuItem();
    }

    public class ItemsBuilder : IItemsBuilder
    {
        private class ApplicationButtonManager
        {
            private IDictionary<string, IApplicationBarIconButton> _buttons = new Dictionary<string, IApplicationBarIconButton>();

            public void AddToManager(string buttonName, IApplicationBarIconButton applicationBarIconButton)
            {
                if (!_buttons.ContainsKey(buttonName))
                {
                    _buttons.Add(buttonName, applicationBarIconButton);
                }
                else
                {
                    _buttons[buttonName] = applicationBarIconButton;
                }
            }

            public IApplicationBarIconButton FindButton(string buttonName)
            {
                if (!_buttons.ContainsKey(buttonName)) { throw new IndexOutOfRangeException(string.Format("Was not able to find button name {0} in the list, make sure this is added prior to asking for it.", buttonName)); }

                return _buttons[buttonName];
            }
        }

        private class ApplicationMenuManager
        {
            private IDictionary<string, IApplicationBarMenuItem> _menuItems = new Dictionary<string, IApplicationBarMenuItem>();

            public void AddToManager(string menuName, IApplicationBarMenuItem applicationBarItem)
            {
                if (!_menuItems.ContainsKey(menuName))
                {
                    _menuItems.Add(menuName, applicationBarItem);
                }
                else
                {
                    _menuItems[menuName] = applicationBarItem;
                }
            }

            public IApplicationBarMenuItem FindButton(string menuName)
            {
                if (!_menuItems.ContainsKey(menuName)) { throw new IndexOutOfRangeException(string.Format("Was not able to find menu name {0} in the list, make sure this is added prior to asking for it.", menuName)); }

                return _menuItems[menuName];
            }
        }

        private ApplicationButtonManager _buttonManager;
        private ApplicationMenuManager _menuManager;

        private IApplicationBar applicationBar;

        public ItemsBuilder(IApplicationBar applicationBar)
        {
            this.applicationBar = applicationBar;
            _buttonManager = new ApplicationButtonManager();
            _menuManager = new ApplicationMenuManager();
        }

        public IApplicationBarIconButton CreateButton(string buttonName, Uri iconUri, bool isEnabled, Action<object, System.EventArgs> action, string text)
        {
            IApplicationBarIconButton applicationBarIconButton = CreateApplicationBarIconButton();

            if (iconUri != null) { applicationBarIconButton.IconUri = iconUri; }
            applicationBarIconButton.IsEnabled = isEnabled;
            applicationBarIconButton.Text = text;
            if (action != null)
                applicationBarIconButton.Click += new EventHandler(action);

            applicationBar.Buttons.Add(applicationBarIconButton);

            _buttonManager.AddToManager(buttonName, applicationBarIconButton);

            return applicationBarIconButton;
        }

        public IApplicationBarMenuItem CreateMenuItem(string menuName, string displayText, bool isEnabled, Action<object, System.EventArgs> action)
        {
            var applicationBarMenuItem = CreateApplicationBarMenuItem();

            applicationBarMenuItem.Text = displayText;
            applicationBarMenuItem.IsEnabled = isEnabled;

            if (action != null)
            {
                applicationBarMenuItem.Click += new EventHandler(action);
            }

            applicationBar.MenuItems.Add(applicationBarMenuItem);
            _menuManager.AddToManager(menuName, applicationBarMenuItem);

            return applicationBarMenuItem;
        }

        public ItemsBuilder UpdateButtonEnabled(string buttonName, bool isEnabled)
        {
            if (applicationBar == null) { throw new NullReferenceException("The provided application bar has not been created, this is required to update a button on it"); }
            if (string.IsNullOrEmpty(buttonName)) { throw new NullReferenceException("The button name provided was either null or empty, this is required to update a button"); }

            //applicationBar.Buttons.Where( x =>  )
            var applicationBarButton = _buttonManager.FindButton(buttonName);
            applicationBarButton.IsEnabled = isEnabled;

            _buttonManager.AddToManager(buttonName, applicationBarButton);

            return this;
        }


        public virtual IApplicationBarIconButton CreateApplicationBarIconButton()
        {
            return new ApplicationBarIconButton();
        }

        public virtual IApplicationBarMenuItem CreateApplicationBarMenuItem()
        {
            return new ApplicationBarMenuItem();
        }
    }
}
