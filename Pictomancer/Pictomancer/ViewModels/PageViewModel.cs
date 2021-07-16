using System.Windows;
using Pictomancer.Mvvm;

namespace Pictomancer.ViewModels
{
    public class PageViewModel
        : ViewModel
    {
        public string Title { get; set; }
        public string Header { get; set; }
        public FrameworkElement Content { get; set; }
    }
}