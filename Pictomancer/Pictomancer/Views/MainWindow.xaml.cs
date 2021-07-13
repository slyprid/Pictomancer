using Pictomancer.Mvvm;
using Pictomancer.ViewModels;

namespace Pictomancer.Views
{
    public partial class MainWindow 
        : RibbonView<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
