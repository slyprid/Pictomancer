using System;
using System.Collections.ObjectModel;
using System.Windows;
using Pictomancer.Mvvm;
using Relm.Maps;

namespace Pictomancer.ViewModels
{
    public class ProjectViewModel
        : ViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public string Title => $"Project '{Name}'";

        #region Dependency Properties

        public static readonly DependencyProperty MapsProperty = DependencyProperty.Register("Maps", typeof(ObservableCollection<Map>), typeof(ProjectViewModel), new PropertyMetadata(default(ObservableCollection<Map>)));
        public ObservableCollection<Map> Maps
        {
            get => (ObservableCollection<Map>) GetValue(MapsProperty);
            set => SetValue(MapsProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ProjectViewModel), new PropertyMetadata(default(object)));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        #endregion

        public ProjectViewModel()
        {
            Name = "Empty Project";
            Maps = new ObservableCollection<Map>();
        }

        public Map CreateNewMap()
        {
            var index = Maps.Count + 1;
            var name = $"Map {index}";
            var map = new Map
            {
                Name = name
            };
            Maps.Add(map);
            return map;
        }

        public void DeleteMap(Map selectedMap)
        {
            Maps.Remove(selectedMap);
        }
    }
}