/*
    eduJSON - A Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;

namespace eduJSON
{
    /// <summary>
    /// A required parameter is missing.
    /// </summary>
    [Serializable]
    public class MissingParameterException : ParameterException
    {
        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="parameter">Parameter name</param>
        public MissingParameterException(string parameter) :
            this(Resources.Strings.ErrorMissingParameter, parameter)
        {
        }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="parameter">Parameter name</param>
        public MissingParameterException(string message, string parameter) :
            base(message, parameter)
        {
        }

        #endregion
    }
}
