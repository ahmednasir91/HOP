﻿#pragma checksum "C:\Users\Ahmed\Documents\Visual Studio 2010\Projects\HomeOnPhone\HomeOnPhone\SellHome.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "652D9DF1E6E6ECA723CA236D5A91A0E4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace HomeOnPhone {
    
    
    public partial class SellHome : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Grid MapGrid;
        
        internal System.Windows.Controls.TextBox LocationTextBox;
        
        internal System.Windows.Controls.RadioButton HybirdMode;
        
        internal System.Windows.Controls.RadioButton SatelliteMode;
        
        internal System.Windows.Controls.RadioButton StreetMode;
        
        internal Microsoft.Phone.Controls.Maps.Map googlemap;
        
        internal Microsoft.Phone.Controls.Maps.MapTileLayer street;
        
        internal Microsoft.Phone.Controls.Maps.MapTileLayer wateroverlay;
        
        internal Microsoft.Phone.Controls.Maps.MapTileLayer hybrid;
        
        internal Microsoft.Phone.Controls.Maps.MapTileLayer satellite;
        
        internal Microsoft.Phone.Controls.Maps.MapTileLayer physical;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/HomeOnPhone;component/SellHome.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.MapGrid = ((System.Windows.Controls.Grid)(this.FindName("MapGrid")));
            this.LocationTextBox = ((System.Windows.Controls.TextBox)(this.FindName("LocationTextBox")));
            this.HybirdMode = ((System.Windows.Controls.RadioButton)(this.FindName("HybirdMode")));
            this.SatelliteMode = ((System.Windows.Controls.RadioButton)(this.FindName("SatelliteMode")));
            this.StreetMode = ((System.Windows.Controls.RadioButton)(this.FindName("StreetMode")));
            this.googlemap = ((Microsoft.Phone.Controls.Maps.Map)(this.FindName("googlemap")));
            this.street = ((Microsoft.Phone.Controls.Maps.MapTileLayer)(this.FindName("street")));
            this.wateroverlay = ((Microsoft.Phone.Controls.Maps.MapTileLayer)(this.FindName("wateroverlay")));
            this.hybrid = ((Microsoft.Phone.Controls.Maps.MapTileLayer)(this.FindName("hybrid")));
            this.satellite = ((Microsoft.Phone.Controls.Maps.MapTileLayer)(this.FindName("satellite")));
            this.physical = ((Microsoft.Phone.Controls.Maps.MapTileLayer)(this.FindName("physical")));
        }
    }
}

