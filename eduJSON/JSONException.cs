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
    /// Generic JSON exception
    /// </summary>
    [Serializable]
    public class JSONException : ApplicationException
    {
        #region Properties

        /// <summary>
        /// Gets the error message and the JSON code, or only the error message if no code is set.
        /// </summary>
        public override string Message => Code != null ? String.Format(Resources.Strings.ErrorJSONCode, base.Message, Code) : base.Message;

        /// <summary>
        /// JSON code that caused the problem
        /// </summary>
        public string Code { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        public JSONException() :
            base()
        { }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        public JSONException(string message) :
            base(message)
        { }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public JSONException(string message, Exception innerException) :
            base(message, innerException)
        { }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/>.</param>
        public JSONException(string message, string code, int start) :
            base(message)
        {
            Code = code.Length < start + 20 ? code.Substring(start) : code.Substring(start, 19) + "…";
        }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/>.</param>
        /// <param name="innerException">Inner exception</param>
        public JSONException(string message, string code, int start, Exception innerException) :
            base(message, innerException)
        {
            Code = code.Length < start + 20 ? code.Substring(start) : code.Substring(start, 19) + "…";
        }

        #endregion

        #region ISerializable Support

        protected JSONException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Code = (string)info.GetValue("Code", typeof(string));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Code", Code);
        }

        #endregion
    }
}
