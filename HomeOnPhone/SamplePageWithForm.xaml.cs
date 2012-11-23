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
using HomeOnPhone.ViewModels;
using Microsoft.Phone.Controls;

namespace HomeOnPhone
{
    public partial class SamplePageWithForm : PhoneApplicationPage
    {
        private readonly SamplePageWithFormViewModel viewModel = new SamplePageWithFormViewModel();

        public SamplePageWithForm()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ContentPanel.DataContext = viewModel;
        }
    }
}