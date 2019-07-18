using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TinyMock
{
    public static class Tdb
    {
        private static readonly JsonSerializer _serializer = JsonSerializer.Create();
        private static readonly string tdb = MockApp.tdb;

        public static List<MockModel> MockSet => GetMockSet();

        public static void UpdateSet(List<MockModel> value)
        {
            using (var sw = new StreamWriter(tdb))
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    _serializer.Serialize(writer, value);
                }
            }
        }

        private static List<MockModel> GetMockSet()
        {
            List<MockModel> result;
            using (var fs = File.Open(tdb, FileMode.Open))
            using (var sr = new StreamReader(fs))
            using (var jsonReader = new JsonTextReader(sr))
            {
                result = _serializer.Deserialize<List<MockModel>>(jsonReader) ?? new List<MockModel>();
            }
            return result;
        }
    }
}