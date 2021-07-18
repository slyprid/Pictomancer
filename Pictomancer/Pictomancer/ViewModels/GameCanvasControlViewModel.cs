using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pictomancer.Models;

namespace Pictomancer.ViewModels
{
    public abstract class GameCanvasControlViewModel
        : PageViewModel
    {
        public bool ContentLoaded { get; set; }

        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime, InputModel input);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}