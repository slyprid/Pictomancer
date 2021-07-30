using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;

namespace Pictomancer.Input
{
    public class WpfKeyboardListener : WpfInputListener
    {
        private Array _keysValues = Enum.GetValues(typeof(Keys));
        private bool _isInitial;
        private TimeSpan _lastPressTime;
        private Keys _previousKey;
        private KeyboardState _previousState;
        private WpfKeyboard _keyboard;

        public WpfKeyboardListener(WpfGame game)
          : this(game, new WpfKeyboardListenerSettings())
        {
        }

        public WpfKeyboardListener(WpfGame game, WpfKeyboardListenerSettings settings)
        {
            this.RepeatPress = settings.RepeatPress;
            this.InitialDelay = settings.InitialDelayMilliseconds;
            this.RepeatDelay = settings.RepeatDelayMilliseconds;
            _keyboard = new WpfKeyboard(game);
        }

        public bool RepeatPress { get; }

        public int InitialDelay { get; }

        public int RepeatDelay { get; }

        public event EventHandler<WpfKeyboardEventArgs> KeyTyped;

        public event EventHandler<WpfKeyboardEventArgs> KeyPressed;

        public event EventHandler<WpfKeyboardEventArgs> KeyReleased;

        public override void Update(GameTime gameTime)
        {
            //KeyboardState state = Keyboard.GetState();
            var state = _keyboard.GetState();
            this.RaisePressedEvents(gameTime, state);
            this.RaiseReleasedEvents(state);
            if (this.RepeatPress)
                this.RaiseRepeatEvents(gameTime, state);
            this._previousState = state;
        }

        private void RaisePressedEvents(GameTime gameTime, KeyboardState currentState)
        {
            if (currentState.IsKeyDown(Keys.LeftAlt) || currentState.IsKeyDown(Keys.RightAlt))
                return;
            foreach (Keys key in this._keysValues.Cast<Keys>().Where<Keys>((Func<Keys, bool>)(key => currentState.IsKeyDown(key) && this._previousState.IsKeyUp(key))))
            {
                WpfKeyboardEventArgs e = new WpfKeyboardEventArgs(key, currentState);
                EventHandler<WpfKeyboardEventArgs> keyPressed = this.KeyPressed;
                if (keyPressed != null)
                    keyPressed((object)this, e);
                if (e.Character.HasValue)
                {
                    EventHandler<WpfKeyboardEventArgs> keyTyped = this.KeyTyped;
                    if (keyTyped != null)
                        keyTyped((object)this, e);
                }
                this._previousKey = key;
                this._lastPressTime = gameTime.TotalGameTime;
                this._isInitial = true;
            }
        }

        private void RaiseReleasedEvents(KeyboardState currentState)
        {
            foreach (Keys key in this._keysValues.Cast<Keys>().Where<Keys>((Func<Keys, bool>)(key => currentState.IsKeyUp(key) && this._previousState.IsKeyDown(key))))
            {
                EventHandler<WpfKeyboardEventArgs> keyReleased = this.KeyReleased;
                if (keyReleased != null)
                    keyReleased((object)this, new WpfKeyboardEventArgs(key, currentState));
            }
        }

        private void RaiseRepeatEvents(GameTime gameTime, KeyboardState currentState)
        {
            double totalMilliseconds = (gameTime.TotalGameTime - this._lastPressTime).TotalMilliseconds;
            if (!currentState.IsKeyDown(this._previousKey) || (!this._isInitial || totalMilliseconds <= (double)this.InitialDelay) && (this._isInitial || totalMilliseconds <= (double)this.RepeatDelay))
                return;
            WpfKeyboardEventArgs e = new WpfKeyboardEventArgs(this._previousKey, currentState);
            EventHandler<WpfKeyboardEventArgs> keyPressed = this.KeyPressed;
            if (keyPressed != null)
                keyPressed((object)this, e);
            if (e.Character.HasValue)
            {
                EventHandler<WpfKeyboardEventArgs> keyTyped = this.KeyTyped;
                if (keyTyped != null)
                    keyTyped((object)this, e);
            }
            this._lastPressTime = gameTime.TotalGameTime;
            this._isInitial = false;
        }
    }
}