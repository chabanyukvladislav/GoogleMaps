using System;
using MapsApiLibrary.Models.Directions;

namespace Maps.Collections
{
    public static class SharedResult
    {
        private static DirectionsResult _result;

        public static DirectionsResult Result
        {
            get => _result;
            set
            {
                _result = value;
                ResultChanged?.Invoke();
            }
        }

        public static event Action ResultChanged;
    }
}
