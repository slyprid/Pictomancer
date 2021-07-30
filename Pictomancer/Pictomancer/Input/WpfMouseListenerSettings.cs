using MonoGame.Extended.ViewportAdapters;
using MonoGame.Framework.WpfInterop;

namespace Pictomancer.Input
{
    public class MouseListenerSettings : WpfInputListenerSettings<WpfMouseListener>
    {
        public MouseListenerSettings()
        {
            this.DoubleClickMilliseconds = 500;
            this.DragThreshold = 2;
        }

        public int DragThreshold { get; set; }

        public int DoubleClickMilliseconds { get; set; }

        public ViewportAdapter ViewportAdapter { get; set; }

        public override WpfMouseListener CreateListener(WpfGame game) => new WpfMouseListener(game, this);
    }
}