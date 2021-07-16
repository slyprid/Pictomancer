using System;
using Pictomancer.Graphics;
using Relm.Maps;

namespace Pictomancer.ViewModels
{
    public class MapViewModel
        : PageViewModel
    {
        public Guid Id { get; set; }
        public Map Map { get; }

        public MapViewModel() { }

        public MapViewModel(Map map)
        {
            Map = map;
            Id = map.Id;
            Title = map.Name;
            Header = map.Name;
        }
    }
}