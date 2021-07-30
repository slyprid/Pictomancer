using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Framework.WpfInterop;
using Pictomancer.Components;

namespace Pictomancer.Input
{
    public static class WpfInput
    {
        private static WpfKeyboardListener _keyboardListener;
        private static WpfMouseListener _mouseListener;

        private static Dictionary<string, EventHandler<WpfKeyboardEventArgs>> _keyboardEvents;
        private static Dictionary<string, EventHandler<WpfMouseEventArgs>> _mouseEvents;


        #region Initialization

        static WpfInput()
        {
            _keyboardEvents = new Dictionary<string, EventHandler<WpfKeyboardEventArgs>>();
            _mouseEvents = new Dictionary<string, EventHandler<WpfMouseEventArgs>>();
        }

        public static void Register(WpfGame game, GameComponentCollection components)
        {
            _keyboardListener = new WpfKeyboardListener(game);
            _mouseListener = new WpfMouseListener(game);
            
            components.Add(new WpfInputListenerComponent(game, _keyboardListener, _mouseListener));
        }

        #endregion

        #region Keyboard

        public static void ClearKeyboardEvents()
        {
            foreach (var kvp in _keyboardEvents)
            {
                if (kvp.Key.StartsWith(nameof(_keyboardListener.KeyPressed))) _keyboardListener.KeyPressed -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_keyboardListener.KeyReleased))) _keyboardListener.KeyReleased -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_keyboardListener.KeyTyped))) _keyboardListener.KeyTyped -= kvp.Value;
            }

            _keyboardEvents.Clear();
        }

        private static void KeyPressed(EventHandler<WpfKeyboardEventArgs> eventHandler)
        {
            _keyboardListener.KeyPressed += eventHandler;
            _keyboardEvents.Add($"{nameof(_keyboardListener.KeyPressed)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void KeyReleased(EventHandler<WpfKeyboardEventArgs> eventHandler)
        {
            _keyboardListener.KeyReleased += eventHandler;
            _keyboardEvents.Add($"{nameof(_keyboardListener.KeyReleased)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void KeyTyped(EventHandler<WpfKeyboardEventArgs> eventHandler)
        {
            _keyboardListener.KeyTyped += eventHandler;
            _keyboardEvents.Add($"{nameof(_keyboardListener.KeyTyped)}_{Guid.NewGuid()}", eventHandler);
        }

        public static void OnKeyPressed(Keys key, Action<object, WpfKeyboardEventArgs> action)
        {
            KeyPressed((sender, args) =>
            {
                if (args.Key != key) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnKeyReleased(Keys key, Action<object, WpfKeyboardEventArgs> action)
        {
            KeyReleased((sender, args) =>
            {
                if (args.Key != key) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnKeyTyped(Keys key, Action<object, WpfKeyboardEventArgs> action)
        {
            KeyTyped((sender, args) =>
            {
                if (args.Key != key) return;
                action?.Invoke(sender, args);
            });
        }

        #endregion

        #region Mouse

        public static void ClearMouseEvents()
        {
            foreach (var kvp in _mouseEvents)
            {
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseClicked))) _mouseListener.MouseClicked -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDoubleClicked))) _mouseListener.MouseDoubleClicked -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDown))) _mouseListener.MouseDown -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDrag))) _mouseListener.MouseDrag -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDragEnd))) _mouseListener.MouseDragEnd -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDragStart))) _mouseListener.MouseDragStart -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseMoved))) _mouseListener.MouseMoved -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseUp))) _mouseListener.MouseUp -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseWheelMoved))) _mouseListener.MouseWheelMoved -= kvp.Value;
            }

            _mouseEvents.Clear();
        }

        private static void MouseClicked(EventHandler<WpfMouseEventArgs> eventHandler)
        {
            _mouseListener.MouseClicked += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseClicked)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDoubleClicked(EventHandler<WpfMouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDoubleClicked += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDoubleClicked)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDown(EventHandler<WpfMouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDown += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDown)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDrag(EventHandler<WpfMouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDrag += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDrag)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDragEnd(EventHandler<WpfMouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDragEnd += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDragEnd)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDragStart(EventHandler<WpfMouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDragStart += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDragStart)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseMoved(EventHandler<WpfMouseEventArgs> eventHandler)
        {
            _mouseListener.MouseMoved += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseMoved)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseUp(EventHandler<WpfMouseEventArgs> eventHandler)
        {
            _mouseListener.MouseUp += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseUp)}_{Guid.NewGuid()}", eventHandler);
        }


        public static void OnMouseClicked(MouseButton button, Action<object, WpfMouseEventArgs> action)
        {
            MouseClicked((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDoubleClicked(MouseButton button, Action<object, WpfMouseEventArgs> action)
        {
            MouseDoubleClicked((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDown(MouseButton button, Action<object, WpfMouseEventArgs> action)
        {
            MouseDown((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDrag(Action<object, WpfMouseEventArgs> action)
        {
            MouseDrag((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDragEnd(Action<object, WpfMouseEventArgs> action)
        {
            MouseDragEnd((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDragStart(Action<object, WpfMouseEventArgs> action)
        {
            MouseDragStart((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseMoved(Action<object, WpfMouseEventArgs> action)
        {
            MouseMoved((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseUp(MouseButton button, Action<object, WpfMouseEventArgs> action)
        {
            MouseUp((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        #endregion
    }
}