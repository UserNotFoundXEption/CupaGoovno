// Source: https://gist.github.com/darktable/1411710 (edited)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace CupaGoovno;

public static class Json
{
    public static object Deserialize(string json)
    {
        if (json == null) return null;
        return Parser.Parse(json);
    }

    public static string Serialize(object obj)
    {
        return Serializer.Serialize(obj);
    }

    sealed class Parser : IDisposable
    {
        const string WORD_BREAK = "{}[],:\"";

        public static bool IsWordBreak(char c)
        {
            return char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1;
        }

        StringReader json;

        Parser(string jsonString)
        {
            json = new StringReader(jsonString);
        }

        public static object Parse(string jsonString)
        {
            using (var instance = new Parser(jsonString))
            {
                return instance.ParseValue();
            }
        }

        public void Dispose()
        {
            json.Dispose();
            json = null;
        }

        Dictionary<string, object> ParseObject()
        {
            Dictionary<string, object> table = new Dictionary<string, object>();

            json.Read(); // consume '{'

            while (true)
            {
                TOKEN nextToken = NextToken;

                if (nextToken == TOKEN.NONE)
                    return null;

                if (nextToken == TOKEN.CURLY_CLOSE)
                {
                    json.Read(); // consume '}'
                    return table;
                }

                string name = ParseString();

                if (name == null)
                    return null;

                if (NextToken != TOKEN.COLON)
                    return null;

                json.Read(); // consume ':'

                object value = ParseValue();
                table[name] = value;

                nextToken = NextToken;

                if (nextToken == TOKEN.COMMA)
                {
                    json.Read(); // consume ',' and continue
                    continue;
                }
                else if (nextToken == TOKEN.CURLY_CLOSE)
                {
                    json.Read(); // consume '}'
                    return table;
                }
                else
                {
                    return null; // unexpected token
                }
            }
        }

        List<object> ParseArray()
        {
            List<object> array = new List<object>();

            json.Read(); // [

            var parsing = true;
            while (parsing)
            {
                TOKEN nextToken = NextToken;

                switch (nextToken)
                {
                    case TOKEN.NONE:
                        return null;
                    case TOKEN.SQUARE_CLOSE:
                        parsing = false;
                        break;
                    default:
                        array.Add(ParseValue());
                        break;
                }

                if (parsing)
                {
                    switch (NextToken)
                    {
                        case TOKEN.COMMA:
                            json.Read();
                            break;
                        case TOKEN.SQUARE_CLOSE:
                            json.Read();
                            parsing = false;
                            break;
                        default:
                            return null;
                    }
                }
            }

            return array;
        }

        object ParseValue()
        {
            switch (NextToken)
            {
                case TOKEN.STRING:
                    return ParseString();
                case TOKEN.NUMBER:
                    return ParseNumber();
                case TOKEN.CURLY_OPEN:
                    return ParseObject();
                case TOKEN.SQUARE_OPEN:
                    return ParseArray();
                case TOKEN.TRUE:
                    ReadWord("true");
                    return true;
                case TOKEN.FALSE:
                    ReadWord("false");
                    return false;
                case TOKEN.NULL:
                    json.Read();
                    return null;
                default:
                    return null;
            }
        }

        string ParseString()
        {
            StringBuilder s = new StringBuilder();
            char c;

            json.Read(); // "

            bool parsing = true;
            while (parsing)
            {
                if (json.Peek() == -1)
                {
                    break;
                }

                c = NextChar;
                switch (c)
                {
                    case '\"':
                        parsing = false;
                        break;
                    case '\\':
                        if (json.Peek() == -1) break;

                        c = NextChar;
                        switch (c)
                        {
                            case '\"': s.Append('\"'); break;
                            case '\\': s.Append('\\'); break;
                            case '/': s.Append('/'); break;
                            case 'b': s.Append('\b'); break;
                            case 'f': s.Append('\f'); break;
                            case 'n': s.Append('\n'); break;
                            case 'r': s.Append('\r'); break;
                            case 't': s.Append('\t'); break;
                            case 'u':
                                var hex = new char[4];
                                for (int i = 0; i < 4; i++) hex[i] = NextChar;
                                s.Append((char)Convert.ToInt32(new string(hex), 16));
                                break;
                        }
                        break;
                    default:
                        s.Append(c);
                        break;
                }
            }

            return s.ToString();
        }

