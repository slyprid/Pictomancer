using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Pictomancer.Models;
using Relm.Maps;

namespace Pictomancer.ViewModels
{
    public class MapViewModel
        : GameCanvasControlViewModel
    {
        public Guid Id { get; set; }
        public Map Map { get; }
        public Color Color { get; set; }

        public List<Vector2> TilePoints = new List<Vector2>();
        public Texture2D TileTexture;
        
        public MapViewModel() { }

        public MapViewModel(Map map)
        {
            Map = map;
            Id = map.Id;
            Title = map.Name;
            Header = map.Name;

            Color = new Color(new Vector3((float)App.Rnd.NextDouble(), (float)App.Rnd.NextDouble(), (float)App.Rnd.NextDouble()));
        }

        public override void LoadContent(ContentManager content)
        {
            if (ContentLoaded) return;

            ContentLoaded = true;

            TileTexture = content.Load<Texture2D>("Grass");
        }

        public override void Update(GameTime gameTime, InputModel input)
        {
            if (input.PreviousMouseState.LeftButton == ButtonState.Released &&
                input.CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                var pos = new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y);
                TilePoints.Add(pos);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) return;

            spriteBatch.Begin();

            foreach (var pos in TilePoints)
            {
                spriteBatch.Draw(TileTexture, pos, Color.White);
            }

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