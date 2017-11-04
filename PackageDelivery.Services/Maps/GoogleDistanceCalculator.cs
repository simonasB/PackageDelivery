using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using PackageDelivery.Domain.Entities;
using PackageDelivery.SharedKernel.Data.Maps;
using PackageDelivery.WebApplication.Services.Maps.Google.DistanceMatrix;
using PackageDelivery.WebApplication.Utils;

namespace PackageDelivery.WebApplication.Services.Maps {
    
    public class GoogleDistanceCalculator : GoogleServicesBase, IDistanceCalculator {
        public GoogleDistanceCalculator(HttpClient httpClient) : base(httpClient) {
        }

        public long CalculateDistanceBetweenTwoPoints(Address origin, Address destination) {
            var uri = new Uri(
                $"{BaseUri}/distancematrix/json?origins={origin}&destinations={destination}&mode=driving&key={APIKey}");

            var result = ExecuteRequest<GetDistanceMatrixResponse>(uri);

            return result?.Rows[0].Elements[0].Distance.Value ?? 10000;
        }

        public PickUpPoint FindNearestPickUpPoint(Address origin, List<PickUpPoint> pickUpPoints) {
            var destinations = Converter.JoinList(pickUpPoints.Select(o => o.Address.ToString()));
            var uri = new Uri(
                $"{BaseUri}/distancematrix/json?origins={origin}&destinations={destinations}&mode=driving&key={APIKey}");

            var result = ExecuteRequest<GetDistanceMatrixResponse>(uri);

            long minDistance = result.Rows[0].Elements[0].Distance.Value;
            int minDistanceIndex = 0;

            for (int i = 0; i < result.Rows[0].Elements.Count; i++) {
                if (minDistance > result.Rows[0].Elements[i].Distance.Value) {
                    minDistance = result.Rows[0].Elements[i].Distance.Value;
                    minDistanceIndex = i;
                }
            }

            return pickUpPoints[minDistanceIndex];
        }
    }
}
