/*
    eduJSON - A Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;

namespace eduJSON
{
    /// <summary>
    /// Invalid element name found.
    /// </summary>
    [Serializable]
    public class InvalidIdentifier : JSONException
    {
        public InvalidIdentifier(string code, int start) :
            base(Resources.ErrorInvalidIdentifier, code, start)
        {
        }
    }
}
