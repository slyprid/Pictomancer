using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using MonoGame.Extended.TextureAtlases;
using Pictomancer.Mvvm;
using Pictomancer.Views;
using Relm.Helpers;
using Relm.Maps;
using Relm.Tiles;

namespace Pictomancer.ViewModels
{
    public class MainViewModel
        : ViewModel
    {
        public bool IsDirty { get; set; }

        public MapViewModel SelectedMap
        {
            get
            {
                if (SelectedPage == null) return null;
                if (SelectedPage.GetType() == typeof(MapViewModel))
                {
                    return (MapViewModel)SelectedPage;
                }

                return null;
            }
        }

        #region Dependency Properties

        public static readonly DependencyProperty ProjectProperty = DependencyProperty.Register("Project", typeof(ProjectViewModel), typeof(MainViewModel), new PropertyMetadata(default(ProjectViewModel)));
        public ProjectViewModel Project
        {
            get => (ProjectViewModel) GetValue(ProjectProperty);
            set => SetValue(ProjectProperty, value);
        }

        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register("Pages", typeof(ObservableCollection<PageViewModel>), typeof(PageViewModel), new PropertyMetadata(default(ObservableCollection<PageViewModel>)));
        public ObservableCollection<PageViewModel> Pages
        {
            get => (ObservableCollection<PageViewModel>) GetValue(PagesProperty);
            set => SetValue(PagesProperty, value);
        }

        public static readonly DependencyProperty SelectedPageProperty = DependencyProperty.Register("SelectedPage", typeof(PageViewModel), typeof(MainViewModel), new PropertyMetadata(default(PageViewModel)));
        public PageViewModel SelectedPage
        {
            get => (PageViewModel) GetValue(SelectedPageProperty);
            set => SetValue(SelectedPageProperty, value);
        }

        public static readonly DependencyProperty TilesViewModelProperty = DependencyProperty.Register("TilesViewModel", typeof(TilesViewModel), typeof(MainViewModel), new PropertyMetadata(default(TilesViewModel)));
        public TilesViewModel TilesViewModel
        {
            get => (TilesViewModel) GetValue(TilesViewModelProperty);
            set => SetValue(TilesViewModelProperty, value);
        }

        public static readonly DependencyProperty ConsoleLogProperty = DependencyProperty.Register("ConsoleLog", typeof(string), typeof(MainViewModel), new PropertyMetadata(default(string)));
        public string ConsoleLog
        {
            get => (string) GetValue(ConsoleLogProperty);
            set => SetValue(ConsoleLogProperty, value);
        }

        #endregion

        #region Command Properties

        public Command NewProjectCommand { get; set; }
        public Command OpenProjectCommand { get; set; }
        public Command SaveProjectCommand { get; set; }
        public Command NewMapCommand { get; set; }
        public Command DeleteMapCommand { get; set; }
        public Command AddTilesetCommand { get; set; }

        #endregion

        #region Initialization

        public MainViewModel()
        {
            Project = new ProjectViewModel
            {
                MainViewModel = this
            };
            TilesViewModel = new TilesViewModel
            {
                Project = Project
            };
            Pages = new ObservableCollection<PageViewModel>
            {
                new StartViewModel()
            };

            App.MainViewModel = this;

            InitializeCommands();

            App.Log("Pictomancer loaded");
        }

        private void InitializeCommands()
        {
            NewProjectCommand = new Command(NewProject);
            OpenProjectCommand = new Command(OpenProject);
            SaveProjectCommand = new Command(SaveProject);
            NewMapCommand = new Command(NewMap);
            DeleteMapCommand = new Command(DeleteMap);
            AddTilesetCommand = new Command(AddTileset);
        }

        #endregion

        #region Commands

        public void NewProject()
        {
            var view = new NewProjectView
            {
                Owner = (MainWindow)Owner
            };
            
            if (!view.ShowDialog().GetValueOrDefault()) return;

            var results = view.ViewModel;
            Project = new ProjectViewModel
            {
                Name = results.ProjectName,
                MainViewModel = this
            };
            App.Log($"New Project [{Project.Name}] created");
            NewMap();
            SetIsDirty(true);
        }

        public void OpenProject()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Pictomancer Files (*.pictomancer)|*.pictomancer|All files (*.*)|*.*"
            };

