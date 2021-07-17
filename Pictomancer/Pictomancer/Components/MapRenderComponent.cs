﻿using Microsoft.Xna.Framework;
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
        private SpriteFont _font;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;

        private InputModel _input;

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
            _font = Game.Content.Load<SpriteFont>("DefaultFont");

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