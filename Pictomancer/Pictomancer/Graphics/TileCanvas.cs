﻿using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Tweening;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using Pictomancer.Models;
using Pictomancer.ViewModels;
using Relm.Extensions;

namespace Pictomancer.Graphics
{
    public class TileCanvas 
        : WpfGame
    {
        private IGraphicsDeviceService _graphicsDeviceManager;
        private WpfKeyboard _keyboard;
        private WpfMouse _mouse;
        private SpriteBatch _spriteBatch;
        private TextureAtlas _tileset;
        private InputModel _input;
        private Vector2 _mousePosition;
        private float _mx;
        private float _my;
        private readonly ColorEx _cursorColor;
        private readonly Tweener _tweener;
        private string primarySelected = "0-0";
        private string secondarySelected = "1-0";

        public int TileWidth = 32;
        public int TileHeight = 32;

        public TileCanvas()
        {
            _input = new InputModel();
            _cursorColor = new ColorEx(Color.Yellow);
            _tweener = new Tweener();
            _tweener.TweenTo(_cursorColor, x => x.Value3, new Vector3(1f, 0, 0f), 0.25f, 0.025f).RepeatForever(0.2f).AutoReverse().Easing(EasingFunctions.Linear);
        }

        protected override void Initialize()
        {
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);

            _keyboard = new WpfKeyboard(this);
            _mouse = new WpfMouse(this);

            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var regions = new Dictionary<string, Rectangle>();

            for (var y = 0; y < TileHeight; y++)
            {
                for (var x = 0; x < TileWidth; x++)
                {
                    regions.Add($"{x}-{y}", new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight));
                }
            }

            _tileset = new TextureAtlas("Tileset 1", Content.Load<Texture2D>("Tileset"), regions);
        }

        protected override void Update(GameTime gameTime)
        {
            _input.PreviousKeyboardState = _input.CurrentKeyboardState;
            _input.PreviousMouseState = _input.CurrentMouseState;
            _input.CurrentKeyboardState = _keyboard.GetState();
            _input.CurrentMouseState = _mouse.GetState();

            _mousePosition = new Vector2(_input.CurrentMouseState.X, _input.CurrentMouseState.Y);

            var xp = 144;
            var yp = 8;
            var ox = _mousePosition.X < xp ? 0 : _mousePosition.X - xp;
            var oy = _mousePosition.Y < yp ? 0 : _mousePosition.Y - yp;

            _mx = (int)(ox / TileWidth) * TileWidth;
            _my = (int)(oy / TileHeight) * TileHeight;

            _tweener.Update(gameTime.GetElapsedSeconds());

            var tx = (int)(_mx / TileWidth);
            var ty = (int) (_my / TileHeight);
            if (_input.IsMousePressed(MouseButton.Left))
            {
                primarySelected = $"{tx}-{ty}";
            }
            else if (_input.IsMousePressed(MouseButton.Right))
            {
                secondarySelected = $"{tx}-{ty}";
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_tileset.GetRegion(primarySelected), new Rectangle(8, 8, TileWidth * 2, TileHeight * 2), Color.White);
            _spriteBatch.Draw(_tileset.GetRegion(secondarySelected), new Rectangle(72, 72, TileWidth * 2, TileHeight * 2), Color.White);

            _spriteBatch.DrawRectangle(8, 8, TileWidth * 2, TileHeight * 2, Color.Black, 1f, 0f);
            _spriteBatch.DrawRectangle(72, 72, TileWidth * 2, TileHeight * 2, Color.Black, 1f, 0f);


            var w = 1632;
            var h = 192;
            var tw = TileWidth;
            var th = TileHeight;

            var xp = 144;
            var yp = 8;
            foreach (var texture in _tileset)
            {
                if (texture.X + xp > w || texture.Y + yp > h) continue;
                _spriteBatch.Draw(texture, new Rectangle(texture.X + xp, texture.Y + yp, tw, th), Color.White);
            }

            for (var y = 8; y < h; y += th)
            {
                for (var x = 144; x < w; x += tw)
                {
                    _spriteBatch.DrawRectangle(new RectangleF(x, y, tw, th), Color.Black);
                }
            }

            _spriteBatch.DrawRectangle(new RectangleF(_mx + xp, _my + yp, TileWidth, TileHeight), _cursorColor.Color);

            _spriteBatch.End();
        }
    }
}