using System;
using System.Collections.Generic;
using Maps.Helpers;
using MapsApiLibrary.Models.Directions;

namespace Maps.Collections
{
    public class SharedRoutePath
    {
        private static readonly object Locker = new object();
        private static SharedRoutePath _sharedRoutePath;

        private IEnumerable<Coordinate> _coordinates;

        public static SharedRoutePath Get
        {
            get
            {
                if (_sharedRoutePath == null)
                {
                    lock (Locker)
                    {
                        if (_sharedRoutePath == null)
                        {
                            _sharedRoutePath = new SharedRoutePath();
                        }
                    }
                }

                return _sharedRoutePath;
            }
        }

        public IEnumerable<Coordinate> Coordinates
        {
            get => _coordinates;
            private set
            {
                _coordinates = value;
                CoordinatesChanged?.Invoke();
            }
        }

        private SharedRoutePath()
        {
            Coordinates = new List<Coordinate>();
            SharedResult.ResultChanged += OnResultChanged;
        }

        public void SetPolyline(string encodePolyline)
        {
            Coordinates = PolylineDecoder.Decode(encodePolyline);
        }

        private void OnResultChanged()
        {
            SetPolyline(SharedResult.Result.Routes[0].OverviewPolyline.Points);
        }

        public event Action CoordinatesChanged;
    }
}
