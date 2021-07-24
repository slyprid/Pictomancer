using System;
using System.Windows;
using Microsoft.Win32;
using Microsoft.Xna.Framework.Graphics;
using Pictomancer.Mvvm;
using Pictomancer.Views;
using Relm.Extensions;

namespace Pictomancer.ViewModels
{
    public class AddTilesetViewModel
        : ViewModel
    {
        #region Dependency Properties

        public static readonly DependencyProperty FilenameProperty = DependencyProperty.Register("Filename", typeof(string), typeof(AddTilesetViewModel), new PropertyMetadata(default(string)));
        public string Filename
        {
            get => (string) GetValue(FilenameProperty);
            set => SetValue(FilenameProperty, value);
        }

        public static readonly DependencyProperty TileWidthProperty = DependencyProperty.Register("TileWidth", typeof(string), typeof(AddTilesetViewModel), new PropertyMetadata(default(string)));
        public string TileWidth
        {
            get => (string) GetValue(TileWidthProperty);
            set => SetValue(TileWidthProperty, value);
        }

        public static readonly DependencyProperty TileHeightProperty = DependencyProperty.Register("TileHeight", typeof(string), typeof(AddTilesetViewModel), new PropertyMetadata(default(string)));
        public string TileHeight
        {
            get => (string) GetValue(TileHeightProperty);
            set => SetValue(TileHeightProperty, value);
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(AddTilesetViewModel), new PropertyMetadata(default(string)));
        public string Name
        {
            get => (string) GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        #endregion

        #region Command Properties

        public Command BrowseCommand { get; set; }
        public Command AddTilesetCommand { get; set; }
        public Command CancelCommand { get; set; }

        #endregion

        #region Initialization

        public AddTilesetViewModel()
        {
            TileWidth = "32";
            TileHeight = "32";
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            BrowseCommand = new Command(Browse);
            AddTilesetCommand = new Command(AddTileset);
            CancelCommand = new Command(Cancel);
        }

        #endregion

        #region Commands

        public void Browse()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files (*.gif, *.jpg, *.png, *.tif, *.dds)|*.gif;*.jpg;*.png;*.tif;*.dds|All files (*.*)|*.*"
            };

            if (!dialog.ShowDialog((MainWindow)((AddTilesetView)Owner).Owner).GetValueOrDefault()) return;

            Filename = dialog.FileName;
        }

        public void AddTileset()
        {
            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("Tileset must have name.", "Pictomancer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(Filename))
            {
                MessageBox.Show("Must choose an image file to load as tileset.", "Pictomancer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!TileWidth.IsNumeric())
            {
                MessageBox.Show("Tile Width must be a number", "Pictomancer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!TileHeight.IsNumeric())
            {
                MessageBox.Show("Tile Height must be a number", "Pictomancer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (int.Parse(TileWidth) <= 0)
            {
                MessageBox.Show("Tile Width must be greater than 0", "Pictomancer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (int.Parse(TileHeight) <= 0)
            {
                MessageBox.Show("Tile Height must be greater than 0", "Pictomancer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var view = (AddTilesetView)Owner;
            view.DialogResult = true;
        }

        public void Cancel()
        {
            var view = (AddTilesetView)Owner;
            view.DialogResult = false;
        }

        #endregion
    }
}