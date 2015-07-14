using System.Collections.Generic;
using System.Linq;
using NetatmoBot.Services.PublicDataModels;
using NetatmoBot.Tests.SampleData;
using Newtonsoft.Json;
using NUnit.Framework;

namespace NetatmoBot.Tests.Services.PublicDataModels
{
    /// <summary>
    /// Ensure that data returned from Netatmo api can be correctly loaded into PublicDataResponse object.
    /// </summary>
    [TestFixture]
    public class PublicDataResposeModelTest
    {
        // Data for station under test.
        // {"_id":"70:ee:50:06:bb:22","place":{"location":[0.00686,52.090706],"altitude":22.87738,"timezone":"Europe\/London"},"mark":13,"measures":{"02:00:00:06:a2:c0":{"res":{"1436109621":[21,79]},"type":["temperature","humidity"]},"05:00:00:01:53:a0":{"rain_60min":1.212,"rain_24h":1.212,"rain_live":0.808,"rain_timeutc":1436109640},"70:ee:50:06:bb:22":{"res":{"1436109650":[1014.4]},"type":["pressure"]}},"modules":["02:00:00:06:a2:c0","05:00:00:01:53:a0"]}
        private const string StationId = "70:ee:50:06:bb:22";
        private const string ModuleKeyWithRain = "05:00:00:01:53:a0";

        [Test]
        [TestCase("Example.PublicData.NoRain.json")]
        public void Populate_FromSampleData_WithoutRainMeasurements(string fileName)
        {
            // Arrange
            // Data expected to be returned from Netatmo api.
            string sampleData = SampleDataHelper.LoadSampleData(fileName);

            // Act
            var publicDataResponse = JsonConvert.DeserializeObject<PublicDataResponse>(sampleData);

            // Assert
            Assert.AreEqual("ok", publicDataResponse.status, "status");

            var body = publicDataResponse.body;

            Assert.IsNotNull(body, "body");
            Assert.AreEqual(544, body.Count);

            // Expect 204 rain sensor modules
            var rainModules = FindRainModules(body);
            Assert.AreEqual(203, rainModules.Count, "rainModules.Count");

            var rainMeasurementModule = FindModule(body, ModuleKeyWithRain);
            Assert.IsNotNull(rainMeasurementModule, "Expected to find rain module measurement.");

            // Rain module before it had rained
            var rainMeasurement = rainMeasurementModule.Value;
            Assert.AreEqual(0m, rainMeasurement.rain_live, "Rain live - not raining");
            Assert.AreEqual(0m, rainMeasurement.rain_24h, "rain_24h");
            Assert.AreEqual(0m, rainMeasurement.rain_60min, "rain_60min");
            Assert.AreEqual(1436098504, rainMeasurement.rain_timeutc, "rain_timeutc");
        }

        [Test]
        [TestCase("Example.PublicData.Rain1.json")]
        public void Populate_FromSampleData_WithRainMeasurements(string fileName)
        {
            // Arrange

            // Data expected to be returned from Netatmo api.
            // Total number of stations: 543
            // Number of stations with rain gauge: 204
            string sampleData = SampleDataHelper.LoadSampleData(fileName);

            // Act
            var publicDataResponse = JsonConvert.DeserializeObject<PublicDataResponse>(sampleData);

            // Assert
            Assert.AreEqual("ok", publicDataResponse.status, "status");

            var body = publicDataResponse.body;

            Assert.IsNotNull(body, "body");
            Assert.AreEqual(543, body.Count);

            // Expect 204 fain modules.
            var rainModules = FindRainModules(body);
            Assert.AreEqual(204, rainModules.Count, "rainModules.Count");

            ShowModules(rainModules);

            // Rain Module.
            var rainMeasurementModule = FindModule(body, ModuleKeyWithRain);
            Assert.IsNotNull(rainMeasurementModule, "Expected to find rain module measurement.");

            // Rain module whilst it was raining.
            var rainMeasurement = rainMeasurementModule.Value;
            Assert.AreEqual(0.808m, rainMeasurement.rain_live, "Rain live");
            Assert.AreEqual(1.212m, rainMeasurement.rain_24h, "rain_24h");
            Assert.AreEqual(1.212m, rainMeasurement.rain_60min, "rain_60min");
            Assert.AreEqual(1436109640, rainMeasurement.rain_timeutc, "rain_timeutc");
        }

