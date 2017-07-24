/*
    eduJSON - A Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;
using System.Runtime.Serialization;

namespace eduJSON
{
    /// <summary>
    /// Unexpected trailing data found.
    /// </summary>
    [Serializable]
    public class TrailingDataException : JSONException, ISerializable
    {
        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public TrailingDataException(string code, int start) :
            this(Resources.Strings.ErrorTrailingData, code, start)
        {
        }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public TrailingDataException(string message, string code, int start) :
            base(message, code, start)
        {
        }

        #endregion
    }
}
