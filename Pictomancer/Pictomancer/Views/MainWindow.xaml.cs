using System.Windows;

namespace Pictomancer.Views
{
    public partial class MainWindow 
    {
        public MainWindow()
        {
            App.MainWindowInstance = this;
            InitializeComponent();
        }

        private void Tools_OnChecked(object sender, RoutedEventArgs e)
        {
            var a = 0;
        }
    }
}