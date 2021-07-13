using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Pictomancer.Mvvm.Annotations;

namespace Pictomancer.Mvvm
{
    public class NotificationObject
        : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}