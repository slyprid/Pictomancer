﻿namespace Pictomancer.Mvvm
{
    public interface IView<T>
        where T : IViewModel
    {
        T ViewModel { get; set; }
    }
}