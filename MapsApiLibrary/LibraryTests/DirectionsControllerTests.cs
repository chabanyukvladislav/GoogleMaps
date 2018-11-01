using System;
using System.Threading.Tasks;
using MapsApiLibrary;
using MapsApiLibrary.Api.Parameters.Directions;
using MapsApiLibrary.Api.Parameters.Directions.Enums;
using Xunit;

namespace LibraryTests
{
    public class DirectionsControllerTests
    {
        [Fact]
        public async Task GetResult_OriginDestinationKey_StatusOK()
        {
            //Arrange
            var controller = new DirectionsService();
            controller.Parameters.Origin = new Location(46.436458, 30.741378);
            controller.Parameters.Destination = new Location(46.437828, 30.726247);
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
            var controller = new DirectionsService();
            controller.Parameters.Origin = new Location(46.436458, 30.741378);
            controller.Parameters.Destination = new Location(46.437828, 30.726247);
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
            var controller = new DirectionsService();
            controller.Parameters.Origin = new Location(46.436458, 30.741378);
            controller.Parameters.Destination = new Location(46.437828, 30.726247);
            controller.Parameters.Key = "AIzaSyA3YhAyyckDAMFGuVR7yRI-fG_NATvL8Yk";
            controller.Parameters.DepartureTime = DateTime.Parse("11/01/2018 16:00");
            controller.Parameters.Optimize = true;
            controller.Parameters.Waypoints.Add(new Location(46.468184, 30.741464));
            controller.Parameters.Waypoints.Add(new Location(46.459931, 30.749942));

            //Act
            var result = await controller.GetResult();

            //Assert
            Assert.Equal("OK", result?.Status);
        }
    }
}
