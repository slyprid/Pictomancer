using System.Windows.Input;
using Microsoft.Xna.Framework.Input;

namespace Pictomancer.Models
{
    public class InputModel
    {
        public MouseState CurrentMouseState { get; set; }
        public MouseState PreviousMouseState { get; set; }

        public KeyboardState CurrentKeyboardState { get; set; }
        public KeyboardState PreviousKeyboardState { get; set; }

        public bool IsMousePressed(MouseButton button)
        {
            if (button == MouseButton.Left && PreviousMouseState.LeftButton == ButtonState.Released && CurrentMouseState.LeftButton == ButtonState.Pressed) return true;
            if (button == MouseButton.Middle && PreviousMouseState.MiddleButton == ButtonState.Released && CurrentMouseState.MiddleButton == ButtonState.Pressed) return true;
            if (button == MouseButton.Right && PreviousMouseState.RightButton == ButtonState.Released && CurrentMouseState.RightButton == ButtonState.Pressed) return true;
            if (button == MouseButton.XButton1 && PreviousMouseState.XButton1 == ButtonState.Released && CurrentMouseState.XButton1 == ButtonState.Pressed) return true;
            if (button == MouseButton.XButton2 && PreviousMouseState.XButton2 == ButtonState.Released && CurrentMouseState.XButton2 == ButtonState.Pressed) return true;
            return false;
        }

        public bool IsMouseReleased(MouseButton button)
        {
            if (button == MouseButton.Left && PreviousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Released) return true;
            if (button == MouseButton.Middle && PreviousMouseState.MiddleButton == ButtonState.Pressed && CurrentMouseState.MiddleButton == ButtonState.Released) return true;
            if (button == MouseButton.Right && PreviousMouseState.RightButton == ButtonState.Pressed && CurrentMouseState.RightButton == ButtonState.Released) return true;
            if (button == MouseButton.XButton1 && PreviousMouseState.XButton1 == ButtonState.Pressed && CurrentMouseState.XButton1 == ButtonState.Released) return true;
            if (button == MouseButton.XButton2 && PreviousMouseState.XButton2 == ButtonState.Released && CurrentMouseState.XButton2 == ButtonState.Released) return true;
            return false;
        }

        public bool IsMouseHeld(MouseButton button)
        {
            if (button == MouseButton.Left && PreviousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Pressed) return true;
            if (button == MouseButton.Middle && PreviousMouseState.MiddleButton == ButtonState.Pressed && CurrentMouseState.MiddleButton == ButtonState.Pressed) return true;
            if (button == MouseButton.Right && PreviousMouseState.RightButton == ButtonState.Pressed && CurrentMouseState.RightButton == ButtonState.Pressed) return true;
            if (button == MouseButton.XButton1 && PreviousMouseState.XButton1 == ButtonState.Pressed && CurrentMouseState.XButton1 == ButtonState.Pressed) return true;
            if (button == MouseButton.XButton2 && PreviousMouseState.XButton2 == ButtonState.Pressed && CurrentMouseState.XButton2 == ButtonState.Pressed) return true;
            return false;
        }

    }
}