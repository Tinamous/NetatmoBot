using NetatmoBot.Model;
using NetatmoBot.Services.PublicDataModels;

namespace NetatmoBot.Services.Mapping
{
    public static class StationPlaceMapper
    {
        public static StationPlace Map(Place place)
        {
            return new StationPlace
            {
                Altitude = place.altitude,
                // Appears Netatmo returns as [long,lat]
                Longitude = place.location[0],
                Lattitude = place.location[1],
                
                Timezone = place.timezone
            };
        }
    }
}