using Pictomancer.Mvvm;
using Pictomancer.Views;
using Relm.Pipeline.Models;

namespace Pictomancer.ViewModels
{
    public class MainViewModel
        : ViewModel
    {
        public PictomancerProject Project { get; set; }

        #region Command Properties

        public Command NewProjectCommand { get; set; }

        #endregion

        #region Initialization

        public MainViewModel()
        {
            Project = new PictomancerProject();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            NewProjectCommand = new Command(NewProject);
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
            Project = new PictomancerProject
            {
                Name = results.ProjectName
            };
            ((MainWindow) Owner).Title = $"Pictomancer - * [ {Project.Name} ]";
        }

        #endregion
    }
}