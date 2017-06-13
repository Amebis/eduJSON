/*
    eduJSON - A Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;

namespace eduJSON
{
    /// <summary>
    /// Unknown value found.
    /// </summary>
    [Serializable]
    public class UnknownValueException : JSONException
    {
        public UnknownValueException(string code, int start) :
            base(Resources.ErrorUnknownValue, code, start)
        {
        }
    }
}
