using System.Windows;
using Pictomancer.Mvvm;
using Pictomancer.Views;

namespace Pictomancer.ViewModels
{
    public class NewProjectViewModel
        : ViewModel
    {
        public string ProjectName { get; set; }

        #region Command Properties

        public Command CreateProjectCommand { get; set; }
        public Command CancelCommand { get; set; }

        #endregion

        #region Initialization

        public NewProjectViewModel()
        {
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