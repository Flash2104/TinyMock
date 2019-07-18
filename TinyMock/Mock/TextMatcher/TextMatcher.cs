using System.Text.RegularExpressions;

namespace TinyMock
{
    public static class TextMatcher
    {
        static class Patterns
        {
            public static Regex identifier = new Regex(@"\belement\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            public static Regex verbs = new Regex(@"\b(create|edit|update|delete|add|move|remove|clean|clear)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            public static Regex containers = new Regex(@"\b(vertical|horizontal|container)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            public static Regex types = new Regex(@"\b(group|button|popout|pop|popup|text|staticText|checkbox|label|boolean|bool|grid)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            public static Regex properties = new Regex(@"\b(layout|title|value)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            public static Regex style = new Regex(@"\bstyle\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            public static Regex backgroundColor = new Regex(@"\bbackground\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            public static Regex color = new Regex(@"\bcolor\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static bool MatchIdentifier(string word)
        {
            return Patterns.identifier.IsMatch(word);
        }

        public static bool MatchVerb(string word)
        {
            return Patterns.verbs.IsMatch(word);
        }

        public static bool MatchContainer(string word)
        {
            return Patterns.containers.IsMatch(word);
        }

        public static bool MatchMockType(string word)
        {
            return Patterns.types.IsMatch(word);
        }

        public static bool MatchPropertyType(string word)
        {
            return Patterns.properties.IsMatch(word);
        }

        public static bool MatchStyle(string word)
        {
            return Patterns.style.IsMatch(word);
        }
        public static bool MatchBackgroundColor(string word)
        {
            return Patterns.backgroundColor.IsMatch(word);
        }
        public static bool MatchColor(string word)
        {
            return Patterns.color.IsMatch(word);
        }
    }
}