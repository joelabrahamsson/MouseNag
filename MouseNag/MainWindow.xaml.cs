using System.Diagnostics;
using System.Windows;
using MouseNag.InputMonitoring;
using MouseNag.Presentation;

namespace MouseNag
{
    public partial class MainWindow : Window
    {
        private MouseNagPresenter presenter;

        public MainWindow()
        {
            InitializeComponent();

            presenter = new MouseNagPresenter(
                new TimerBasedInputSource(),
                new WindowNagger());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
