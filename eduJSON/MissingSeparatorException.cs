/*
    eduJSON - A Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;

namespace eduJSON
{
    /// <summary>
    /// Missing ":" separator.
    /// </summary>
    [Serializable]
    public class MissingSeparatorException : JSONException
    {
        public MissingSeparatorException(string code, int start) :
            base(Resources.ErrorMissingSeparator, code, start)
        {
        }
    }
}
