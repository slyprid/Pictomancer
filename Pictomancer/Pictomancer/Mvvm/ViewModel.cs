using System.Runtime.CompilerServices;
using System.Windows;

namespace Pictomancer.Mvvm
{
    public abstract class ViewModel
        : NotificationObject, IViewModel
    {
        public FrameworkElement Owner { get; set; }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "none")
        {
            OnPropertyChanged(propertyName);
        }

        protected void NotifyPropertyChanging([CallerMemberName] string propertyName = "none")
        {
            OnPropertyChanged(propertyName);
        }
    }
}