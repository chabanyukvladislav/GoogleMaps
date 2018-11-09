using MapsApiLibrary.Api.Parameters.Directions.Enums;
using MapsApiLibrary.Helpers.MethodExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static System.Boolean;

namespace MapsApiLibrary.Api.Parameters.Directions
{
    public class DirectionsParameters
    {
        public Location Origin { get; set; }
        public Location Destination { get; set; }
        public string Key { get; set; }
        public Modes? Mode { get; set; }
        public List<Location> Waypoints { get; set; }
        public bool? Optimize { get; set; }
        public bool? Alternatives { get; set; }
        public Avoids? Avoid { get; set; }
        public Units? Units { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public TrafficModels? TrafficModel { get; set; }
        public TransitModes? TransitMode { get; set; }
        public TransitRoutingPreferences? TransitRoutingPreference { get; set; }

        public DirectionsParameters()
        {
            Origin = new Location();
            Destination = new Location();
            Waypoints = new List<Location>(8);
        }

        private static string WaypointsToString(IEnumerable<Location> waypoints, bool? isOptimize = null)
        {
            var result = new StringBuilder();
            result.Append($"{nameof(Waypoints).ToLower()}=");
            if (isOptimize != null && isOptimize.Value)
            {
                result.Append($"{nameof(Optimize).ToLower()}:true|");
            }

            foreach (var waypoint in waypoints)
            {
                result.Append($"{(string)waypoint}|");
            }

            result[result.Length - 1] = '&';
            return result.ToString();
        }
        private static (List<Location>, bool?) StringToWaypoints(string waypoints)
        {
            var result = new List<Location>();
            bool? optimize = null;
            var values = waypoints.Split('|');
            var startIndex = 0;
            if (values[0] == "optimize:true")
            {
                optimize = true;
                startIndex = 1;
            }

            for (var i = startIndex; i < values.Length; ++i)
            {
                var waypoint = values[i];
                result.Add(waypoint);
            }
            return (result, optimize);
        }

        private static string DateTimeToString(DateTime date)
        {
            var mainPoint = new DateTime(1970, 1, 1); //Main point date
            var pastTime = date - mainPoint;
            return pastTime.TotalSeconds.ToString(CultureInfo.InvariantCulture).Trim(',');
        }
        private static DateTime StringToDateTime(string date)
        {
            var mainPoint = new DateTime(1970, 1, 1); //Main point date
            var timeOfDay = double.Parse(date) + mainPoint.TimeOfDay.TotalSeconds;
            var timeSpan = new TimeSpan(0, 0, (int)timeOfDay);
            var result = mainPoint + timeSpan;
            return result;
        }

        private static string TransitRoutingPreferencesToString(TransitRoutingPreferences transitRoutingPreference)
        {
            string value;
            switch (transitRoutingPreference)
            {
                case TransitRoutingPreferences.FewerTransfers:
                    value = "fewer_transfers";
                    break;
                case TransitRoutingPreferences.LessWalking:
                    value = "less_walking";
                    break;
                default:
                    value = "";
                    break;
            }
            var result = $"transit_routing_preference={value}&";
            return result;
        }
        private static TransitRoutingPreferences? StringToTransitRoutingPreferences(string transitRoutingPreferences)
        {
            switch (transitRoutingPreferences)
            {
                case "fewer_transfers":
                    return TransitRoutingPreferences.FewerTransfers;
                case "less_walking":
                    return TransitRoutingPreferences.LessWalking;
                default:
                    return null;
            }
        }

        public void Clear()
        {
            Origin = new Location();
            Destination = new Location();
            Key = null;
            Waypoints = new List<Location>(8);
            Mode = null;
            Optimize = null;
            Alternatives = null;
            Avoid = null;
            Units = null;
            ArrivalTime = null;
            DepartureTime = null;
            TrafficModel = null;
            TransitMode = null;
            TransitRoutingPreference = null;
        }

        public static implicit operator string(DirectionsParameters param)
        {
            if (param == null)
            {
                return "";
            }

            var result = new StringBuilder();
            result.Append($"{nameof(Key).ToLower()}={param.Key}&");
            result.Append($"{nameof(Origin).ToLower()}={(string)param.Origin}&");
            result.Append($"{nameof(Destination).ToLower()}={(string)param.Destination}&");

            if (param.Mode != null && param.Mode.Value != Modes.Driving)
            {
                result.Append($"{nameof(Mode).ToLower()}={param.Mode.Value.ToString().ToLower()}&");
            }

            if (param.Waypoints.Count > 0)
            {
                var waypoints = WaypointsToString(param.Waypoints, param.Optimize);
                result.Append(waypoints);
            }

            if (param.Alternatives != null && param.Alternatives.Value)
            {
                result.Append($"{nameof(Alternatives).ToLower()}=true&");
            }

            if (param.Avoid != null)
            {
                result.Append($"{nameof(Avoid).ToLower()}={param.Avoid.Value.ToString().ToLower()}&");
            }

            if (param.Units != null)
            {
                result.Append($"{nameof(Units).ToLower()}={param.Units.Value.ToString().ToLower()}&");
            }

            if (param.ArrivalTime != null)
            {
                var date = DateTimeToString(param.ArrivalTime.Value);
                result.Append($"arrival_time={date}&");
            }

            if (param.DepartureTime != null)
            {
                var date = DateTimeToString(param.DepartureTime.Value);
                result.Append($"departure_time={date}&");
            }

            if (param.TrafficModel != null && param.TrafficModel.Value != TrafficModels.BestGuess)
            {
                result.Append($"traffic_model={param.TrafficModel.Value.ToString().ToLower()}&");
            }

            if (param.TransitMode != null)
            {
                result.Append($"transit_mode={param.TransitMode.Value.ToString().ToLower()}&");
            }

            if (param.TransitRoutingPreference != null)
            {
                var transitRoutingPreference = TransitRoutingPreferencesToString(param.TransitRoutingPreference.Value);
                result.Append(transitRoutingPreference);
            }

            if (result[result.Length - 1] == '&')
            {
                result = result.Remove(result.Length - 1, 1);
            }

            return result.ToString();
        }
        public static implicit operator DirectionsParameters(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                return new DirectionsParameters();
            }

            var result = new DirectionsParameters();
            var parameters = param.Split('&');
            foreach (var parameter in parameters)
            {
                var keyAndValue = parameter.Split('=');
                var nameValue = new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1]);

