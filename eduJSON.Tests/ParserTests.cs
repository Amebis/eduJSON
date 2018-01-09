/*
    eduJSON - Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace eduJSON.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void ParseBooleanTest()
        {
            // Test basic data types.
            Assert.AreEqual(true, Parser.Parse("// Test 1\n  True /* Trailing comment */"), "Boolean \"true\" not recognized");
            Assert.AreEqual(false, Parser.Parse("   falSe\r\n// Trailing comment"), "Boolean \"false\" not recognized");
        }

        [TestMethod()]
        public void ParseNullTest()
        {
            Assert.AreEqual(null, Parser.Parse("NULL"), "\"null\" not recognized");
        }

        [TestMethod()]
        public void ParseIntegerTest()
        {
            Assert.AreEqual(1234, Parser.Parse(" 1234 "), "Integer not recognized");
            Assert.AreEqual(1234, Parser.Parse(" +1234 "), "Integer with an explicit \"+\" sign not recognized (JSON EX)");
            Assert.AreEqual(-1234, Parser.Parse(" -1234 "), "Negative integer not recognized");
        }

        [TestMethod()]
        public void ParseFloatTest()
        {
            Assert.AreEqual(1.0870e-3, (double)Parser.Parse(" +1.0870e-3 "), 1e-10, "Float not recognized");
            Assert.AreEqual(-2e+31, (double)Parser.Parse(" -2e+31 "), 1e-10, "Negative float not recognized");
        }

        [TestMethod()]
        public void ParseStringTest()
        {
            Assert.AreEqual("This is a test.", Parser.Parse(" \"This is a test.\" "), "Quoted string not recognized");
            Assert.AreEqual("  \"/\\\b\f\n\r\t\u263a\\x  ", Parser.Parse(" \"  \\\"\\/\\\\\\b\\f\\n\\r\\t\\u263A\\x  \" "), "JSON string escape sequences not recognized");
        }

        [TestMethod()]
        public void ParseStructTest()
        {
            // CollectionAssert.AreEqual() does not work for sub-collections; Check manually.
            object obj = Parser.Parse("{ \"key1\" : true , \"key2\" : [ 1, 2, 3, 4, 5], \"key3\" : { \"k1\": \"test1\", k2:\"test2\"}}");
            Dictionary<string, object> obj_dict = (Dictionary<string, object>)obj;
            Assert.AreEqual(3, obj_dict.Count, "Incorrect number of child elements");
            Assert.AreEqual(true, obj_dict["key1"], "Child element mismatch");
            CollectionAssert.AreEqual(new List<object>
                {
                    1,
                    2,
                    3,
                    4,
                    5
                }, (List<object>)obj_dict["key2"], "Child element mismatch");
            CollectionAssert.AreEqual(new Dictionary<string, object>
                {
                    { "k1", "test1" },
                    { "k2", "test2" }
                }, (Dictionary<string, object>)obj_dict["key3"], "Child element mismatch");

            try
            {
                Parser.Parse("[1, 2");
                Assert.Fail("Missing \"[\" parenthesis tolerated");
            }
            catch (MissingClosingParenthesisException) { }
            try
            {
                Parser.Parse("[1 2]");
                Assert.Fail("Missing separator tolerated");
            }
            catch (MissingSeparatorOrClosingParenthesisException) { }
            try
            {
                Parser.Parse("{ \"k1\": 1, \"k2\": 2");
                Assert.Fail("Missing \"}\" parenthesis tolerated");
            }
            catch (MissingClosingParenthesisException) { }
            try
            {
                Parser.Parse("{ \"k1\": 1 \"k2\": 2 }");
                Assert.Fail("Missing separator tolerated");
            }
            catch (MissingSeparatorOrClosingParenthesisException) { }
            try
            {
                Parser.Parse("{ \"key\"  \"value\" }");
                Assert.Fail("Missing separator tolerated");
            }
            catch (MissingSeparatorException) { }
            try
            {
                Parser.Parse("{ \"k1\": 1, $$$: 2 }");
                Assert.Fail("Invalid identifier tolerated");
            }
            catch (InvalidIdentifier) { }
            try
            {
                Parser.Parse("{ \"k1\": 1, \"k1\": 2 }");
                Assert.Fail("Duplicate element tolerated");
            }
            catch (DuplicateElementException) { }
        }

        [TestMethod()]
        public void ParseIssuesTest()
        {
            try
            {
                Parser.Parse("   false\r\nTrailing data");
                Assert.Fail("Trailing JSON data tolerated");
            } catch (TrailingDataException) {}
            try
            {
                Parser.Parse("abc");
                Assert.Fail("Unknown JSON value tolerated");
            }
            catch (UnknownValueException) { }
        }

        [TestMethod()]
        public void ParseGetValueTest()
        {
            var obj = Parser.Parse("{ \"k_string\": \"abc\", \"k_bool\": true, \"k_int\": 123, \"k_array\": [1, 2, 3], \"k_dict\": {} }") as Dictionary<string, object>;

            // Function result variant
            Assert.AreEqual(Parser.GetValue<string>(obj, "k_string"), "abc");
            Assert.AreEqual(Parser.GetValue<bool>(obj, "k_bool"), true);
            Assert.AreEqual(Parser.GetValue<int>(obj, "k_int"), 123);
            CollectionAssert.AreEqual(Parser.GetValue<List<object>>(obj, "k_array"), new List<object>() { 1, 2, 3 });
            CollectionAssert.AreEqual(Parser.GetValue<Dictionary<string, object>>(obj, "k_dict"), new Dictionary<string, object>());

            try
            {
                Parser.GetValue<string>(obj, "foobar");
                Assert.Fail("Non-existing parameter found");
            }
            catch (MissingParameterException) { }

            try
            {
                Parser.GetValue<int>(obj, "k_string");
                Assert.Fail("Parameter type mismatch tolerated");
            }
            catch (InvalidParameterTypeException) { }

            // Variable reference variant
            Assert.IsTrue(Parser.GetValue(obj, "k_string", out string val_string) && val_string == "abc");
            Assert.IsTrue(Parser.GetValue(obj, "k_bool", out bool val_bool) && val_bool == true);
            Assert.IsTrue(Parser.GetValue(obj, "k_int", out int val_int) && val_int == 123);
            Assert.IsTrue(Parser.GetValue(obj, "k_array", out List<object> val_array)/* && val_array.Equals(new List<object>() { 1, 2, 3 })*/);
            Assert.IsTrue(Parser.GetValue(obj, "k_dict", out Dictionary<string, object> val_dict)/* && val_dict.Equals(new Dictionary<string, object>())*/);

            Assert.IsFalse(Parser.GetValue(obj, "foobar", out val_string));

            try
            {
                Parser.GetValue(obj, "k_string", out val_int);
                Assert.Fail("Parameter type mismatch tolerated");
            }
            catch (InvalidParameterTypeException) { }
        }

        [TestMethod()]
        public void ParseGetLocalizedValueTest()
        {
            CultureInfo culture;
            string val_string;
            var obj = Parser.Parse("{ \"key1\": \"<language independent>\", \"key2\": { \"de-DE\": \"Sprache\", \"en-US\": \"Language\" }, \"key3\": { \"de-DE\": \"Nur Deutsch\" } }") as Dictionary<string, object>;

            // Set language preference to German (Germany).
            culture = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Function result variant
            Assert.AreEqual(Parser.GetLocalizedValue<string>(obj, "key1"), "<language independent>");
            Assert.AreEqual(Parser.GetLocalizedValue<string>(obj, "key2"), "Sprache");
            Assert.AreEqual(Parser.GetLocalizedValue<string>(obj, "key3"), "Nur Deutsch");

            // Variable reference variant
            Assert.IsTrue(Parser.GetLocalizedValue(obj, "key1", out val_string) && val_string == "<language independent>");
            Assert.IsTrue(Parser.GetLocalizedValue(obj, "key2", out val_string) && val_string == "Sprache");
            Assert.IsTrue(Parser.GetLocalizedValue(obj, "key3", out val_string) && val_string == "Nur Deutsch");

            // Set language preference to Slovenian (Slovenia).
            culture = new CultureInfo("sl-SI");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Function result variant
            Assert.AreEqual(Parser.GetLocalizedValue<string>(obj, "key1"), "<language independent>");
            Assert.AreEqual(Parser.GetLocalizedValue<string>(obj, "key2"), "Language");
            Assert.AreEqual(Parser.GetLocalizedValue<string>(obj, "key3"), "Nur Deutsch");

            // Variable reference variant
            Assert.IsTrue(Parser.GetLocalizedValue(obj, "key1", out val_string) && val_string == "<language independent>");
            Assert.IsTrue(Parser.GetLocalizedValue(obj, "key2", out val_string) && val_string == "Language");
            Assert.IsTrue(Parser.GetLocalizedValue(obj, "key3", out val_string) && val_string == "Nur Deutsch");

            // Set language preference to English (U.S.).
            culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Function result variant
            Assert.AreEqual(Parser.GetLocalizedValue<string>(obj, "key1"), "<language independent>");
            Assert.AreEqual(Parser.GetLocalizedValue<string>(obj, "key2"), "Language");
            Assert.AreEqual(Parser.GetLocalizedValue<string>(obj, "key3"), "Nur Deutsch");

            // Variable reference variant
            Assert.IsTrue(Parser.GetLocalizedValue(obj, "key1", out val_string) && val_string == "<language independent>");
            Assert.IsTrue(Parser.GetLocalizedValue(obj, "key2", out val_string) && val_string == "Language");
            Assert.IsTrue(Parser.GetLocalizedValue(obj, "key3", out val_string) && val_string == "Nur Deutsch");

            // Test failures
            try
            {
                Parser.GetLocalizedValue<string>(obj, "foobar");
                Assert.Fail("Non-existing parameter found");
            }
            catch (MissingParameterException) { }

            try
            {
                Parser.GetLocalizedValue<int>(obj, "key2");
                Assert.Fail("Parameter type mismatch tolerated");
            }
            catch (InvalidParameterTypeException) { }

            Assert.IsFalse(Parser.GetValue(obj, "foobar", out val_string));

            try
            {
                Parser.GetValue(obj, "key2", out int val_int);
                Assert.Fail("Parameter type mismatch tolerated");
            }
            catch (InvalidParameterTypeException) { }
        }
    }
}
