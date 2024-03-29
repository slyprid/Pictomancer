﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pictomancer.Models;
using Relm.Tiles;

namespace Pictomancer.ViewModels
{
    public class TilesViewModel
        : GameCanvasControlViewModel
    {
        public ProjectViewModel Project { get; set; }
        public Tileset Tileset { get; set; }
        public string PrimarySelected { get; set; }
        public string SecondarySelected { get; set; }
        public Tile PrimarySelectedTile { get; set; }
        public Tile SecondarySelectedTile { get; set; }

        public override void LoadContent(ContentManager content)
        {
            
        }

        public override void Update(GameTime gameTime, InputModel input)
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
    }
}