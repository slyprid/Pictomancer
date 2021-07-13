using System.Windows;

namespace Pictomancer.Mvvm
{
    public interface IViewModel
    {
        FrameworkElement Owner { get; set; }
    }
}