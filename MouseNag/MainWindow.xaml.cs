using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MouseNag
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MousePosition mousePosition;
        private DispatcherTimer dispatcherTimer;
        private Point lastCursorPosition;
        private MouseNagPresenter presenter;

        public MainWindow()
        {
            InitializeComponent();

            presenter = new MouseNagPresenter(
                new TimerBasedInputSource(new SystemInput(), new Timer()), 
                new WindowNagger());

            //mousePosition = new MousePosition();
            //lastCursorPosition = mousePosition.GetCoordinates();
            
            //SetupTimer();
        }

        public class WindowNagger : INag
        {
            public void Nag()
            {
                byte[] keyStates = new byte[256];
                int retVal = GetKeyboardState(keyStates);
                var annoyWindow = new Annoy();
                annoyWindow.ShowDialog();
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern int GetKeyboardState(byte[] keystate);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //private void SetupTimer()
        //{
        //    dispatcherTimer = new DispatcherTimer();
        //    dispatcherTimer.Interval = 1000.Milliseconds();
        //    dispatcherTimer.Tick += TimerTick;
        //    dispatcherTimer.Start();
        //}

        //private void TimerTick(object sender, EventArgs e)
        //{
        //    var currentCursorPosition = mousePosition.GetCoordinates();
        //    if(currentCursorPosition != lastCursorPosition)
        //    {
        //        Annoy();
        //    }
        //    lastCursorPosition = currentCursorPosition;
        //}

        //private void Annoy()
        //{
        //    var annoyWindow = new Annoy();
        //    annoyWindow.Topmost = true;
        //    annoyWindow.ShowDialog();
        //    //MessageBox.Show("Moving the mouse again, eh?");
        //}
    }

    public static class IntegerTimeSpanExtensions
    {
        public static TimeSpan Milliseconds(this int number)
        {
            return new TimeSpan(0, 0, 0, 0, number);
        }
    }
}
