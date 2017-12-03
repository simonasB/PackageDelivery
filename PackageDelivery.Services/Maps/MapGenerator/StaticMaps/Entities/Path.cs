using System.Collections.Generic;
using PackageDelivery.Services.Maps.MapGenerator.Common;

namespace PackageDelivery.Services.Maps.MapGenerator.StaticMaps.Entities
{
	public class Path
	{
		public PathStyle Style { get; set; }

		public IList<ILocationString> Locations { get; set; }
	}
}