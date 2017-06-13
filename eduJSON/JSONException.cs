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
        public JSONException()
        { }

        public JSONException(string message) :
            base(message)
        { }

        public JSONException(string message, Exception innerException) :
            base(message, innerException)
        { }

        public JSONException(string message, string code, int idx) :
            base(message)
        {
            Code = code.Length < idx + 20 ? code.Substring(idx) : code.Substring(idx, 19) + "…";
        }

        public JSONException(string message, string code, int idx, Exception innerException) :
            base(message, innerException)
        {
            Code = code.Length < idx + 20 ? code.Substring(idx) : code.Substring(idx, 19) + "…";
        }

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

        /// <summary>
        /// Gets the error message and the JSON code, or only the error message if no code is set.
        /// </summary>
        public override string Message => Code != null ? String.Format("{0} - {1}", base.Message, Code) : base.Message;

        /// <summary>
        /// JSON code that caused the problem
        /// </summary>
        public string Code { get; }
    }
}
