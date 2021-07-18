using System;
using Pictomancer.ViewModels;

namespace Pictomancer
{
    public partial class App
    {
        public static MainViewModel MainViewModel { get; set; }
        public static Random Rnd { get; set; }
        
        static App()
        {
            Rnd = new Random();
        }

        public static void Log(string message)
        {
            var timestamp = $"{DateTime.Now:G}";
            MainViewModel.ConsoleLog += $"[{timestamp}]: {message}\r\n";
        }
    }
}