/*
    eduJSON - A Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;

namespace eduJSON
{
    /// <summary>
    /// Unexpected trailing data found.
    /// </summary>
    [Serializable]
    public class TrailingDataException : JSONException
    {
        public TrailingDataException(string code, int start) :
            base(Resources.ErrorTrailingData, code, start)
        {
        }
    }
}
