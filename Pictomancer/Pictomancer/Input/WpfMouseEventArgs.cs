using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;

namespace Pictomancer.Input
{
    public class WpfMouseEventArgs : EventArgs
    {
        public WpfMouseEventArgs(
            ViewportAdapter viewportAdapter,
            TimeSpan time,
            MouseState previousState,
            MouseState currentState,
            MouseButton button = MouseButton.None)
        {
            this.PreviousState = previousState;
            this.CurrentState = currentState;
            this.Position = viewportAdapter != null ? viewportAdapter.PointToScreen(currentState.X, currentState.Y) : new Point(currentState.X, currentState.Y);
            this.Button = button;
            this.ScrollWheelValue = currentState.ScrollWheelValue;
            this.ScrollWheelDelta = currentState.ScrollWheelValue - previousState.ScrollWheelValue;
            this.Time = time;
        }

        public TimeSpan Time { get; }

        public MouseState PreviousState { get; }

        public MouseState CurrentState { get; }

        public Point Position { get; }

        public MouseButton Button { get; }

        public int ScrollWheelValue { get; }

        public int ScrollWheelDelta { get; }

        public Vector2 DistanceMoved
        {
            get
            {
                Point position = this.CurrentState.Position;
                Vector2 vector2_1 = position.ToVector2();
                position = this.PreviousState.Position;
                Vector2 vector2_2 = position.ToVector2();
                return vector2_1 - vector2_2;
            }
        }
    }
}