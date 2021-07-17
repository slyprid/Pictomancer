using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using Pictomancer.ViewModels;

namespace Pictomancer.Components
{
    public class MapRenderComponent
        : WpfDrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        public MapViewModel ViewModel { get; set; }

        public MapRenderComponent(WpfGame game) 
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Game.Content.Load<SpriteFont>("DefaultFont");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            ViewModel?.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ViewModel?.Color ?? Color.CornflowerBlue);
            ViewModel?.Draw(gameTime, _spriteBatch);
            base.Draw(gameTime);
        }
    }
}