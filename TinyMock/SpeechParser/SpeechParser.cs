using System;
using System.IO;
using Google.Cloud.Speech.V1;

namespace TinyMock
{
    internal static class SpeechParser
    {
        internal static string Parse(Stream stream)
        {
            var speech = SpeechClient.Create();
            var response = speech.Recognize(new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Flac,
                SampleRateHertz = 16000,
                LanguageCode = "en",
            }, RecognitionAudio.FromStream(stream));
            string resultText = string.Empty;
            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    resultText = alternative.Transcript;
                }
            }
            return resultText;
        }
    }
}