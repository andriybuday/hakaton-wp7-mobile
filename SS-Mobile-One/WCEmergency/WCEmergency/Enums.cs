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

namespace WCEmergency
{
    public class Enums
    {
        /// <summary>
        /// Enumeration to provide details on the view stage for a given view.
        /// This is used to determine if a view is being newly constructed or if it has been
        ///     pulled off of the backstack
        /// </summary>
        public enum ViewLifeStage
        {
            /// <summary>
            /// Will indicate that the view has been newly created.
            /// This will be set when the view has been navigated to via forward navigation
            /// </summary>
            Constructed,

            /// <summary>
            /// Will indicate that the view has been reloaded off of the backstack
            /// This will be set when the view is navigated to via the back button/back stack
            /// </summary>
            Rehydrated
        }
    }
}
