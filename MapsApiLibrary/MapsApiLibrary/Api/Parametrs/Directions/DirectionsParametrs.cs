using System;
using System.Collections.Generic;
using System.Text;
using MapsApiLibrary.Api.Parametrs.Directions.Enums;
using MapsApiLibrary.Models.Directions;

namespace MapsApiLibrary.Api.Parametrs.Directions
{
    public class DirectionsParametrs
    {
        public Location Origin { get; set; }
        public Location Destination { get; set; }
        public string Key { get; set; }
        public Modes? Mode { get; set; }
        public List<Location> Waypoints { get; set; }
        public bool? Optimize { get; set; } //Waypoints
        public bool? Alternatives { get; set; }
        public Avoids? Avoid { get; set; }
        public Units? Units { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public TrafficModels? TrafficModel { get; set; }
        public TransitModes? TransitMode { get; set; }
        public TransitRoutingPreferences? TransitRoutingPreference { get; set; }

        public DirectionsParametrs()
        {
            Origin = new Location();
            Destination = new Location();
            Waypoints = new List<Location>(8);

        }

        public static implicit operator string(DirectionsParametrs param)
        {
            if (param == null)
            {
                return "";
            }

            var result = new StringBuilder();
            result.Append(string.Format($"{0}={1}&", nameof(Key).ToLower(), param.Key));
            result.Append(string.Format($"{0}={1}&", nameof(Origin).ToLower(), param.Origin));
            result.Append(string.Format($"{0}={1}&", nameof(Destination).ToLower(), param.Destination));

            if (param.Mode != null && param.Mode.Value != Modes.Driving)
            {
                result.Append(string.Format($"{0}={1}&", nameof(Mode).ToLower(), param.Mode.Value.ToString().ToLower()));
            }

            if (param.Waypoints.Count > 0)
            {
                result.Append(string.Format($"{0}=", nameof(Waypoints).ToLower()));
                if (param.Optimize != null && param.Optimize.Value)
                {
                    result.Append(string.Format($"{0}:true|", nameof(Optimize).ToLower()));
                }

                foreach (var waypoint in param.Waypoints)
                {
                    result.Append(string.Format($"{0}|", waypoint));
                }

                result[result.Length - 1] = '&';
            }

            if (param.Alternatives != null && param.Alternatives.Value)
            {
                result.Append(string.Format($"{0}=true&", nameof(Alternatives).ToLower()));
            }

            if (param.Avoid != null)
            {
                result.Append(string.Format($"{0}={1}&", nameof(Avoid).ToLower(), param.Avoid.Value.ToString().ToLower()));
            }

            if (param.Units != null)
            {
                result.Append(string.Format($"{0}={1}&", nameof(Units).ToLower(), param.Units.Value.ToString().ToLower()));
            }

            if (param.ArrivalTime != null)
            {
                var date = param.ArrivalTime.Value.Date;
                result.Append(string.Format($"arrival_time={0}{1}{2}&", date.Month, date.Day, date.Year));
            }

            if(param.DepartureTime != null)
            {
                var date = param.DepartureTime.Value.Date;
                result.Append(string.Format($"departure_time={0}{1}{2}&", date.Date, date.Day, date.Year));
            }

            if (param.TrafficModel != null && param.TrafficModel.Value != TrafficModels.BestGuess)
            {
                result.Append(string.Format($"traffic_model={1}&", param.TrafficModel.Value.ToString().ToLower()));
            }

            if (param.TransitMode != null)
            {
                result.Append(string.Format($"transit_mode={1}&", param.TransitMode.Value.ToString().ToLower()));
            }

            if (param.TransitRoutingPreference != null)
            {
                string value;
                switch (param.TransitRoutingPreference.Value)
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
                result.Append(string.Format($"transit_routing_preference={1}&", value));
            }

            if (result[result.Length - 1] == '&')
                result = result.Remove(result.Length - 1, 1);
            return result.ToString();
        }
        public static implicit operator DirectionsParametrs(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                return new DirectionsParametrs();
            }

            var result = new DirectionsParametrs();
            var parametrs = param.Split('&');
            var fields = typeof(DirectionsParametrs).GetFields();
            foreach (var parametr in parametrs)
            {
                var keyAndValue = parametr.Split('=');
                var nameValue = new KeyValuePair<string, string>(keyAndValue[0], keyAndValue[1]);
                if (nameValue.Key.ToLower() == "waypoints")
                {
                    var value = nameValue.Value.Split('|');
                    var startIndex = 0;
                    if (value[0] == "true")
                    {
                        result.Optimize = true;
                        startIndex = 1;
                    }

                    for (var i = startIndex; i < value.Length; ++i)
                    {
                        result.Waypoints.Add(value[i]);
                    }
                    continue;
                }
                foreach (var field in fields)
                {
                    if (field.Name != nameValue.Key)
                    {
                        continue;
                    }

                    if (field.FieldType.IsEnum)
                    {
                        field.SetValue(result, Convert.ChangeType(nameValue.Value, field.FieldType));
                        break;
                    }

                    if (field.FieldType == typeof(DateTime))
                    {
                        var mounth = nameValue.Value.PadLeft(2);
                        var day = nameValue.Value.Remove(0, 2).Remove(2);
                        var year = nameValue.Value.PadRight(4);
                        var date = string.Format($"{0} {1} {2}", mounth, day, year);
                        field.SetValue(result, Convert.ChangeType(date, field.FieldType));
                        break;
                    }

                    field.SetValue(result, nameValue.Value);
                    break;
                }
            }

            return result;
        }
    }
}
