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
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) return;

            spriteBatch.Begin();

            for (var y = 0; y < Map.Size.Y; y++)
            {
                for (var x = 0; x < Map.Size.X; x++)
                {
                    var w = Map.TileSize.X;
                    var h = Map.TileSize.Y;
                    var loc = new Vector2(x * w, y * h);
                    spriteBatch.DrawRectangle(new RectangleF(x * w, y * h, w, h), Color.Black, 1f, 0f);
                }
            }

            spriteBatch.End();
        }
    }
}