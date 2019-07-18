using System;
using System.IO;

namespace TinyMock
{
    public class MockApp
    {
        public static readonly string tdb = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp", "tdb.json");

        public static void Init()
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var tempRoot = Path.Combine(root, "temp");
            if (!Directory.Exists(tempRoot))
            {
                Directory.CreateDirectory(tempRoot);
            }
            if (!File.Exists(tdb))
            {
                using (var fs = File.Create(tdb))
                {
                    fs.Write(new byte[0], 0, 0);
                }
            }

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GoogleSpeechTest-e098b10eb7db.json"));
        }

    }
}