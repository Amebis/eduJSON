/*
    eduJSON - A Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace eduJSON
{
    /// <summary>
    /// Missing "{0}" parenthesis.
    /// </summary>
    [Serializable]
    public class MissingClosingParenthesisException : JSONException
    {
        public MissingClosingParenthesisException(string parenthesis, string code, int start) :
            base(String.Format(Resources.ErrorMissingClosingParenthesis, parenthesis), code, start)
        {
            Parenthesis = parenthesis;
        }

        public MissingClosingParenthesisException(string message, string parenthesis, string code, int start) :
            base(message, code, start)
        {
            Parenthesis = parenthesis;
        }

        protected MissingClosingParenthesisException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Parenthesis = (string)info.GetValue("Parenthesis", typeof(string));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Parenthesis", Parenthesis);
        }

        /// <summary>
        /// Missing closing parenthesis
        /// </summary>
        public string Parenthesis { get; }
    }
}
