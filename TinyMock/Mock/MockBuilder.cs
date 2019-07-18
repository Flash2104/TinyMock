using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Google.Api;

namespace TinyMock
{
    public interface IMockGetter
    {
        MockModel GetMock();
    }

    internal class MockGetter : IMockGetter
    {
        public string Id { get; set; }
        public ActionType Action { get; set; }
        public Dictionary<string, object> PropertyValues { get; } = new Dictionary<string, object>();

        public MockModel GetMock()
        {
            return new MockModel()
            {
                Id = Id,
                Scheme = GetMockScheme(),
                Action = Action
            };
        }

        private object GetMockScheme()
        {
            var obj = new ExpandoObject() as IDictionary<string, object>;
            foreach (var pair in PropertyValues)
            {
                obj.Add(pair.Key, pair.Value);
            }
            return obj;
        }
    }

    public class MockBuilder
    {
        private readonly string[] _wordBank;

        public MockBuilder(string[] wordBank)
        {
            _wordBank = wordBank;
        }

        public IMockGetter BuildGetter()
        {
            MockGetter getter = new MockGetter();
            for (int i = 0; i < _wordBank.Length; i++)
            {

                if (TextMatcher.MatchIdentifier(_wordBank[i]))
                {
                    getter.Id = "element" + "." + _wordBank[i + 1];
                }
                else if (TextMatcher.MatchVerb(_wordBank[i]))
                {
                    getter.Action = MockModel.MapToActionType(_wordBank[i]);
                    if (getter.Action == ActionType.Clear)
                    {
                        return getter;
                    }
                }
                else if (TextMatcher.MatchMockType(_wordBank[i]))
                {
                    getter.PropertyValues["type"] = FirstCharToUpper(_wordBank[i]);
                }
                else if (TextMatcher.MatchPropertyType(_wordBank[i]))
                {
                    getter.PropertyValues[_wordBank[i]] = _wordBank[i + 1];
                    i++;
                }
                object obj;
                IDictionary<string, object> expobj = null;
                if (!getter.PropertyValues.TryGetValue("style", out obj))
                {
                    expobj = new ExpandoObject();
                }
                expobj = expobj ?? (IDictionary<string, object>)obj;
                if (TextMatcher.MatchBackgroundColor(_wordBank[i]))
                {
                    if (!TextMatcher.MatchColor(_wordBank[i + 1]))
                    {
                        i++;
                    }
                    else
                    {
                        i = i + 2;
                    }
                    expobj.Add("background-color", _wordBank[i]);
                }
                else if (TextMatcher.MatchColor(_wordBank[i]))
                {
                    expobj.Add("color", _wordBank[i + 1]);
                    i++;
                }
                getter.PropertyValues["style"] = expobj;
            }
            object type;
            if (getter.PropertyValues.TryGetValue("type", out type) || getter.Id != null)
            {
                return getter;
            }
            return new MockGetter();
        }
        public string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (string.Equals("button", input, StringComparison.OrdinalIgnoreCase))
            {
                return "button";
            }
            return input.First().ToString().ToUpper() + input.Substring(1) + "-" + "field";
        }
    }
}
