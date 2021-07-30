using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Framework.WpfInterop;
using Pictomancer.Input;

namespace Pictomancer.Components
{
    public class WpfInputListenerComponent
        : WpfDrawableGameComponent, IUpdate
    {
        private readonly List<WpfInputListener> _listeners;

        public WpfInputListenerComponent(WpfGame game) : base(game) => _listeners = new List<WpfInputListener>();

        public WpfInputListenerComponent(WpfGame game, params WpfInputListener[] listeners) : base(game) => _listeners = new List<WpfInputListener>(listeners);

        public IList<WpfInputListener> Listeners => _listeners;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!Game.IsActive) return;
            foreach (var listener in _listeners)
                listener.Update(gameTime);
        }
    }
}