﻿/*
    eduJSON - Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;
using System.Runtime.Serialization;

namespace eduJSON
{
    /// <summary>
    /// Invalid element name found.
    /// </summary>
    [Serializable]
    public class InvalidIdentifier : JSONException, ISerializable
    {
        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public InvalidIdentifier(string code, int start) :
            this(Resources.Strings.ErrorInvalidIdentifier, code, start)
        {
        }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public InvalidIdentifier(string message, string code, int start) :
            base(message, code, start)
        {
        }

        #endregion
    }
}
