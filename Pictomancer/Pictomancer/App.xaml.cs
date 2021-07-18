using System;

namespace Pictomancer
{
    public partial class App
    {
        public static Random Rnd { get; set; }

        static App()
        {
            Rnd = new Random();
        }
    }
}