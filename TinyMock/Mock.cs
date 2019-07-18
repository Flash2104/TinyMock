using System;
using System.IO;

namespace TinyMock
{
    public class Mock
    {
        private readonly string[] _wordBank;

        public Mock(string[] wordBank)
        {
            if (wordBank == null)
            {
                _wordBank = new string[0];
            }
            _wordBank = wordBank;
        }

        public MockModel Get()
        {
            var builder = new MockBuilder(_wordBank);
            var getter = builder.BuildGetter();
            return getter.GetMock();
        }
    }
}