            if (!dialog.ShowDialog((MainWindow)Owner).GetValueOrDefault()) return;

            Project.OpenProject(dialog.FileName);

            App.Log($"Opening Project from [{dialog.FileName}]");
        }

        public void SaveProject()
        {
            if (string.IsNullOrEmpty(Project.Path))
            {
                SaveNewProject();
                return;
            }

            SetIsDirty(false);

            App.Log($"Project Saved");
        }

        public void SaveNewProject()
        {
            var dialog = new SaveFileDialog
            {
                FileName = $"{Project.Name.Replace(" ", "_")}.pictomancer",
                Filter = "Pictomancer Files (*.pictomancer)|*.pictomancer|All files (*.*)|*.*"
            };

            if (!dialog.ShowDialog((MainWindow)Owner).GetValueOrDefault()) return;

            SetIsDirty(false);
            App.Log($"Project Saved");
        }

        public void NewMap()
        {
            if (CheckForEmptyProject()) return;

            var map = Project.CreateNewMap();
            var vm = new MapViewModel(map);
            Pages.Add(vm);
            SelectedPage = vm;
            vm.Project = Project;

            App.Log($"New map created [{map.Name}]");
        }

        public void DeleteMap()
        {
            if (CheckForEmptyProject()) return;
            if (CheckForNoMapSelected()) return;

            var map = (Map) Project.SelectedItem;
            var page = Pages.OfType<MapViewModel>().SingleOrDefault(x => x.Id == map.Id);
            Project.DeleteMap(map);
            if (page != null)
            {
                Pages.Remove(page);
            }

            App.Log($"Map [{map.Name}] deleted");
        }

        public void AddTileset()
        {
            if (CheckForEmptyProject()) return;

            var view = new AddTilesetView
            {
                Owner = (MainWindow)Owner
            };

            if (!view.ShowDialog().GetValueOrDefault()) return;

            var results = view.ViewModel;

            var texture = ContentHelper.LoadTextureFromFile(TilesViewModel.GraphicsDevice, results.Filename);
            var tileWidth = int.Parse(results.TileWidth);
            var tileHeight = int.Parse(results.TileHeight);
            var regions = GraphicsHelper.CreateTextureAtlasRegions(texture.Width, texture.Height, tileWidth, tileHeight);

            var tileset = new Tileset
            {
                Name = results.Name,
                TileWidth = tileWidth,
                TileHeight = tileHeight,
                TextureAtlas = new TextureAtlas(results.Name, texture, regions)
            };

            Project.Tilesets.Add(tileset);
            TilesViewModel.Tileset = tileset;
            TilesViewModel.PrimarySelected = "0-0";
            TilesViewModel.SecondarySelected = "1-0";
            TilesViewModel.PrimarySelectedTile = new Tile();
            TilesViewModel.SecondarySelectedTile = new Tile();
            

            SetIsDirty(true);

            App.Log($"New Tileset Added [{results.Name}] - Tile Width/Height: [{results.TileWidth}px/{results.TileHeight}px]");
        }

        #endregion

        public void SetIsDirty(bool value)
        {
            IsDirty = value;
            var flag = IsDirty ? "* " : "";
            ((MainWindow)Owner).Title = $"Pictomancer - {flag}[ {Project.Name} ]";
        }

        public bool CheckForEmptyProject()
        {
            if (Project.Name != "Empty Project") return false;
            MessageBox.Show("Unable to delete map because there is no project", "Pictomancer");
            App.Log($"ERROR: Unable to delete map because there is no project");
            return true;
        }

        public bool CheckForNoMapSelected()
        {
            if (Project.SelectedItem.GetType() == typeof(Map)) return false;
            MessageBox.Show("Unable to delete map because no map selected", "Pictomancer");
            App.Log($"ERROR: Unable to delete map because no map selected");
            return true;
        }
    }
}