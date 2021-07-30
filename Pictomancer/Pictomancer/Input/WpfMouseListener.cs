using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;

namespace Pictomancer.Input
{
    public class WpfMouseListener 
        : WpfInputListener
    {
        private MouseState _currentState;
        private bool _dragging;
        private GameTime _gameTime;
        private bool _hasDoubleClicked;
        private WpfMouseEventArgs _mouseDownArgs;
        private WpfMouseEventArgs _previousClickArgs;
        private MouseState _previousState;
        private WpfMouse _mouse;

        public WpfMouseListener(WpfGame game) : this(game, new MouseListenerSettings()) { }

        public WpfMouseListener(WpfGame game, ViewportAdapter viewportAdapter) : this(game, new MouseListenerSettings()) => this.ViewportAdapter = viewportAdapter;

        public WpfMouseListener(WpfGame game, MouseListenerSettings settings)
        {
            this.ViewportAdapter = settings.ViewportAdapter;
            this.DoubleClickMilliseconds = settings.DoubleClickMilliseconds;
            this.DragThreshold = settings.DragThreshold;
            _mouse = new WpfMouse(game);
        }

        public ViewportAdapter ViewportAdapter { get; }

        public int DoubleClickMilliseconds { get; }

        public int DragThreshold { get; }

        public bool HasMouseMoved => this._previousState.X != this._currentState.X || this._previousState.Y != this._currentState.Y;

        public event EventHandler<WpfMouseEventArgs> MouseDown;

        public event EventHandler<WpfMouseEventArgs> MouseUp;

        public event EventHandler<WpfMouseEventArgs> MouseClicked;

        public event EventHandler<WpfMouseEventArgs> MouseDoubleClicked;

        public event EventHandler<WpfMouseEventArgs> MouseMoved;

        public event EventHandler<WpfMouseEventArgs> MouseWheelMoved;

        public event EventHandler<WpfMouseEventArgs> MouseDragStart;

        public event EventHandler<WpfMouseEventArgs> MouseDrag;

        public event EventHandler<WpfMouseEventArgs> MouseDragEnd;

        private void CheckButtonPressed(
          Func<MouseState, ButtonState> getButtonState,
          MouseButton button)
        {
            if (getButtonState(this._currentState) != ButtonState.Pressed || getButtonState(this._previousState) != ButtonState.Released)
                return;
            WpfMouseEventArgs e = new WpfMouseEventArgs(this.ViewportAdapter, this._gameTime.TotalGameTime, this._previousState, this._currentState, button);
            EventHandler<WpfMouseEventArgs> mouseDown = this.MouseDown;
            if (mouseDown != null)
                mouseDown((object)this, e);
            this._mouseDownArgs = e;
            if (this._previousClickArgs == null)
                return;
            if ((e.Time - this._previousClickArgs.Time).TotalMilliseconds <= (double)this.DoubleClickMilliseconds)
            {
                EventHandler<WpfMouseEventArgs> mouseDoubleClicked = this.MouseDoubleClicked;
                if (mouseDoubleClicked != null)
                    mouseDoubleClicked((object)this, e);
                this._hasDoubleClicked = true;
            }
            this._previousClickArgs = (WpfMouseEventArgs)null;
        }

        private void CheckButtonReleased(
          Func<MouseState, ButtonState> getButtonState,
          MouseButton button)
        {
            if (getButtonState(this._currentState) != ButtonState.Released || getButtonState(this._previousState) != ButtonState.Pressed)
                return;
            WpfMouseEventArgs e = new WpfMouseEventArgs(this.ViewportAdapter, this._gameTime.TotalGameTime, this._previousState, this._currentState, button);
            if (this._mouseDownArgs.Button == e.Button)
            {
                if (WpfMouseListener.DistanceBetween(e.Position, this._mouseDownArgs.Position) < this.DragThreshold)
                {
                    if (!this._hasDoubleClicked)
                    {
                        EventHandler<WpfMouseEventArgs> mouseClicked = this.MouseClicked;
                        if (mouseClicked != null)
                            mouseClicked((object)this, e);
                    }
                }
                else
                {
                    EventHandler<WpfMouseEventArgs> mouseDragEnd = this.MouseDragEnd;
                    if (mouseDragEnd != null)
                        mouseDragEnd((object)this, e);
                    this._dragging = false;
                }
            }
            EventHandler<WpfMouseEventArgs> mouseUp = this.MouseUp;
            if (mouseUp != null)
                mouseUp((object)this, e);
            this._hasDoubleClicked = false;
            this._previousClickArgs = e;
        }

        private void CheckMouseDragged(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if (getButtonState(this._currentState) != ButtonState.Pressed || getButtonState(this._previousState) != ButtonState.Pressed)
                return;
            WpfMouseEventArgs e = new WpfMouseEventArgs(this.ViewportAdapter, this._gameTime.TotalGameTime, this._previousState, this._currentState, button);
            if (this._mouseDownArgs.Button != e.Button)
                return;
            if (this._dragging)
            {
                EventHandler<WpfMouseEventArgs> mouseDrag = this.MouseDrag;
                if (mouseDrag == null)
                    return;
                mouseDrag((object)this, e);
            }
            else
            {
                if (WpfMouseListener.DistanceBetween(e.Position, this._mouseDownArgs.Position) <= this.DragThreshold)
                    return;
                this._dragging = true;
                EventHandler<WpfMouseEventArgs> mouseDragStart = this.MouseDragStart;
                if (mouseDragStart == null)
                    return;
                mouseDragStart((object)this, e);
            }
        }

        public override void Update(GameTime gameTime)
        {
            this._gameTime = gameTime;
            //this._currentState = Mouse.GetState();
            _currentState = _mouse.GetState();
            this.CheckButtonPressed((Func<MouseState, ButtonState>)(s => s.LeftButton), MouseButton.Left);
            this.CheckButtonPressed((Func<MouseState, ButtonState>)(s => s.MiddleButton), MouseButton.Middle);
            this.CheckButtonPressed((Func<MouseState, ButtonState>)(s => s.RightButton), MouseButton.Right);
            this.CheckButtonPressed((Func<MouseState, ButtonState>)(s => s.XButton1), MouseButton.XButton1);
            this.CheckButtonPressed((Func<MouseState, ButtonState>)(s => s.XButton2), MouseButton.XButton2);
            this.CheckButtonReleased((Func<MouseState, ButtonState>)(s => s.LeftButton), MouseButton.Left);
            this.CheckButtonReleased((Func<MouseState, ButtonState>)(s => s.MiddleButton), MouseButton.Middle);
            this.CheckButtonReleased((Func<MouseState, ButtonState>)(s => s.RightButton), MouseButton.Right);
            this.CheckButtonReleased((Func<MouseState, ButtonState>)(s => s.XButton1), MouseButton.XButton1);
            this.CheckButtonReleased((Func<MouseState, ButtonState>)(s => s.XButton2), MouseButton.XButton2);
            if (this.HasMouseMoved)
            {
                EventHandler<WpfMouseEventArgs> mouseMoved = this.MouseMoved;
                if (mouseMoved != null)
                    mouseMoved((object)this, new WpfMouseEventArgs(this.ViewportAdapter, gameTime.TotalGameTime, this._previousState, this._currentState));
                this.CheckMouseDragged((Func<MouseState, ButtonState>)(s => s.LeftButton), MouseButton.Left);
                this.CheckMouseDragged((Func<MouseState, ButtonState>)(s => s.MiddleButton), MouseButton.Middle);
                this.CheckMouseDragged((Func<MouseState, ButtonState>)(s => s.RightButton), MouseButton.Right);
                this.CheckMouseDragged((Func<MouseState, ButtonState>)(s => s.XButton1), MouseButton.XButton1);
                this.CheckMouseDragged((Func<MouseState, ButtonState>)(s => s.XButton2), MouseButton.XButton2);
            }
            if (this._previousState.ScrollWheelValue != this._currentState.ScrollWheelValue)
            {
                EventHandler<WpfMouseEventArgs> mouseWheelMoved = this.MouseWheelMoved;
                if (mouseWheelMoved != null)
                    mouseWheelMoved((object)this, new WpfMouseEventArgs(this.ViewportAdapter, gameTime.TotalGameTime, this._previousState, this._currentState));
            }
            this._previousState = this._currentState;
        }

        private static int DistanceBetween(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}