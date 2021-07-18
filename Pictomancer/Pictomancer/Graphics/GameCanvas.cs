using System.Windows;
using System.Windows.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using Pictomancer.Components;
using Pictomancer.ViewModels;

namespace Pictomancer.Graphics
{
    public class GameCanvas 
        : WpfGame
    {
        // ReSharper disable once NotAccessedField.Local
        private IGraphicsDeviceService _graphicsDeviceManager;
        
        private MainViewModel _mainViewModel;
        private MapRenderComponent _mapRenderer;

        public static readonly DependencyProperty ControlViewModelProperty = DependencyProperty.Register("ControlViewModel", typeof(GameCanvasControlViewModel), typeof(GameCanvas), new PropertyMetadata(default(GameCanvasControlViewModel)));
        
        public GameCanvasControlViewModel ControlViewModel
        {
            get => (GameCanvasControlViewModel) GetValue(ControlViewModelProperty);
            set => SetValue(ControlViewModelProperty, value);
        }

        protected override void Initialize()
        {
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);
            base.Initialize();

            ControlViewModel = (GameCanvasControlViewModel) ((ContentPresenter) TemplatedParent).Content;
            
            _mapRenderer = new MapRenderComponent(this);
            Components.Add(_mapRenderer);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_mainViewModel == null)
            {
                var vm = (MainViewModel) ((TabControl) ((ContentPresenter) TemplatedParent).TemplatedParent)?.DataContext;
                _mainViewModel = vm;
            }

            if (_mainViewModel != null)
            {
                var page = _mainViewModel.SelectedPage;
                if (page.GetType() == typeof(MapViewModel))
                {
                    _mapRenderer.ViewModel = ((MapViewModel) page);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}