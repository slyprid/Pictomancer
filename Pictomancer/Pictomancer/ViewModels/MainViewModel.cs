using System;
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
            Project.CreateNewMap();
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

            Project.CreateNewMap();
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

            Project.DeleteMap((Map)Project.SelectedItem);
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