        /// <summary>
        /// Examine the full station properties.
        /// </summary>
        /// <param name="fileName"></param>
        [Test]
        [TestCase("Example.PublicData.Rain1.json")]
        public void Populate_FromSampleData_SetsStationProperties(string fileName)
        {
            // Arrange
            // Data expected to be returned from Netatmo api.
            // Total number of stations: 543
            // Number of stations with rain gauge: 204
            string sampleData = SampleDataHelper.LoadSampleData(fileName);

            // Act
            var publicDataResponse = JsonConvert.DeserializeObject<PublicDataResponse>(sampleData);

            // Assert
            Assert.AreEqual("ok", publicDataResponse.status, "status");

            var body = publicDataResponse.body;

            Assert.IsNotNull(body, "body");
            Assert.AreEqual(543, body.Count);

            // Just assert on the first item.
            PublicDataResponseItem publicDataResponseItem = body.First(x => x._id == StationId);

            // {"_id":"70:ee:50:06:bb:22",
            Assert.AreEqual(StationId, publicDataResponseItem._id, "_id");

            AssertPlace(publicDataResponseItem);

            // Assert measures.
            Assert.IsNotNull(publicDataResponseItem.measures, "measures");
            Assert.AreEqual(3, publicDataResponseItem.measures.Count, "measures.Count");

            AssertOutsideModule(publicDataResponseItem);

            AssertRainModule(publicDataResponseItem);

            AssertPressureModule(publicDataResponseItem);

            AssertModules(publicDataResponseItem);
        }

        private static void AssertPlace(PublicDataResponseItem publicDataResponseItem)
        {
// Assert place.
            // "place":{"location":[0.00686,52.090706],"altitude":22.87738,"timezone":"Europe\/London"},"mark":13,"measures":{"02:00:00:06:a2:c0":{"res":{"1436109621":[21,79]},"type":["temperature","humidity"]},"05:00:00:01:53:a0":{"rain_60min":1.212,"rain_24h":1.212,"rain_live":0.808,"rain_timeutc":1436109640},"70:ee:50:06:bb:22":{"res":{"1436109650":[1014.4]},"type":["pressure"]}},"modules":["02:00:00:06:a2:c0","05:00:00:01:53:a0"]}
            Place place = publicDataResponseItem.place;
            Assert.AreEqual(0.00686D, place.location[0], 0.00001D, "place.location[0] - long");
            Assert.AreEqual(52.090706D, place.location[1], 0.00001D, "place.location[1] - lat");
            Assert.AreEqual("Europe/London", place.timezone, "timezone");
        }

        private static void AssertOutsideModule(PublicDataResponseItem publicDataResponseItem)
        {
// Outside Temperature/Humidity
            // "measures":{"02:00:00:06:a2:c0":{"res":{"1436109621":[21,79]},"type":["temperature","humidity"]},
            Measurement temperatureMeasurement = publicDataResponseItem.measures["02:00:00:06:a2:c0"];
            Assert.IsNotNull(temperatureMeasurement, "First measure.key");

            // result and types are index and align.
            var temperatureResults = temperatureMeasurement.res["1436109621"];
            Assert.AreEqual(21, temperatureResults[0], "results[0] - temperature");
            Assert.AreEqual(79, temperatureResults[1], "results[1] - humidity");

            var temperatureTypes = temperatureMeasurement.type;
            Assert.AreEqual("temperature", temperatureTypes[0], "type[0]");
            Assert.AreEqual("humidity", temperatureTypes[1], "type[1]");
        }

