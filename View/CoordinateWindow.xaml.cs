using CoordinateMap.Model;
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
using System.Windows.Shapes;

namespace CoordinateMap.View
{
    /// <summary>
    /// Interaction logic for CoordinateWindow.xaml
    /// </summary>
    public partial class CoordinateWindow : Window
    {
        public Coordinates Coordinates { get; private set; }
        public CoordinateWindow(Coordinates coordinates)
        {
            InitializeComponent();
            Coordinates = coordinates;
            DataContext = Coordinates;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
