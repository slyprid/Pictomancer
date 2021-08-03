using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Xna.Framework;
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

        public static readonly DependencyProperty SelectedLayerProperty = DependencyProperty.Register("SelectedLayer", typeof(MapLayer), typeof(ProjectViewModel), new PropertyMetadata(default(MapLayer)));
        public MapLayer SelectedLayer
        {
            get => (MapLayer) GetValue(SelectedLayerProperty);
            set => SetValue(SelectedLayerProperty, value);
        }

        public static readonly DependencyProperty MapWidthProperty = DependencyProperty.Register("MapWidth", typeof(int), typeof(ProjectViewModel), new PropertyMetadata(default(int)));
        public int MapWidth
        {
            get => (int) GetValue(MapWidthProperty);
            set => SetValue(MapWidthProperty, value);
        }

        public static readonly DependencyProperty MapHeightProperty = DependencyProperty.Register("MapHeight", typeof(int), typeof(ProjectViewModel), new PropertyMetadata(default(int)));
        public int MapHeight
        {
            get => (int) GetValue(MapHeightProperty);
            set => SetValue(MapHeightProperty, value);
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
                Name = name,
                Size = new Vector2(MapWidth, MapHeight)
            };
            map.AddLayer<TileLayer>().Name = "Ground";
            map.AddLayer<TileLayer>().Name = "Detail";
            map.AddLayer<TileLayer>().Name = "Overlay";

            SelectedItem = map.Layers[0];
            SelectedLayer = map.Layers[0];

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
            else if (e.NewValue.GetType().BaseType == typeof(MapLayer))
            {
                project.SelectedLayer = (MapLayer) e.NewValue;
            }
        }
    }
}