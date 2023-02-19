using Sudoku.Model;
using Sudoku.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            fw1Img.Visibility = Visibility.Collapsed;
            fw2Img.Visibility = Visibility.Collapsed;
            fw3Img.Visibility = Visibility.Collapsed;
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
                checkers.SetCandidates(fields);
                posImage.Visibility = Visibility.Visible;
                negImage.Visibility = Visibility.Collapsed;
            }
            if (checkers.GameOver(fields))
            {
                checkButton.Click -= checkButton_Click;
                DispatcherTimer timerFireworks = new DispatcherTimer();
                timerFireworks.Tick += TimerFireworks_Tick;
                timerFireworks.Interval = TimeSpan.FromMilliseconds(100);
                timerFireworks.Start();
            }
        }

        private void TimerFireworks_Tick(object? sender, EventArgs e)
        {
            Random r = new Random();
            posImage.Visibility = Visibility.Collapsed;
            negImage.Visibility = Visibility.Collapsed;
            fw1Img.Visibility = Visibility.Collapsed;
            fw2Img.Visibility = Visibility.Collapsed;
            fw3Img.Visibility = Visibility.Collapsed;
            switch (r.Next(1, 4))
            {
                case 1:
                    fw1Img.Visibility = Visibility.Visible;
                    break;
                case 2:
                    fw2Img.Visibility = Visibility.Visible;
                    break;
                case 3:
                    fw3Img.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new Exception("Unreachable");
            }
        }
    }
}
