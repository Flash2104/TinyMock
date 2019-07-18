using System;
using System.Collections.Generic;
using System.IO;

namespace TinyMock
{
    public class MockCollection
    {
        private readonly char[] _splitters = new char[] { ' ', ',', '.' };
        private readonly string _sourceText;
        public MockCollection(string sourceText)
        {
            if (sourceText == null)
            {
                _sourceText = String.Empty;
            }
            _sourceText = sourceText;
        }

        public MockCollection(Stream stream)
        {
            _sourceText = SpeechParser.Parse(stream);
        }

        public List<Mock> Get()
        {
            var words = _sourceText.Split(_splitters, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<Mock>();
            var wordBank = new List<string>();
            int temp = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if (TextMatcher.MatchVerb(words[i]))
                {
                    if (temp != i)
                    {
                        var mock = new Mock(wordBank.ToArray());
                        result.Add(mock);
                        temp = i;
                        wordBank.Clear();
                    }
                }
                wordBank.Add(words[i]);
            }
            result.Add(new Mock(wordBank.ToArray()));
            return result;
        }
    }
}