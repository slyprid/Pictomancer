using System.Windows;
using System.Windows.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using Pictomancer.Components;
using Pictomancer.Models;
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
        private SpriteBatch _spriteBatch;
        private InputModel _input;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;

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
            //Components.Add(_mapRenderer);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this); // { CaptureMouseWithin = false };

            _input = new InputModel();
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateInput();

            if (_mainViewModel == null)
            {
                var vm = (MainViewModel) ((TabControl) ((ContentPresenter) TemplatedParent).TemplatedParent)?.DataContext;
                _mainViewModel = vm;
            }

            if (_mainViewModel != null)
            {
                _mainViewModel.GraphicsDevice = GraphicsDevice;
                var page = _mainViewModel.SelectedPage;
                if (page.GetType() == typeof(MapViewModel))
                {
                    var vm = ((MapViewModel)page);
                    _mapRenderer.ViewModel = vm;
                    vm.LoadContent(Content);
                    vm.Update(gameTime, _input);
                }
            }

            base.Update(gameTime);
        }

        private void UpdateInput()
        {
            _input.Update(_keyboard, _mouse);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (!IsActive) return;

            var page = _mainViewModel.SelectedPage;
            var vm = ((MapViewModel) page);
            vm.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}