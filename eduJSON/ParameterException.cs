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
    /// Unacceptable or missing parameter.
    /// </summary>
    [Serializable]
    public class ParameterException : ApplicationException
    {
        #region Members

        /// <summary>
        /// The error message
        /// </summary>
        public override string Message => ParameterName != null ? String.Format(Resources.Strings.ErrorParameter, base.Message, ParameterName) : base.Message;

        /// <summary>
        /// Parameter name
        /// </summary>
        public string ParameterName { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="parameter">Parameter name</param>
        public ParameterException(string message, string parameter) :
            base(message)
        {
            ParameterName = parameter;
        }

        #endregion

        #region ISerializable Support

        protected ParameterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ParameterName = (string)info.GetValue("ParameterName", typeof(string));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ParameterName", ParameterName);
        }

        #endregion
    }
}