                switch (nameValue.Key)
                {
                    case "origin":
                        {
                            result.Origin = nameValue.Value;
                            break;
                        }
                    case "destination":
                        {
                            result.Destination = nameValue.Value;
                            break;
                        }
                    case "key":
                        {
                            result.Key = nameValue.Value;
                            break;
                        }
                    case "mode":
                        {
                            result.Mode = Enum.Parse<Modes>(nameValue.Value.ToUpperFirstSymbol());
                            break;
                        }
                    case "waypoints":
                        {
                            var waypoints = StringToWaypoints(nameValue.Value);
                            result.Waypoints = waypoints.Item1;
                            result.Optimize = waypoints.Item2;
                            break;
                        }
                    case "alternatives":
                        {
                            result.Alternatives = Parse(nameValue.Value);
                            break;
                        }
                    case "avoid":
                        {
                            result.Avoid = Enum.Parse<Avoids>(nameValue.Value.ToUpperFirstSymbol());
                            break;
                        }
                    case "units":
                        {
                            result.Units = Enum.Parse<Units>(nameValue.Value.ToUpperFirstSymbol());
                            break;
                        }
                    case "arrival_time":
                        {
                            var date = StringToDateTime(nameValue.Value);
                            result.ArrivalTime = date;
                            break;
                        }
                    case "departure_time":
                        {
                            var date = StringToDateTime(nameValue.Value);
                            result.DepartureTime = date;
                            break;
                        }
                    case "traffic_model":
                        {
                            result.TrafficModel = Enum.Parse<TrafficModels>(nameValue.Value.ToUpperFirstSymbol());
                            break;
                        }
                    case "transit_mode":
                        {
                            result.TransitMode = Enum.Parse<TransitModes>(nameValue.Value.ToUpperFirstSymbol());
                            break;
                        }
                    case "transit_routing_preference":
                        {
                            result.TransitRoutingPreference = StringToTransitRoutingPreferences(nameValue.Value);
                            break;
                        }
                }
            }

            return result;
        }
    }
}
