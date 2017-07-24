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
    /// Missing "," separator or "{0}" parenthesis.
    /// </summary>
    [Serializable]
    public class MissingSeparatorOrClosingParenthesisException : MissingClosingParenthesisException, ISerializable
    {
        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="parenthesis">Parenthesis</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public MissingSeparatorOrClosingParenthesisException(string parenthesis, string code, int start) :
            this(String.Format(Resources.Strings.ErrorMissingSeparatorOrClosingParenthesis, parenthesis), parenthesis, code, start)
        {
        }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="parenthesis">Parenthesis</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public MissingSeparatorOrClosingParenthesisException(string message, string parenthesis, string code, int start) :
            base(message, parenthesis, code, start)
        {
        }

        #endregion
    }
}
