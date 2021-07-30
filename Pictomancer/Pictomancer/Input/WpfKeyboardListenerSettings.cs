using MonoGame.Framework.WpfInterop;

namespace Pictomancer.Input
{
    public class WpfKeyboardListenerSettings : WpfInputListenerSettings<WpfKeyboardListener>
    {
        public WpfKeyboardListenerSettings()
        {
            this.RepeatPress = true;
            this.InitialDelayMilliseconds = 800;
            this.RepeatDelayMilliseconds = 50;
        }

        public bool RepeatPress { get; set; }

        public int InitialDelayMilliseconds { get; set; }

        public int RepeatDelayMilliseconds { get; set; }

        public override WpfKeyboardListener CreateListener(WpfGame game) => new WpfKeyboardListener(game, this);
    }
}