        object ParseNumber()
        {
            string number = NextWord;

            if (number.IndexOf('.') == -1)
            {
                long.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out long parsedInt);
                return parsedInt;
            }

            double.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedDouble);
            return parsedDouble;
        }

        void EatWhitespace()
        {
            while (char.IsWhiteSpace(PeekChar)) json.Read();
        }

        char PeekChar => Convert.ToChar(json.Peek());

        char NextChar => Convert.ToChar(json.Read());

        string NextWord
        {
            get
            {
                StringBuilder word = new StringBuilder();
                while (!IsWordBreak(PeekChar)) word.Append(NextChar);
                return word.ToString();
            }
        }

        TOKEN NextToken
        {
            get
            {
                EatWhitespace();
                if (json.Peek() == -1) return TOKEN.NONE;

                char c = PeekChar;
                switch (c)
                {
                    case '{': return TOKEN.CURLY_OPEN;
                    case '}': return TOKEN.CURLY_CLOSE;
                    case '[': return TOKEN.SQUARE_OPEN;
                    case ']': return TOKEN.SQUARE_CLOSE;
                    case ',': return TOKEN.COMMA;
                    case '\"': return TOKEN.STRING;
                    case ':': return TOKEN.COLON;
                    case 't': return TOKEN.TRUE;
                    case 'f': return TOKEN.FALSE;
                    case 'n': return TOKEN.NULL;
                    default:
                        return char.IsDigit(c) || c == '-' ? TOKEN.NUMBER : TOKEN.NONE;
                }
            }
        }

        enum TOKEN
        {
            NONE, CURLY_OPEN, CURLY_CLOSE, SQUARE_OPEN, SQUARE_CLOSE,
            COLON, COMMA, STRING, NUMBER, TRUE, FALSE, NULL
        }

        void ReadWord(string expected)
        {
            foreach (char c in expected)
            {
                if (json.Read() != c)
                    throw new Exception($"Unexpected character while reading '{expected}'");
            }
        }
    }

    sealed class Serializer
    {
        StringBuilder builder;

        Serializer() => builder = new StringBuilder();

        public static string Serialize(object obj)
        {
            var instance = new Serializer();
            instance.SerializeValue(obj);
            return instance.builder.ToString();
        }

        void SerializeValue(object value)
        {
            switch (value)
            {
                case null: builder.Append("null"); break;
                case string s: SerializeString(s); break;
                case bool b: builder.Append(b ? "true" : "false"); break;
                case IList list: SerializeArray(list); break;
                case IDictionary<string, object> dict: SerializeObject(dict); break;
                case char c: SerializeString(c.ToString()); break;
                default:
                    if (value is double || value is float || value is long || value is int)
                        builder.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
                    else
                        SerializeString(value.ToString());
                    break;
            }
        }

        void SerializeObject(IDictionary<string, object> obj)
        {
            bool first = true;
            builder.Append('{');

            foreach (var kvp in obj)
            {
                if (!first) builder.Append(',');
                SerializeString(kvp.Key);
                builder.Append(':');
                SerializeValue(kvp.Value);
                first = false;
            }

            builder.Append('}');
        }

        void SerializeArray(IList anArray)
        {
            builder.Append('[');
            bool first = true;

            foreach (var obj in anArray)
            {
                if (!first) builder.Append(',');
                SerializeValue(obj);
                first = false;
            }

            builder.Append(']');
        }

        void SerializeString(string str)
        {
            builder.Append('\"');

            foreach (var c in str)
            {
                switch (c)
                {
                    case '\"': builder.Append("\\\""); break;
                    case '\\': builder.Append("\\\\"); break;
                    case '\b': builder.Append("\\b"); break;
                    case '\f': builder.Append("\\f"); break;
                    case '\n': builder.Append("\\n"); break;
                    case '\r': builder.Append("\\r"); break;
                    case '\t': builder.Append("\\t"); break;
                    default:
                        if (char.IsControl(c))
                            builder.Append($"\\u{(int)c:X4}");
                        else
                            builder.Append(c);
                        break;
                }
            }

            builder.Append('\"');
        }
    }
}
