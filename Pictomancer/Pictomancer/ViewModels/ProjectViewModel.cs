using System.Collections.ObjectModel;
using System.Windows;
using Pictomancer.Mvvm;
using Relm.Maps;
using Relm.Tiles;

namespace Pictomancer.ViewModels
{
    public class ProjectViewModel
        : ViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public string Title => $"Project '{Name}'";

        public MainViewModel MainViewModel { get; set; }
        
        #region Dependency Properties

        public static readonly DependencyProperty MapsProperty = DependencyProperty.Register("Maps", typeof(ObservableCollection<Map>), typeof(ProjectViewModel), new PropertyMetadata(default(ObservableCollection<Map>)));
        public ObservableCollection<Map> Maps
        {
            get => (ObservableCollection<Map>) GetValue(MapsProperty);
            set => SetValue(MapsProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ProjectViewModel), new PropertyMetadata(default(object), OnSelectedItemChanged));
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty TilesetsProperty = DependencyProperty.Register("Tilesets", typeof(ObservableCollection<Tileset>), typeof(ProjectViewModel), new PropertyMetadata(default(ObservableCollection<Tileset>)));
        public ObservableCollection<Tileset> Tilesets
        {
            get => (ObservableCollection<Tileset>) GetValue(TilesetsProperty);
            set => SetValue(TilesetsProperty, value);
        }

        #endregion

        public ProjectViewModel()
        {
            Name = "Empty Project";
            Maps = new ObservableCollection<Map>();
            Tilesets = new ObservableCollection<Tileset>();
        }

        public Map CreateNewMap()
        {
            var index = Maps.Count + 1;
            var name = $"Map {index}";
            var map = new Map
            {
                Name = name
            };
            map.AddLayer<TileLayer>().Name = "Tile Layer 1";
            Maps.Add(map);
            return map;
        }

        public void DeleteMap(Map selectedMap)
        {
            Maps.Remove(selectedMap);
        }

        public void OpenProject(string filename)
        {

        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var project = (ProjectViewModel) d;

            if (e.NewValue.GetType() == typeof(Tileset))
            {
                project.MainViewModel.TilesViewModel.Tileset = (Tileset) e.NewValue;
            }
        }
    }
}