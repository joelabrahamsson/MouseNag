using MouseNag.Presentation;

namespace MouseNag
{
    public class WindowNagger : INag
    {
        public void Nag()
        {
            var annoyWindow = new Annoy();
            annoyWindow.ShowDialog();
        }
    }
}
