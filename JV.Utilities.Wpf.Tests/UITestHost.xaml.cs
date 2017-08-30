using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using JV.Utilities.Wpf.Tests.Elements;

namespace JV.Utilities.Wpf.Tests
{
    public partial class UITestHost : Window
    {
        public UITestHost()
        {
            InitializeComponent();
        }

        private void StretchPanelTest_Click(object sender, RoutedEventArgs e)
        {
            new StretchPanelUITests().ShowDialog();
        }
    }
}
