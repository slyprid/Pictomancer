using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Relm.Maps;

namespace Pictomancer.ViewModels
{
    public class MapViewModel
        : GameCanvasControlViewModel
    {
        public Guid Id { get; set; }
        public Map Map { get; }

        public Color Color { get; set; }

        private SpriteBatch _spriteBatch;

        public MapViewModel() { }

        public MapViewModel(Map map)
        {
            Map = map;
            Id = map.Id;
            Title = map.Name;
            Header = map.Name;

            Color = new Color(new Vector3((float)App.Rnd.NextDouble(), (float)App.Rnd.NextDouble(), (float)App.Rnd.NextDouble()));
        }

        public override void Update(GameTime gameTime)
        {
            if (_spriteBatch == null)
            {
                _spriteBatch = new SpriteBatch(Canvas.GraphicsDevice);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Canvas.GraphicsDevice.Clear(Color);

            if (_spriteBatch == null) return;

            _spriteBatch.Begin();

            _spriteBatch.DrawCircle(new Vector2(App.Rnd.Next(0, 400), App.Rnd.Next(0, 400)), App.Rnd.Next(0, 256), 16, new Color(new Vector3((float)App.Rnd.NextDouble(), (float)App.Rnd.NextDouble(), (float)App.Rnd.NextDouble())));

            _spriteBatch.End();
        }
    }
}