        private static void AssertRainModule(PublicDataResponseItem publicDataResponseItem)
        {
// Rain Gauge
            // "05:00:00:01:53:a0":{"rain_60min":1.212,"rain_24h":1.212,"rain_live":0.808,"rain_timeutc":1436109640},
            Measurement rainMeasurement = publicDataResponseItem.measures["05:00:00:01:53:a0"];
            Assert.IsNull(rainMeasurement.res, "rainMeasurement.res != null");
            Assert.IsNull(rainMeasurement.type, "rainMeasurement.type != null");
            Assert.AreEqual(1.212M, rainMeasurement.rain_24h, "rainMeasurement.rain_24h");
            Assert.AreEqual(1.212M, rainMeasurement.rain_60min, "rainMeasurement.rain_60min");
            Assert.AreEqual(0.808M, rainMeasurement.rain_live, "rainMeasurement.rain_live");
            Assert.AreEqual(1436109640, rainMeasurement.rain_timeutc, "rainMeasurement.rain_timeutc");
        }

        private static void AssertPressureModule(PublicDataResponseItem publicDataResponseItem)
        {
// Pressure Gauge
            // "70:ee:50:06:bb:22":{"res":{"1436109650":[1014.4]},"type":["pressure"]}},
            Measurement pressureMeasurement = publicDataResponseItem.measures["70:ee:50:06:bb:22"];
            Assert.IsNotNull(pressureMeasurement, "pressureMeasurement != null");
            Assert.IsNotNull(pressureMeasurement.res, "pressureMeasurement.res != null");
            Assert.AreEqual(1, pressureMeasurement.res.Count, "pressureMeasurement.res.Count");
            Assert.IsNotNull(pressureMeasurement.type, "pressureMeasurement.type != null");
            Assert.AreEqual(1, pressureMeasurement.type.Length, "pressureMeasurement.type.Count");

            var pressureResults = pressureMeasurement.res["1436109650"];
            Assert.AreEqual(1014.4M, pressureResults[0], "pressureResults[0]");

            var pressureTypes = pressureMeasurement.type;
            Assert.AreEqual("pressure", pressureTypes[0], "pressureTypes[0]");
        }

        private static void AssertModules(PublicDataResponseItem publicDataResponseItem)
        {
//"modules":["02:00:00:06:a2:c0","05:00:00:01:53:a0"]}
            Assert.AreEqual(2, publicDataResponseItem.modules.Length, "modules.Length");
            Assert.AreEqual("02:00:00:06:a2:c0", publicDataResponseItem.modules[0], "modules[0]");
            Assert.AreEqual("05:00:00:01:53:a0", publicDataResponseItem.modules[1], "modules[1]");
        }

        /// <summary>
        /// Find a module by its key
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="moduleKeyWithRain"></param>
        /// <returns></returns>
        private KeyValuePair<string, Measurement> FindModule(List<PublicDataResponseItem> stations, string moduleKeyWithRain)
        {
            return (from station in stations
                    from measurement in station.measures
                    where measurement.Key == moduleKeyWithRain
                    select measurement).First();
        }

        /// <summary>
        /// Find a rain module
        /// </summary>
        /// <param name="stations"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, Measurement>> FindRainModules(List<PublicDataResponseItem> stations)
        {
            return (from station in stations
                    from measurement in station.measures
                    where measurement.Value.IsRain()
                    select measurement).ToList();
        }


        // Debug to help find module with rain
        private void ShowModules(List<KeyValuePair<string, Measurement>> modules)
        {

            //foreach (var module in rainModules)
            //{
            //    var measurement = module.Value;
            //    string message = string.Format("Rain Module: {0}. Rain Live: {1}, 60min: {2}, 24h: {3}", module.Key, measurement.rain_live, measurement.rain_60min, measurement.rain_24h);
            //    Trace.WriteLine(message);
            //}
        }
    }
}