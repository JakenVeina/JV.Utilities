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

namespace JV.Utilities.Wpf.Tests.Elements
{
    public partial class StretchPanelUITests : Window
    {
        public StretchPanelUITests()
        {
            InitializeComponent();
        }

        private void Vertical_Click(object sender, RoutedEventArgs e)
            => UUT.Orientation = Orientation.Vertical;

        private void Horizontal_Click(object sender, RoutedEventArgs e)
            => UUT.Orientation = Orientation.Horizontal;
    }
}
