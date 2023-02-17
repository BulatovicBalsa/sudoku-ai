using Sudoku.Model;
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

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            posImage.Visibility = Visibility.Collapsed;
            negImage.Visibility = Visibility.Collapsed;
            Fields fields = new Fields();
            fields.Draw(canMain);
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void checkButton_Click(object sender, RoutedEventArgs e)
        {
            if (negImage.Visibility == Visibility.Collapsed)
                negImage.Visibility = Visibility.Visible;
            else if (negImage.Visibility == Visibility.Visible) 
                negImage.Visibility = Visibility.Collapsed;
        }
    }
}
