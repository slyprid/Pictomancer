using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using Pictomancer.ViewModels;

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var regions = new Dictionary<string, Rectangle>();

            var i = 0;
            for (var y = 0; y < 32; y++)
            {
                for (var x = 0; x < 32; x++)
                {
                    regions.Add($"{i++}", new Rectangle(x * 32, y * 32, 32, 32));
                }
            }

            _tileset = new TextureAtlas("Tileset 1", Content.Load<Texture2D>("Tileset"), regions);
        }

        protected override void Update(GameTime time)
        {
            // every update we can now query the keyboard & mouse for our WpfGame
            var mouseState = _mouse.GetState();
            var keyboardState = _keyboard.GetState();
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_tileset.GetRegion("0"), new Rectangle(8, 8, 64, 64), Color.White);
            _spriteBatch.Draw(_tileset.GetRegion("1"), new Rectangle(72, 72, 64, 64), Color.White);

            _spriteBatch.DrawRectangle(8, 8, 64, 64, Color.Black, 1f, 0f);
            _spriteBatch.DrawRectangle(72, 72, 64, 64, Color.Black, 1f, 0f);


            var w = 1632;
            var h = 192;
            var tw = 32;
            var th = 32;

            var xp = 144;
            var yp = 8;
            foreach (var texture in _tileset)
            {
                _spriteBatch.Draw(texture, new Rectangle(xp, yp, tw, th), Color.White);
                xp += tw;
                if (xp >= w)
                {
                    xp = 144;
                    yp += th;
                }

                if (yp >= h)
                {
                    break;
                }
            }

            for (var y = 8; y < h; y += th)
            {
                for (var x = 144; x < w; x += tw)
                {
                    _spriteBatch.DrawRectangle(new RectangleF(x, y, tw, th), Color.Black);
                }
            }

            _spriteBatch.End();
        }
    }
}