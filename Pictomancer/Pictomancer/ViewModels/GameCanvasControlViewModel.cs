using Microsoft.Xna.Framework;
using Pictomancer.Graphics;
using Pictomancer.Mvvm;

namespace Pictomancer.ViewModels
{
    public abstract class GameCanvasControlViewModel
        : PageViewModel
    {
        public GameCanvas Canvas { get; set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}