/*
    eduJSON - Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace eduJSON
{
    /// <summary>
    /// JSON parser
    /// </summary>
    public class Parser
    {
        #region Methods

        /// <summary>
        /// Parses the input JSON string <paramref name="str"/> and builds an object tree representing JSON data.
        /// </summary>
        /// <param name="str">The JSON string to parse</param>
        /// <param name="ct">The token to monitor for cancellation requests</param>
        /// <returns>An object representing JSON data</returns>
        public static object Parse(string str, CancellationToken ct = default(CancellationToken))
        {
            int idx = 0;

            // Skip leading spaces and comments.
            SkipSpace(str, ref idx);

            // Parse the root value.
            object obj = ParseValue(str, ref idx, ct);

            // Skip trailing spaces and comments.
            SkipSpace(str, ref idx);
            if (idx < str.Length)
                throw new TrailingDataException(str, idx);

            return obj;
        }

        #region Parsing Primitives

        /// <summary>
        /// Parses the constant value <paramref name="keyword"/> encoded as JSON string <paramref name="str"/>. Used for parsing "true", "false" and "null" values.
        /// </summary>
        /// <param name="str">The JSON string to parse</param>
        /// <param name="idx">Starting index in <paramref name="str"/></param>
        /// <param name="keyword">Expected keyword. Should be all-lowercase.</param>
        /// <returns><c>true</c> when JSON string <paramref name="str"/> at <paramref name="idx"/> matches the keyword <paramref name="keyword"/>; <c>false</c> otherwise.</returns>
        /// <remarks>The JSON string <paramref name="str"/> is converted to lowercase for matching only. Therefore <paramref name="keyword"/> should be given all-lowercase.</remarks>
        private static bool ParseKeyword(string str, ref int idx, string keyword)
        {
            int len = keyword.Length;

            if (idx + len <= str.Length && str.Substring(idx, len).ToLower() == keyword)
            {
                // Keyword found. Check that non-identifier character follows.
                int i = idx + len;
                if (i >= str.Length || (!char.IsLetterOrDigit(str[i]) && str[i] != '_'))
                {
                    idx += len;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Parses the integer or floating number encoded as JSON string <paramref name="str"/>.
        /// </summary>
        /// <param name="str">The JSON string to parse</param>
        /// <param name="idx">Starting index in <paramref name="str"/></param>
        /// <returns>The number of type <see cref="int"/> or <see cref="double"/> (depending on JSON string <paramref name="str"/> at <paramref name="idx"/>); or <c>null</c> if not-a-number.</returns>
        private static object ParseNumber(string str, ref int idx)
        {
            int i = idx, n = str.Length;

            if (i < n)
            {
                bool positive;
                if (str[i] == '-')
                {
                    // The number is negative.
                    positive = false;
                    i++;
                }
                else if (str[i] == '+')
                {
                    // JSON EXT: Explicit positive sign
                    positive = true;
                    i++;
                }
                else
                {
                    // We are positive by default. :)
                    positive = true;
                }

                if (i < n)
                {
                    uint value;
                    if (str[i] == '0')
                    {
                        // The integer part is 0.
                        value = 0;
                        i++;
                    }
                    else if ('1' <= str[i] && str[i] <= '9')
                    {
                        value = (uint)(str[i] - '0');
                        i++;

                        // Parse rest of the number.
                        for (; i < n && '0' <= str[i] && str[i] <= '9'; i++)
                            value = value * 10 + (uint)(str[i] - '0');
                    }
                    else
                        return null;

                    double value_f = value;
                    bool is_f = false;

                    if (i < n && str[i] == '.')
                    {
                        // Digital part.
                        i++;
                        if (i < n && '0' <= str[i] && str[i] <= '9')
                        {
                            uint c = 10, digital = (uint)(str[i] - '0');
                            i++;

                            // Parse the digital part.
                            for (; i < n && '0' <= str[i] && str[i] <= '9'; i++)
                            {
                                digital = digital * 10 + (uint)(str[i] - '0');
                                c *= 10;
                            }

                            value_f += (double)digital / c;
                            is_f = true;
                        }
                        else
                            return null;
                    }

                    if (i < n && (str[i] == 'E' || str[i] == 'e'))
                    {
                        // Exponential part.
                        i++;
                        bool e_positive;
                        if (str[i] == '-')
                        {
                            // The exponent will be negative.
                            e_positive = false;
                            i++;
                        }
                        else if (str[i] == '+')
                        {
                            // The exponent will be positive.
                            e_positive = true;
                            i++;
                        }
                        else
                        {
                            // Default exponent sign is positive.
                            e_positive = true;
                        }

                        if (i < n && '0' <= str[i] && str[i] <= '9')
                        {
                            int exp = (int)(str[i] - '0');
                            i++;

                            // Parse rest of the number.
                            for (; i < n && '0' <= str[i] && str[i] <= '9'; i++)
                                exp = exp * 10 + (int)(str[i] - '0');

                            value_f *= Math.Pow(10, e_positive ? exp : -exp);
                            is_f = true;
                        }
                        else
                            return null;
                    }

                    idx = i;
                    if (is_f)
                    {
                        // Return number as "double".
                        return positive ? value_f : -value_f;
                    }
                    else
                    {
                        // Return number as unsigned integer.
                        return positive ? (int)value : -(int)value;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Parses the string encoded as JSON string <paramref name="str"/>.
        /// </summary>
        /// <param name="str">The JSON string to parse</param>
        /// <param name="idx">Starting index in <paramref name="str"/></param>
        /// <returns>The string of type <see cref="string"/>; or <c>null</c> if JSON string <paramref name="str"/> at <paramref name="idx"/> does not represent a string.</returns>
        private static object ParseString(string str, ref int idx)
        {
            int i = idx, n = str.Length;

            if (i < n && str[i] == '"')
            {
                // Opening quote found.
                i++;
                StringBuilder res = new StringBuilder(n);
                for (; i < n; )
                {
                    char chr = str[i];
                    if (chr == '"')
                    {
                        // Closing quote found.
                        idx = i + 1;
                        return res.ToString();
                    }
                    else if (chr == '\\')
                    {
                        // Escape sequence found.
                        i++;
                        switch (str[i])
                        {
                            case '"': res.Append('"'); i++; break;
                            case '\\': res.Append('\\'); i++; break;
                            case '/': res.Append('/'); i++; break;
                            case 'b': res.Append('\b'); i++; break;
                            case 'f': res.Append('\f'); i++; break;
                            case 'n': res.Append('\n'); i++; break;
                            case 'r': res.Append('\r'); i++; break;
                            case 't': res.Append('\t'); i++; break;
                            case 'u':
                                i++;
                                uint unicode = 0;
                                for (uint count = 0; count < 4 && i < n; i++, count++)
                                {
                                    chr = str[i];
                                    if ('0' <= chr && chr <= '9')
                                        unicode = unicode * 16 + (uint)(chr - '0');
                                    else if ('a' <= chr && chr <= 'f')
                                        unicode = unicode * 16 + (uint)(chr - 'a') + 10;
                                    else if ('A' <= chr && chr <= 'F')
                                        unicode = unicode * 16 + (uint)(chr - 'A') + 10;
                                    else
                                    {
                                        // JSON EXT: Shorter than 4-hexadecimal Unicode codes
                                        break;
                                    }
                                }
                                res.Append((char)unicode);
                                break;

                            default:
                                // JSON EXT: Ignore invalid escape sequence
                                res.Append('\\');
                                res.Append(str[i]); i++;
                                break;
                        }
                    }
                    else
                    {
                        // JSON EXT: Control characters in strings
                        res.Append(chr); i++;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Parses the field identifier encoded as JSON string <paramref name="str"/>.
        /// </summary>
        /// <param name="str">The JSON string to parse</param>
        /// <param name="idx">Starting index in <paramref name="str"/></param>
        /// <returns>The string of type <see cref="string"/>; or <c>null</c> if JSON string <paramref name="str"/> at <paramref name="idx"/> does not represent an identifier.</returns>
        private static object ParseIdentifier(string str, ref int idx)
        {
            int i = idx, n = str.Length;

            if (i < n && str[i] == '"')
            {
                // Identifier is encoded as quoted string.
                return ParseString(str, ref idx);
            }
            else
            {
                // JSON EX: Non-quoted identifiers
                for (; i < n && (char.IsLetterOrDigit(str[i]) || str[i] == '_'); i++) ;

                if (idx < i)
                {
                    string res = str.Substring(idx, i - idx);
                    idx = i;
                    return res;
                }
            }

            return null;
        }

        #endregion

        #region Parsing Helpers

        /// <summary>
        /// Parses the value encoded as JSON string <paramref name="str"/>.
        /// </summary>
        /// <param name="str">The JSON string to parse</param>
        /// <param name="idx">Starting index in <paramref name="str"/></param>
        /// <param name="ct">The token to monitor for cancellation requests</param>
        /// <returns>An object representing JSON value</returns>
        private static object ParseValue(string str, ref int idx, CancellationToken ct)
        {
            if (ParseKeyword(str, ref idx, "true"))
            {
                // A logical value "true" was found.
                return true;
            }

            if (ParseKeyword(str, ref idx, "false"))
            {
                // A logical value "false" was found.
                return false;
            }

            if (ParseKeyword(str, ref idx, "null"))
            {
                // A "null" was found.
                return null;
            }

            {
                object obj = ParseNumber(str, ref idx);
                if (obj != null)
                    return obj;
            }

            {
                object obj = ParseString(str, ref idx);
                if (obj != null)
                    return obj;
            }

            switch (str[idx])
            {
                case '[':
                    {
                        // An array was found.
                        int array_origin = idx;
                        List<object> obj = new List<object>();
                        bool is_empty = true, has_separator = false;

                        for (idx++; idx < str.Length;)
                        {
                            ct.ThrowIfCancellationRequested();

                            // Skip leading spaces and comments.
                            SkipSpace(str, ref idx);

                            if (idx < str.Length && str[idx] == ']')
                            {
                                // This is the end.
                                idx++;
                                return obj;
                            }
                            else if (is_empty || has_separator)
                            {
                                // Analyse value recursively, and add it.
                                obj.Add(ParseValue(str, ref idx, ct));
                                is_empty = false;

                                // Skip trailing spaces and comments.
                                SkipSpace(str, ref idx);

                                if (idx < str.Length && str[idx] == ',')
                                {
                                    // A separator has been found. Skip it.
                                    idx++;
                                    has_separator = true;
                                }
                                else
                                    has_separator = false;
                            }
                            else
                                throw new MissingSeparatorOrClosingParenthesisException("]", str, idx);
                        }

                        throw new MissingClosingParenthesisException("]", str, array_origin);
                    }

                case '{':
                    {
                        // An object has been found.
                        int object_origin = idx;
                        Dictionary<string, object> obj = new Dictionary<string, object>();
                        bool is_empty = true, has_separator = false;

                        for (idx++; idx < str.Length;)
                        {
                            ct.ThrowIfCancellationRequested();

                            // Skip leading spaces and comments.
                            SkipSpace(str, ref idx);

                            if (idx < str.Length && str[idx] == '}')
                            {
                                // This is the end.
                                idx++;
                                return obj;
                            }
                            else if (is_empty || has_separator)
                            {
                                int identifier_origin = idx;
                                object key = ParseIdentifier(str, ref idx);
                                if (key != null)
                                {
                                    // An element key has been found.
                                    if (obj.ContainsKey((string)key))
                                        throw new DuplicateElementException((string)key, str, identifier_origin);

                                    // Skip trailing spaces and comments.
                                    SkipSpace(str, ref idx);

                                    if (idx < str.Length && str[idx] == ':')
                                    {
                                        // An key:value separator found.
                                        idx++;

                                        // Skip leading spaces and comments.
                                        SkipSpace(str, ref idx);

                                        // Analyse value recursively, and add it.
                                        obj.Add((string)key, ParseValue(str, ref idx, ct));
                                        is_empty = false;

                                        // Skip trailing spaces and comments.
                                        SkipSpace(str, ref idx);

                                        if (idx < str.Length && str[idx] == ',')
                                        {
                                            // A separator has been found. Skip it.
                                            idx++;
                                            has_separator = true;
                                        }
                                        else
                                            has_separator = false;
                                    }
                                    else
                                        throw new MissingSeparatorException(str, idx);
                                }
                                else
                                    throw new InvalidIdentifier(str, idx);
                            }
                            else
                                throw new MissingSeparatorOrClosingParenthesisException("}", str, idx);
                        }
                        throw new MissingClosingParenthesisException("}", str, object_origin);
                    }
            }

            throw new UnknownValueException(str, idx);
        }

        /// <summary>
        /// Skips white-space between JSON values.
        /// </summary>
        /// <param name="str">The JSON string to parse</param>
        /// <param name="idx">Starting index in <paramref name="str"/></param>
        /// <remarks>C/C++ style comments are also treated as white-space and skipped.</remarks>
        private static void SkipSpace(string str, ref int idx)
        {
            for (int len = str.Length; idx < len;)
            {
                if (char.IsWhiteSpace(str[idx]))
                {
                    // Skip whitespace.
                    idx++;
                }
                else if (idx + 1 < len && str[idx] == '/')
                {
                    // JSON EXT: C/C++ style comments
                    if (str[idx + 1] == '/')
                    {
                        // C++ line comment. Skip anything up to the line-break.
                        for (idx += 2; ;)
                        {
                            if (idx >= len)
                                break;
                            else if (str[idx] == '\n')
                            {
                                idx++;
                                break;
                            }
                            else
                                idx++;
                        }
                    }
                    else if (str[idx + 1] == '*')
                    {
                        // C comment. Skip anything until "*/".
                        for (idx += 2; ;)
                        {
                            if (idx >= len)
                                break;
                            else if (idx + 1 < len && str[idx] == '*' && str[idx + 1] == '/')
                            {
                                idx += 2;
                                break;
                            }
                            else
                                idx++;
                        }
                    }
                    else
                        break;
                }
                else
                    break;
            }
        }

        #endregion

        #region Dictionary Helpers

        /// <summary>
        /// Safely gets a value with name from the dictionary
        /// </summary>
        /// <typeparam name="T">Requested value type. Can be: <see cref="bool"/>, <see cref="int"/>, <see cref="double"/>, <see cref="string"/>, <c>Dictionary&lt;string, object&gt;</c> or <c>List&gt;object&lt;</c></typeparam>
        /// <param name="dict">Dictionary of name/value pairs</param>
        /// <param name="name">Name of the value</param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> when <paramref name="name"/> found; <c>false</c> otherwise; throws when <paramref name="name"/> not of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidParameterTypeException">Wrong type of value</exception>
        public static bool GetValue<T>(Dictionary<string, object> dict, string name, out T value)
        {
            if (!dict.TryGetValue(name, out object obj))
            {
                value = default(T);
                return false;
            }

            if (obj.GetType() != typeof(T))
                throw new InvalidParameterTypeException(name, typeof(T), obj.GetType());

            value = (T)obj;
            return true;
        }

        /// <summary>
        /// Safely gets a value with name from the dictionary
        /// </summary>
        /// <typeparam name="T">Requested value type. Can be: <see cref="bool"/>, <see cref="int"/>, <see cref="double"/>, <see cref="string"/>, <c>Dictionary&lt;string, object&gt;</c> or <c>List&gt;object&lt;</c></typeparam>
        /// <param name="dict">Dictionary of name/value pairs</param>
        /// <param name="name">Name of the value</param>
        /// <returns>The value; or throws when <paramref name="name"/> not found in <paramref name="dict"/> or not of type <typeparamref name="T"/>.</returns>
        /// <exception cref="MissingParameterException">Value not found</exception>
        /// <exception cref="InvalidParameterTypeException">Wrong type of value</exception>
        public static T GetValue<T>(Dictionary<string, object> dict, string name)
        {
            if (!dict.TryGetValue(name, out object obj))
                throw new MissingParameterException(name);

            if (obj.GetType() != typeof(T))
                throw new InvalidParameterTypeException(name, typeof(T), obj.GetType());

            return (T)obj;
        }

        /// <summary>
        /// Safely gets a localized value with name from the dictionary
        /// </summary>
        /// <typeparam name="T">Requested value type. Can be: <see cref="bool"/>, <see cref="int"/>, <see cref="double"/>, <see cref="string"/>, or <c>List&gt;object&lt;</c></typeparam>
        /// <param name="dict">Dictionary of name/value pairs</param>
        /// <param name="name">Name of the value</param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> when <paramref name="name"/> found; <c>false</c> otherwise; throws when <paramref name="name"/> not of type <typeparamref name="T"/>.</returns>
        /// <remarks>The <typeparamref name="T"/> can not be <c>Dictionary&lt;string, object&gt;</c>, as <c>Dictionary&lt;string, object&gt;</c> is used to store localized versions.</remarks>
        /// <exception cref="InvalidParameterTypeException">Wrong type of value</exception>
        public static bool GetLocalizedValue<T>(Dictionary<string, object> dict, string name, out T value)
        {
            if (!dict.TryGetValue(name, out object obj))
            {
                value = default(T);
                return false;
            }

            if (obj is Dictionary<string, object> obj_dict)
            {
                // Load value according to thread UI culture.
                if (GetValue<T>(obj_dict, Thread.CurrentThread.CurrentUICulture.Name, out value))
                    return true;

                // Load value according to thread culture.
                if (GetValue<T>(obj_dict, Thread.CurrentThread.CurrentCulture.Name, out value))
                    return true;

                // Fallback to "en-US".
                if (GetValue<T>(obj_dict, "en-US", out value))
                    return true;

                // Fallback to the first value.
                foreach (var obj_first in obj_dict.Values)
                {
                    if (obj_first.GetType() != typeof(T))
                        throw new InvalidParameterTypeException(name, typeof(T), obj_first.GetType());

                    value = (T)obj_first;
                    return true;
                }

                return false;
            }
            else if (obj.GetType() != typeof(T))
                throw new InvalidParameterTypeException(name, typeof(T), obj.GetType());

            value = (T)obj;
            return true;
        }

        /// <summary>
        /// Safely gets a localized value with name from the dictionary
        /// </summary>
        /// <typeparam name="T">Requested value type. Can be: <see cref="bool"/>, <see cref="int"/>, <see cref="double"/>, <see cref="string"/>, or <c>List&gt;object&lt;</c></typeparam>
        /// <param name="dict">Dictionary of name/value pairs</param>
        /// <param name="name">Name of the value</param>
        /// <returns>The value; or throws when <paramref name="name"/> not found in <paramref name="dict"/> or not of type <typeparamref name="T"/>.</returns>
        /// <remarks>The <typeparamref name="T"/> can not be <c>Dictionary&lt;string, object&gt;</c>, as <c>Dictionary&lt;string, object&gt;</c> is used to store localized versions.</remarks>
        /// <exception cref="MissingParameterException">Value not found</exception>
        /// <exception cref="InvalidParameterTypeException">Wrong type of value</exception>
        public static T GetLocalizedValue<T>(Dictionary<string, object> dict, string name)
        {
            if (!dict.TryGetValue(name, out object obj))
                throw new MissingParameterException(name);

            if (obj is Dictionary<string, object> obj_dict)
            {
                T value;

                // Load value according to thread UI culture.
                if (GetValue<T>(obj_dict, Thread.CurrentThread.CurrentUICulture.Name, out value))
                    return value;

                // Load value according to thread culture.
                if (GetValue<T>(obj_dict, Thread.CurrentThread.CurrentCulture.Name, out value))
                    return value;

                // Fallback to "en-US".
                if (GetValue<T>(obj_dict, "en-US", out value))
                    return value;

                // Fallback to the first value.
                foreach (var obj_first in obj_dict.Values)
                {
                    if (obj_first.GetType() != typeof(T))
                        throw new InvalidParameterTypeException(name, typeof(T), obj_first.GetType());

                    return (T)obj_first;
                }

                throw new MissingParameterException(name);
            }
            else if (obj.GetType() != typeof(T))
                throw new InvalidParameterTypeException(name, typeof(T), obj.GetType());

            return (T)obj;
        }

        #endregion

        #endregion
    }
}
