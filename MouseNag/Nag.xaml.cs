using System;
using System.Windows;

namespace MouseNag
{
    /// <summary>
    /// Interaction logic for Annoy.xaml
    /// </summary>
    public partial class Annoy : Window
    {
        public Annoy()
        {
            InitializeComponent();
            
            Topmost = true;
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

            RandomlyPlaceCloseButton();
        }

        void RandomlyPlaceCloseButton()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            if(random.Next(0, 2) == 1)
                closeButton.VerticalAlignment = VerticalAlignment.Top;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
