using System.Windows;
using System.Windows.Controls;
using Pictomancer.ViewModels;

namespace Pictomancer.Selectors
{
    public class TabItemTemplateSelector
        : DataTemplateSelector
    {
        public DataTemplate MapViewTemplate { get; set; }
        public DataTemplate StartViewTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tabItem = item as PageViewModel;
            if (tabItem?.GetType() == typeof(MapViewModel)) return MapViewTemplate;
            if (tabItem?.GetType() == typeof(StartViewModel)) return StartViewTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}