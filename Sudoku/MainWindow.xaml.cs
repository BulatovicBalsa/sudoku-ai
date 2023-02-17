using Sudoku.Model;
using Sudoku.Utils;
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
        Checkers checkers = new Checkers();
        Fields fields = new Fields();

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            posImage.Visibility = Visibility.Collapsed;
            negImage.Visibility = Visibility.Collapsed;
            fields.Draw(canMain);
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            checkers.InitGame(fields);
            if (!checkers.IsPlayable(fields))
            {
                fields.UnsolveAll();
                posImage.Visibility = Visibility.Collapsed;
                negImage.Visibility = Visibility.Visible;
            }
            else
            {
                posImage.Visibility = Visibility.Visible;
                negImage.Visibility = Visibility.Collapsed;
                startButton.Click -= startButton_Click;
            }
        }

        private void checkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!checkers.IsPlayable(fields))
            {
                posImage.Visibility = Visibility.Collapsed;
                negImage.Visibility = Visibility.Visible;
            }
            else
            {
                posImage.Visibility = Visibility.Visible;
                negImage.Visibility = Visibility.Collapsed;
            }
        }
    }
}
