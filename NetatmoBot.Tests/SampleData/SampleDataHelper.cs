using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace NetatmoBot.Tests.SampleData
{
    public static class SampleDataHelper
    {
        public static string LoadSampleData(string name)
        {
            string resourceName = string.Format("NetatmoBot.Tests.SampleData.{0}", name);
            Trace.WriteLine("Loading sample data: " + resourceName);

            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName)) //GetFile(resourceName)) // GetManifestResourceStream?
            {
                if (stream == null)
                {
                    Trace.WriteLine("File not found: " + resourceName);
                    return string.Empty;
                }
                //Debug.Assert(stream != null, "File not found");

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}