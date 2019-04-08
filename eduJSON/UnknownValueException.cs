﻿/*
    eduJSON - Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017-2019 The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;
using System.Runtime.Serialization;

namespace eduJSON
{
    /// <summary>
    /// Unknown value found.
    /// </summary>
    [Serializable]
    public class UnknownValueException : JSONException
    {
        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public UnknownValueException(string code, int start) :
            this(Resources.Strings.ErrorUnknownValue, code, start)
        {
        }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public UnknownValueException(string message, string code, int start) :
            base(message, code, start)
        {
        }

        #endregion

        #region ISerializable Support

        /// <summary>
        /// Deserialize object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> populated with data.</param>
        /// <param name="context">The source of this deserialization.</param>
        protected UnknownValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
