using System;
using System.Collections.ObjectModel;
using System.Linq;
using Maps.Content;
using Maps.Controls;
using Maps.Controls.Models;
using MapsApiStandardLibrary.Models.Directions;

namespace Maps.Collections
{
    public class SharedMyPins
    {
        private static readonly object Locker = new object();
        private static SharedMyPins _sharedMyPins;

        private MyPins _pins;
        private MyPin _selectedPin;

        public static SharedMyPins Get
        {
            get
            {
                if (_sharedMyPins == null)
                {
                    lock (Locker)
                    {
                        if (_sharedMyPins == null)
                        {
                            _sharedMyPins = new SharedMyPins();
                        }
                    }
                }

                return _sharedMyPins;
            }
        }

        public MyPins Pins
        {
            get => _pins;
            set
            {
                _pins = value;
                PinsChanged?.Invoke();
            }
        }

        private SharedMyPins()
        {
            _selectedPin = null;
            Pins = new MyPins();
        }

        public bool AddPin(MyPin pin)
        {
            bool result;
            switch (pin.MyType)
            {
                case MyPinType.Start:
                    result = AddStartPin(pin.Coordinate);
                    break;
                case MyPinType.Waypoint:
                    result = AddWaypointPin(pin);
                    break;
                case MyPinType.End:
                    result = AddEndPin(pin.Coordinate);
                    break;
                case MyPinType.MyLocation:
                    result = AddMyCoordinatePin(pin.Coordinate);
                    break;
                case MyPinType.Undefined:
                    result = AddWaypointPin(pin);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
        public bool AddStartPin(Coordinate startCoordinate)
        {
            Pins.StartPin.Coordinate = startCoordinate;
            var pin = new MyPin
            {
                Coordinate = startCoordinate,
                IconPath = IconsPath.StartEndPin,
                Label = "Start",
                MyType = MyPinType.Start
            };
            PinAdded?.Invoke(pin);
            return true;
        }
        public bool AddWaypointPin(MyPin waypointPin)
        {
            if (Pins.WaypointsPin.FirstOrDefault(el => el.Coordinate.Equals(waypointPin.Coordinate)) != null)
            {
                return false;
            }
            Pins.WaypointsPin.Add(waypointPin);
            PinAdded?.Invoke(waypointPin);
            return true;
        }
        public bool AddEndPin(Coordinate endCoordinate)
        {
            Pins.EndPin.Coordinate = endCoordinate;
            var pin = new MyPin
            {
                Coordinate = endCoordinate,
                IconPath = IconsPath.StartEndPin,
                Label = "End",
                MyType = MyPinType.End
            };
            PinAdded?.Invoke(pin);
            return true;
        }
        public bool AddMyCoordinatePin(Coordinate myCoordinateCoordinate)
        {
            Pins.MyCoordinatePin.Coordinate = myCoordinateCoordinate;
            var pin = new MyPin
            {
                Coordinate = myCoordinateCoordinate,
                IconPath = IconsPath.MyLocation,
                Label = "I",
                MyType = MyPinType.MyLocation
            };
            PinAdded?.Invoke(pin);
            return true;
        }

        public bool RemovePin(MyPin pin)
        {
            bool result;
            switch (pin.MyType)
            {
                case MyPinType.Start:
                    result = RemoveStartPin();
                    break;
                case MyPinType.Waypoint:
                    result = RemoveWaypointPin(pin);
                    break;
                case MyPinType.End:
                    result = RemoveEndPin();
                    break;
                case MyPinType.MyLocation:
                    result = RemoveMyCoordinatePin();
                    break;
                case MyPinType.Undefined:
                    result = RemoveUndefinedPin(pin);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
        public bool RemoveStartPin()
        {
            var pinCoordinate = Pins.StartPin.Coordinate;
            Pins.StartPin.Coordinate = null;
            PinRemoved?.Invoke(pinCoordinate);
            return true;
        }
        public bool RemoveWaypointPin(MyPin waypointPin)
        {
            var element = Pins.WaypointsPin.FirstOrDefault(el => el.Equals(waypointPin));
            if (element == null)
            {
                return false;
            }
            Pins.WaypointsPin.Remove(element);
            PinRemoved?.Invoke(waypointPin.Coordinate);
            return true;
        }
        public bool RemoveEndPin()
        {
            var pinCoordinate = Pins.EndPin.Coordinate;
            Pins.EndPin.Coordinate = null;
            PinRemoved?.Invoke(pinCoordinate);
            return true;
        }
        public bool RemoveMyCoordinatePin()
        {
            var pinCoordinate = Pins.MyCoordinatePin.Coordinate;
            Pins.MyCoordinatePin.Coordinate = null;
            PinRemoved?.Invoke(pinCoordinate);
            return true;
        }
        public bool RemoveUndefinedPin(MyPin pin)
        {

            if (RemoveWaypointPin(pin))
            {
                return true;
            }

            if (Pins.StartPin.Coordinate != null && Pins.StartPin.Coordinate.Equals(pin.Coordinate))
            {
                return RemoveStartPin();
            }

            if (Pins.EndPin.Coordinate != null && Pins.EndPin.Coordinate.Equals(pin.Coordinate))
            {
                return RemoveEndPin();
            }

            if (Pins.MyCoordinatePin.Coordinate != null &&
                Pins.MyCoordinatePin.Coordinate.Equals(pin.Coordinate))
            {
                return RemoveMyCoordinatePin();
            }

            return false;
        }

        public MyPin FindPin(MyPin pin)
        {
            switch (pin.MyType)
            {
                case MyPinType.Undefined:
                    var element = Pins.WaypointsPin.FirstOrDefault(el => el.Equals(pin));
                    if (element != null)
                    {
                        return element;
                    }

                    if (Pins.StartPin.Equals(pin))
                    {
                        return Pins.StartPin;
                    }

                    if (Pins.EndPin.Equals(pin))
                    {
                        return Pins.EndPin;
                    }

                    if (Pins.MyCoordinatePin.Equals(pin))
                    {
                        return Pins.MyCoordinatePin;
                    }

                    return null;
                case MyPinType.Start:
                    return Pins.StartPin;
                case MyPinType.Waypoint:
                    return Pins.WaypointsPin.FirstOrDefault(el => el.Equals(pin));
                case MyPinType.End:
                    return Pins.EndPin;
                case MyPinType.MyLocation:
                    return Pins.MyCoordinatePin;
                default:
                    return null;
            }
        }
        public void SelectPin(MyPin pin)
        {
            if (_selectedPin != null)
            {
                switch (_selectedPin.MyType)
                {
                    case MyPinType.Start:
                        _selectedPin.IconPath = IconsPath.StartEndPin;
                        break;
                    case MyPinType.Waypoint:
                        _selectedPin.IconPath = IconsPath.WaypointPin;
                        break;
                    case MyPinType.End:
                        _selectedPin.IconPath = IconsPath.StartEndPin;
                        break;
                    case MyPinType.MyLocation:
                        _selectedPin.IconPath = IconsPath.MyLocation;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            var myPin = FindPin(pin);
            if (myPin != null)
            {
                myPin.IconPath = IconsPath.SelectedPin;
            }
            PinSelected?.Invoke(myPin, _selectedPin);
            _selectedPin = myPin;
        }

        public void UpdatePinsFromPinPoints(ObservableCollection<PinPoint> collection)
        {
            Pins.StartPin.Coordinate = collection[0].Coordinate;
            Pins.WaypointsPin.Clear();
            for (var i = 1; i < collection.Count - 1; ++i)
            {
                var pin = new MyPin
                {
                    MyType = MyPinType.Waypoint,
                    Coordinate = collection[i].Coordinate,
                    IconPath = IconsPath.WaypointPin
                };
                Pins.WaypointsPin.Add(pin);
            }
            Pins.EndPin.Coordinate = collection[collection.Count - 1].Coordinate;

            PinsUpdated?.Invoke();
        }

        public event Action PinsChanged;
        public event Action PinsUpdated;
        public event Action<MyPin> PinAdded;
        public event Action<Coordinate> PinRemoved;
        public event Action<MyPin, MyPin> PinSelected;
    }
}
