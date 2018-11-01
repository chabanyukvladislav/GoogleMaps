using System;
using System.Threading.Tasks;
using MapsApiLibrary;
using MapsApiLibrary.Api.Parameters.Directions;
using MapsApiLibrary.Api.Parameters.Directions.Enums;
using MapsApiLibrary.Models.Directions;
using Xunit;

namespace LibraryTests
{
    public class DirectionsControllerTests
    {
        [Fact]
        public async Task GetResult_OriginDestinationKey_StatusOK()
        {
            //Arrange
            var controller = new DirectionsController();
            controller.Parameters.Origin = new Location
            {
                Latitude = 46.436458,
                Longitude = 30.741378,
            };
            controller.Parameters.Destination = new Location
            {
                Latitude = 46.437828,
                Longitude = 30.726247,
            };
            controller.Parameters.Key = "AIzaSyA3YhAyyckDAMFGuVR7yRI-fG_NATvL8Yk";

            //Act
            var result = await controller.GetResult();

            //Assert
            Assert.Equal("OK", result?.Status);
        }

        [Fact]
        public async Task GetResult_OriginDestinationKeyModeDepartureTime_StatusOK()
        {
            //Arrange
            var controller = new DirectionsController();
            controller.Parameters.Origin = new Location
            {
                Latitude = 46.436458,
                Longitude = 30.741378,
            };
            controller.Parameters.Destination = new Location
            {
                Latitude = 46.437828,
                Longitude = 30.726247,
            };
            controller.Parameters.Key = "AIzaSyA3YhAyyckDAMFGuVR7yRI-fG_NATvL8Yk";
            controller.Parameters.Mode = Modes.Transit;
            controller.Parameters.DepartureTime = DateTime.Parse("11/01/2018 16:00");

            //Act
            var result = await controller.GetResult();

            //Assert
            Assert.Equal("OK", result?.Status);
        }
        
        [Fact]
        public async Task GetResult_OriginDestinationKeyWaypointsDepartureTime_StatusOK()
        {
            //Arrange
            var controller = new DirectionsController();
            controller.Parameters.Origin = new Location
            {
                Latitude = 46.436458,
                Longitude = 30.741378,
            };
            controller.Parameters.Destination = new Location
            {
                Latitude = 46.437828,
                Longitude = 30.726247,
            };
            controller.Parameters.Key = "AIzaSyA3YhAyyckDAMFGuVR7yRI-fG_NATvL8Yk";
            controller.Parameters.DepartureTime = DateTime.Parse("11/01/2018 16:00");
            Waypoint.Optimize = true;
            controller.Parameters.Waypoints.Add(new Waypoint
            {
                Location = new Location()
                {
                    Latitude = 46.468184,
                    Longitude = 30.741464,
                }
            });
            controller.Parameters.Waypoints.Add(new Waypoint
            {
                Location = new Location()
                {
                    Latitude = 46.459931,
                    Longitude = 30.749942,
                }
            });

            //Act
            var result = await controller.GetResult();

            //Assert
            Assert.Equal("OK", result?.Status);
        }
    }
}
