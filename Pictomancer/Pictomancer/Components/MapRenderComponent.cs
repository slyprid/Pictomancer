using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using Pictomancer.Models;
using Pictomancer.ViewModels;

namespace Pictomancer.Components
{
    public class MapRenderComponent
        : WpfDrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private readonly WpfKeyboard _keyboard;
        private readonly WpfMouse _mouse;
        private readonly InputModel _input;

        public MapViewModel ViewModel { get; set; }

        public MapRenderComponent(WpfGame game) 
            : base(game)
        {
            _keyboard = new WpfKeyboard(game);
            _mouse = new WpfMouse(game);

            _input = new InputModel();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _input.PreviousKeyboardState = _input.CurrentKeyboardState;
            _input.PreviousMouseState = _input.CurrentMouseState;
            _input.CurrentKeyboardState = _keyboard.GetState();
            _input.CurrentMouseState = _mouse.GetState();

            ViewModel?.LoadContent(Game.Content);
            ViewModel?.Update(gameTime, _input);

            if (_input.IsMousePressed(MouseButton.Left) || _input.IsMouseHeld(MouseButton.Left))
            {
                ViewModel?.DrawPrimaryTile();
            }

            if (_input.IsMousePressed(MouseButton.Right) || _input.IsMouseHeld(MouseButton.Right))
            {
                ViewModel?.DrawSecondaryTile();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ViewModel?.Color ?? Color.CornflowerBlue);
            ViewModel?.Draw(gameTime, _spriteBatch);
            base.Draw(gameTime);
        }
    }
}