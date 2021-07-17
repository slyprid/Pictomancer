using Microsoft.Xna.Framework.Input;

namespace Pictomancer.Models
{
    public class InputModel
    {
        public MouseState CurrentMouseState { get; set; }
        public MouseState PreviousMouseState { get; set; }

        public KeyboardState CurrentKeyboardState { get; set; }
        public KeyboardState PreviousKeyboardState { get; set; }

    }
}