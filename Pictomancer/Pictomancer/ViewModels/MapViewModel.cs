using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;
using Pictomancer.Models;
using Relm.Extensions;
using Relm.Maps;
using Relm.Tiles;

namespace Pictomancer.ViewModels
{
    public class MapViewModel
        : GameCanvasControlViewModel
    {
        private Vector2 _mousePosition;
        private float _mx;
        private float _my;
        private readonly ColorEx _cursorColor;
        private readonly Tweener _tweener;

        public Guid Id { get; set; }
        public Map Map { get; }
        public Color Color { get; set; }
        public int MouseX { get; set; }
        public int MouseY { get; set; }

        public Tile PrimarySelectedTile { get; set; }
        public Tile SecondarySelectedTile { get; set; }
        
        public MapViewModel() { }

        public MapViewModel(Map map)
        {
            Map = map;
            Id = map.Id;
            Title = map.Name;
            Header = map.Name;

            Color = new Color(30, 30, 30, 255);

            _cursorColor = new ColorEx(Color.Yellow);
            _tweener = new Tweener();
            _tweener.TweenTo(_cursorColor, x => x.Value3, new Vector3(1f, 0, 0f), 0.25f, 0.025f).RepeatForever(0.2f).AutoReverse().Easing(EasingFunctions.Linear);

            PrimarySelectedTile = new Tile();
            SecondarySelectedTile = new Tile();
        }

        public override void LoadContent(ContentManager content)
        {
            if (ContentLoaded) return;

            ContentLoaded = true;

            var grassTexture = content.Load<Texture2D>("Grass");
            var blockTexture = content.Load<Texture2D>("Block");
            PrimarySelectedTile.Texture = blockTexture;
            SecondarySelectedTile.Texture = grassTexture;

            foreach (var layer in Map.Layers.OfType<TileLayer>())
            {
                for (var y = 0; y < layer.Height; y++)
                {
                    for (var x = 0; x < layer.Width; x++)
                    {
                        layer[x, y].Texture = grassTexture;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime, InputModel input)
        {
            _mousePosition = new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y);
            _mx = (int)(_mousePosition.X / Map.TileSize.X) * Map.TileSize.X;
            _my = (int)(_mousePosition.Y / Map.TileSize.Y) * Map.TileSize.Y;
            MouseX = (int) _mx;
            MouseY = (int) _my;

            _tweener.Update(gameTime.GetElapsedSeconds());
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (spriteBatch == null) return;
            
            spriteBatch.Begin();

            Map.Draw(gameTime, spriteBatch);

            for (var y = 0; y < Map.Size.Y; y++)
            {
                for (var x = 0; x < Map.Size.X; x++)
                {
                    var w = Map.TileSize.X;
                    var h = Map.TileSize.Y;
                    spriteBatch.DrawRectangle(new RectangleF(x * w, y * h, w, h), Color.Black);
                }
            }

            spriteBatch.DrawRectangle(new RectangleF(_mx, _my, Map.TileSize.X, Map.TileSize.Y), _cursorColor.Color);

            spriteBatch.End();
        }
    }
}