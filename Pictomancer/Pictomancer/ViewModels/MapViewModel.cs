using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
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
        private List<Vector2> _filledPositions = new List<Vector2>();

        public Guid Id { get; set; }
        public Map Map { get; }
        public Color Color { get; set; }
        public int MouseX { get; set; }
        public int MouseY { get; set; }

        public ProjectViewModel Project { get; set; }

        public Tile PrimarySelectedTile => Project.MainViewModel.TilesViewModel.PrimarySelectedTile;
        public Tile SecondarySelectedTile => Project.MainViewModel.TilesViewModel.SecondarySelectedTile;

        public MapViewModel() { }

        public MapViewModel(Map map)
        {
            Map = map;
            Id = map.Id;
            Title = map.Name;
            Header = map.Name;

            Color = new Color(137, 137, 137, 255);

            _cursorColor = new ColorEx(Color.Yellow);
            _tweener = new Tweener();
            _tweener.TweenTo(_cursorColor, x => x.Value3, new Vector3(1f, 0, 0f), 0.25f, 0.025f).RepeatForever(0.2f).AutoReverse().Easing(EasingFunctions.Linear);
        }

        public override void LoadContent(ContentManager content)
        {
            if (ContentLoaded) return;

            ContentLoaded = true;
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
            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            Map.Draw(gameTime, spriteBatch);

            for (var y = 0; y < Map.Size.Y; y++)
            {
                for (var x = 0; x < Map.Size.X; x++)
                {
                    var w = Map.TileSize.X;
                    var h = Map.TileSize.Y;
                    spriteBatch.DrawRectangle(new RectangleF(x * w, y * h, w, h), Color.Black.WithOpacity(0.25f));
                }
            }

            spriteBatch.DrawRectangle(new RectangleF(_mx, _my, Map.TileSize.X, Map.TileSize.Y), _cursorColor.Color);

            spriteBatch.End();
        }

        #region Actions

        public void DrawPrimaryTile()
        {
            if (PrimarySelectedTile == null) return;
            var tx = (int)(MouseX / Map.TileSize.X);
            var ty = (int)(MouseY / Map.TileSize.Y);
            
            var mainViewModel = Project.MainViewModel;

            if (mainViewModel.PaintToolSelected) PaintTile(tx, ty, PrimarySelectedTile);
            if (mainViewModel.EraseToolSelected) EraseTile(tx, ty);
            if (mainViewModel.FillToolSelected) FillTile(tx, ty, PrimarySelectedTile);
        }

        public void DrawSecondaryTile()
        {
            if (SecondarySelectedTile == null) return;
            var tx = (int)(MouseX / Map.TileSize.X);
            var ty = (int)(MouseY / Map.TileSize.Y);
            SecondarySelectedTile.Position = new Vector2(tx * Map.TileSize.X, ty * Map.TileSize.Y);

            var mainViewModel = Project.MainViewModel;

            if (mainViewModel.PaintToolSelected) PaintTile(tx, ty, SecondarySelectedTile);
            if (mainViewModel.EraseToolSelected) EraseTile(tx, ty);
            if (mainViewModel.FillToolSelected) FillTile(tx, ty, SecondarySelectedTile);
        }

        private void PaintTile(int tx, int ty, Tile tile)
        {
            tile.Position = new Vector2(tx * Map.TileSize.X, ty * Map.TileSize.Y);
            var selectedLayer = ((TileLayer)Project.SelectedLayer);
            if (tx >= selectedLayer.Width) return;
            if (ty >= selectedLayer.Height) return;
            selectedLayer[tx, ty] = Tile.Clone(tile);
        }

        private void EraseTile(int tx, int ty)
        {
            var selectedLayer = ((TileLayer)Project.SelectedLayer);
            if (tx >= selectedLayer.Width) return;
            if (ty >= selectedLayer.Height) return;
            selectedLayer[tx, ty] = null;
        }

        private void FillTile(int tx, int ty, Tile tile)
        {
            tile.Position = new Vector2(tx * Map.TileSize.X, ty * Map.TileSize.Y);
            var selectedLayer = ((TileLayer)Project.SelectedLayer);
            var w = selectedLayer.Width;
            var h = selectedLayer.Height;
            if (tx >= selectedLayer.Width) return;
            if (ty >= selectedLayer.Height) return;
            
            var fillTile = Tile.Clone(tile);
            var originalTile = selectedLayer[tx, ty];
            _filledPositions.Clear();
            FloodFill(tx, ty, w, h, selectedLayer, fillTile, originalTile);
        }

        private void FloodFill(int x, int y, int w, int h, TileLayer layer, Tile fillTile, Tile oldTile)
        {
            if (layer == null) return;
            if (fillTile == null) return;

            try
            {
                if (x < 0 || x >= w) return;
                if (y < 0 || y >= h) return;
                var newPosition = new Vector2(x, y);
                if (_filledPositions.Contains(newPosition)) return;
                _filledPositions.Add(newPosition);
                var oldTileIndex = oldTile?.TextureRegion?.Name;
                var currentTileIndex = layer[x, y]?.TextureRegion?.Name;

                if (currentTileIndex == oldTileIndex)
                {
                    fillTile.Position = new Vector2(x * Map.TileSize.X, y * Map.TileSize.Y);
                    layer[x, y] = new Tile
                    {
                        Position = fillTile.Position,
                        Size = fillTile.Size,
                        Texture = fillTile.Texture,
                        TextureRegion = fillTile.TextureRegion
                    };
                    ;
                    FloodFill(x + 1, y, w, h, layer, fillTile, oldTile);
                    FloodFill(x - 1, y, w, h, layer, fillTile, oldTile);
                    FloodFill(x, y + 1, w, h, layer, fillTile, oldTile);
                    FloodFill(x, y - 1, w, h, layer, fillTile, oldTile);
                }
            }
            catch (StackOverflowException)
            {
                return;
            }
        }

        #endregion
    }
}