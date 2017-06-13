/*
    eduJSON - A Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;

namespace eduJSON
{
    /// <summary>
    /// Missing "," separator or "{0}" parenthesis.
    /// </summary>
    [Serializable]
    public class MissingSeparatorOrClosingParenthesisException : MissingClosingParenthesisException
    {
        public MissingSeparatorOrClosingParenthesisException(string parenthesis, string code, int start) :
            base(String.Format(Resources.ErrorMissingSeparatorOrClosingParenthesis, parenthesis), parenthesis, code, start)
        {
        }
    }
}
