using System.Collections.Generic;
using PackageDelivery.Domain.Entities;
namespace PackageDelivery.WebApplication.Services.Maps {
    public interface IDistanceCalculator {
        /// <summary>
        /// Returns distance between two address in meters
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        long CalculateDistanceBetweenTwoPoints(Address origin, Address destination);
        PickUpPoint FindNearestPickUpPoint(Address origin, List<PickUpPoint> pickUpPoints);
    }
}
