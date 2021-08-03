using System.Windows;
using Pictomancer.Mvvm;
using Pictomancer.Views;

namespace Pictomancer.ViewModels
{
    public class NewProjectViewModel
        : ViewModel
    {
        public string ProjectName { get; set; }

        #region Dependency Properties

        public static readonly DependencyProperty MapWidthProperty = DependencyProperty.Register("MapWidth", typeof(int), typeof(NewProjectViewModel), new PropertyMetadata(default(int)));
        public int MapWidth
        {
            get => (int) GetValue(MapWidthProperty);
            set => SetValue(MapWidthProperty, value);
        }

        public static readonly DependencyProperty MapHeightProperty = DependencyProperty.Register("MapHeight", typeof(int), typeof(NewProjectViewModel), new PropertyMetadata(default(int)));
        public int MapHeight
        {
            get => (int) GetValue(MapHeightProperty);
            set => SetValue(MapHeightProperty, value);
        }

        #endregion

        #region Command Properties

        public Command CreateProjectCommand { get; set; }
        public Command CancelCommand { get; set; }

        #endregion

        #region Initialization

        public NewProjectViewModel()
        {
            MapWidth = 25;
            MapHeight = 25;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateProjectCommand = new Command(CreateProject);
            CancelCommand = new Command(Cancel);
        }

        #endregion

        #region Commands

        public void CreateProject()
        {
            if (string.IsNullOrEmpty(ProjectName))
            {
                System.Windows.MessageBox.Show("Project Name cannot be empty.", "Pictomancer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var view = (NewProjectView)Owner;
            view.DialogResult = true;
        }

        public void Cancel()
        {
            var view = (NewProjectView) Owner;
            view.DialogResult = false;
        }

        #endregion
    }
}