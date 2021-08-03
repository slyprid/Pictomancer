using System;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ILogToUi
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Log(string message)
        {
            var now = DateTime.Now.TimeOfDay;
            LogOutput.AppendText($"{now}: {message}{Environment.NewLine}");
            LogOutput.ScrollToEnd();
        }
    }
}
