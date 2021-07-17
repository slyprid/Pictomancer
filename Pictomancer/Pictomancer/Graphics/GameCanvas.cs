using System.Windows;
using System.Windows.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using Pictomancer.Components;
using Pictomancer.ViewModels;

namespace Pictomancer.Graphics
{
    public class GameCanvas 
        : WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceManager;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;
        private bool _disposed;
        
        private MainViewModel _mainViewModel;
        private TextComponent _text;
        private MapRenderComponent _mapRenderer;

        public static readonly DependencyProperty ControlViewModelProperty = DependencyProperty.Register("ControlViewModel", typeof(GameCanvasControlViewModel), typeof(GameCanvas), new PropertyMetadata(default(GameCanvasControlViewModel)));
        
        public GameCanvasControlViewModel ControlViewModel
        {
            get => (GameCanvasControlViewModel) GetValue(ControlViewModelProperty);
            set => SetValue(ControlViewModelProperty, value);
        }

        protected override void Initialize()
        {
            // must be initialized. required by Content loading and rendering (will add itself to the Services)
            // note that MonoGame requires this to be initialized in the constructor, while WpfInterop requires it to
            // be called inside Initialize (before base.Initialize())
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            // wpf and keyboard need reference to the host control in order to receive input
            // this means every WpfGame control will have it's own keyboard & mouse manager which will only react if the mouse is in the control
            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);

            // must be called after the WpfGraphicsDeviceService instance was created
            base.Initialize();

            // content loading now possible

            ControlViewModel = (GameCanvasControlViewModel) ((ContentPresenter) TemplatedParent).Content;
            ControlViewModel.Canvas = this;

            _mapRenderer = new MapRenderComponent(this);
            Components.Add(_mapRenderer);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            base.Dispose(disposing);
            Services.RemoveService(typeof(IGraphicsDeviceService));
        }

        protected override void Update(GameTime gameTime)
        {
            // every update we can now query the keyboard & mouse for our WpfGame
            var mouseState = _mouse.GetState();
            var keyboardState = _keyboard.GetState();

            if (_mainViewModel == null)
            {
                var vm = (MainViewModel) ((TabControl) ((ContentPresenter) TemplatedParent).TemplatedParent)?.DataContext;
                _mainViewModel = vm;
            }

            var page = _mainViewModel.SelectedPage;
            if (page.GetType() == typeof(MapViewModel))
            {
                _mapRenderer.ViewModel = ((MapViewModel) page);
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