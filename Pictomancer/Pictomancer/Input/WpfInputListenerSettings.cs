using MonoGame.Framework.WpfInterop;

namespace Pictomancer.Input
{
    public abstract class WpfInputListenerSettings<T> where T : WpfInputListener
    {
        public abstract T CreateListener(WpfGame game);
    }
}