﻿/*
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
        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/>.</param>
        public MissingSeparatorException(string code, int start) :
            this(Resources.ErrorMissingSeparator, code, start)
        {
        }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/>.</param>
        public MissingSeparatorException(string message, string code, int start) :
            base(message, code, start)
        {
        }

        #endregion
    }
}