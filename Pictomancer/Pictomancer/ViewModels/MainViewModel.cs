using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Pictomancer.Mvvm;
using Pictomancer.Views;
using Relm.Maps;
using Relm.Pipeline.Models;

namespace Pictomancer.ViewModels
{
    public class MainViewModel
        : ViewModel
    {
        public bool IsDirty { get; set; }
        
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

        #endregion

        #region Command Properties

        public Command NewProjectCommand { get; set; }
        public Command OpenProjectCommand { get; set; }
        public Command SaveProjectCommand { get; set; }
        public Command NewMapCommand { get; set; }
        public Command DeleteMapCommand { get; set; }

        #endregion

        #region Initialization

        public MainViewModel()
        {
            Project = new ProjectViewModel();
            Pages = new ObservableCollection<PageViewModel>
            {
                new StartViewModel()
            };
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            NewProjectCommand = new Command(NewProject);
            OpenProjectCommand = new Command(OpenProject);
            SaveProjectCommand = new Command(SaveProject);
            NewMapCommand = new Command(NewMap);
            DeleteMapCommand = new Command(DeleteMap);
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
                Name = results.ProjectName
            };
            NewMap();
            SetIsDirty(true);
        }

        public void OpenProject()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Pictomancer Files (*.pictomancer)|*.pictomancer|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (!dialog.ShowDialog((MainWindow)Owner).GetValueOrDefault()) return;

            
        }

        public void SaveProject()
        {
            if (string.IsNullOrEmpty(Project.Path))
            {
                SaveNewProject();
                return;
            }

            SetIsDirty(false);
        }

        public void SaveNewProject()
        {
            var dialog = new SaveFileDialog
            {
                FileName = $"{Project.Name.Replace(" ", "_")}.pictomancer",
                Filter = "Pictomancer Files (*.pictomancer)|*.pictomancer|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (!dialog.ShowDialog((MainWindow)Owner).GetValueOrDefault()) return;

            SetIsDirty(false);
        }

        public void NewMap()
        {
            if (Project.Name == "Empty Project")
            {
                MessageBox.Show("Unable to create new map because there is no project", "Pictomancer");
                return;
            }

            var map = Project.CreateNewMap();
            var vm = new MapViewModel(map);
            Pages.Add(vm);
            SelectedPage = vm;
        }

        public void DeleteMap()
        {
            if (Project.Name == "Empty Project")
            {
                MessageBox.Show("Unable to delete map because there is no project", "Pictomancer");
                return;
            }

            if (Project.SelectedItem.GetType() != typeof(Map))
            {
                MessageBox.Show("Unable to delete map because no map selected", "Pictomancer");
                return;
            }

            var map = (Map) Project.SelectedItem;
            var page = Pages.OfType<MapViewModel>().SingleOrDefault(x => x.Id == map.Id);
            Project.DeleteMap(map);
            if (page != null)
            {
                Pages.Remove(page);
            }
        }

        #endregion

        public void SetIsDirty(bool value)
        {
            IsDirty = value;
            var flag = IsDirty ? "* " : "";
            ((MainWindow)Owner).Title = $"Pictomancer - {flag}[ {Project.Name} ]";
        }
    }
}