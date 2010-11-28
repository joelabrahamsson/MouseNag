using System;
using System.Collections.Generic;
using System.Windows;

namespace MouseNag
{
    /// <summary>
    /// Interaction logic for Annoy.xaml
    /// </summary>
    public partial class Annoy : Window
    {
        Random random;

        public Annoy()
        {
            InitializeComponent();

            Topmost = true;
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            closeButton.Focus();
            RandomlyPlaceCloseButton();
        }

        void RandomlyPlaceCloseButton()
        {
            random = new Random(DateTime.Now.Millisecond);
            closeButton.VerticalAlignment = GetRandomVerticalAlignment();

            closeButton.HorizontalAlignment = GetRandomHorizontalAlignment();
        }

        private VerticalAlignment GetRandomVerticalAlignment()
        {
            var verticalAlignments = new Dictionary<int, VerticalAlignment>
                                           {
                                               {0, VerticalAlignment.Top},
                                               {1, VerticalAlignment.Bottom},
                                           };
            return verticalAlignments[random.Next(0, verticalAlignments.Count)];
        }

        private HorizontalAlignment GetRandomHorizontalAlignment()
        {
            var horizontalAlignments = new Dictionary<int, HorizontalAlignment>
                                           {
                                               {0, HorizontalAlignment.Left},
                                               {1, HorizontalAlignment.Center},
                                               {2, HorizontalAlignment.Right}
                                           };

            return horizontalAlignments[random.Next(0, horizontalAlignments.Count)];
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
