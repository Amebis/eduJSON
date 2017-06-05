/*
    Copyright 2017 Amebis

    This file is part of eduJSON.

    eduJSON is free software: you can redistribute it and/or modify it
    under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    eduJSON is distributed in the hope that it will be useful, but
    WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with eduJSON. If not, see <http://www.gnu.org/licenses/>.
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace eduJSON.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            // Test basic data types.
            Assert.AreEqual(true, Parser.Parse("// Test 1\n  True /* Trailing comment */"), "Boolean \"true\" not recognized");
            Assert.AreEqual(false, Parser.Parse("   falSe\r\n// Trailing comment"), "Boolean \"false\" not recognized");
            Assert.AreEqual(null, Parser.Parse("NULL"), "\"null\" not recognized");
            Assert.AreEqual(1234, Parser.Parse(" 1234 "), "Integer not recognized");
            Assert.AreEqual(1234, Parser.Parse(" +1234 "), "Integer with an explicit \"+\" sign not recognized (JSON EX)");
            Assert.AreEqual(-1234, Parser.Parse(" -1234 "), "Negative integer not recognized");
            Assert.AreEqual(1.0870e-3, (double)Parser.Parse(" +1.0870e-3 "), 1e-10, "Float not recognized");
            Assert.AreEqual(-2e+31, (double)Parser.Parse(" -2e+31 "), 1e-10, "Negative float not recognized");
            Assert.AreEqual("This is a test.", Parser.Parse(" \"This is a test.\" "), "Quoted string not recognized");
            Assert.AreEqual("  \"/\\\b\f\n\r\t\u263a\\x  ", Parser.Parse(" \"  \\\"\\/\\\\\\b\\f\\n\\r\\t\\u263A\\x  \" "), "JSON string escape sequences not recognized");

            // Test objects and arrays.
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
       }
    }
}