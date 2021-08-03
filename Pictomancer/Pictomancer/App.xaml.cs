using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Pictomancer.Graphics;
using Pictomancer.ViewModels;
using Pictomancer.Views;

namespace Pictomancer
{
    public partial class App
    {
        public static MainViewModel MainViewModel { get; set; }
        public static Random Rnd { get; set; }
        public static MainWindow MainWindowInstance { get; set; }

        public static Dictionary<string, GraphicsDeviceState> GraphicsDeviceStates { get; set; }
        
        static App()
        {
            Rnd = new Random();
            GraphicsDeviceStates = new Dictionary<string, GraphicsDeviceState>();
        }

        public static void Log(string message)
        {
            var timestamp = $"{DateTime.Now:G}";
            MainViewModel.ConsoleLog += $"[{timestamp}]: {message}\r\n";
        }

        public static void SaveState(string key, GraphicsDeviceState state)
        {
            if (GraphicsDeviceStates.ContainsKey(key))
            {
                GraphicsDeviceStates[key] = state;
            }
            else
            {
                GraphicsDeviceStates.Add(key, state);
            }
        }

        public static void RestoreState(string key, GraphicsDevice graphicsDevice)
        {
            var state = GraphicsDeviceStates[key];
            state.Restore(graphicsDevice);
        }

        public static void SetMapStatus(string message)
        {
            MainWindowInstance.txtMapLocation.Text = message;
        }

        public static void SetMouseStatus(string message)
        {
            MainWindowInstance.txtMouseLocation.Text = message;
        }
    }
}