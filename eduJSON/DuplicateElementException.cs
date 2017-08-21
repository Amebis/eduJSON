/*
    eduJSON - Lightweight JSON Parser for eduVPN (and beyond)

    Copyright: 2017, The Commons Conservancy eduVPN Programme
    SPDX-License-Identifier: GPL-3.0+
*/

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace eduJSON
{
    /// <summary>
    /// ErrorDuplicateElement
    /// </summary>
    [Serializable]
    public class DuplicateElementException : JSONException, ISerializable
    {
        #region Properties

        /// <summary>
        /// Element name
        /// </summary>
        public string ElementName { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="name">Element name</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public DuplicateElementException(string name, string code, int start) :
            this(String.Format(Resources.Strings.ErrorDuplicateElement, name), name, code, start)
        {
        }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="name">Element name</param>
        /// <param name="code">JSON code</param>
        /// <param name="start">Starting offset in <paramref name="code"/></param>
        public DuplicateElementException(string message, string name, string code, int start) :
            base(message, code, start)
        {
            ElementName = name;
        }

        #endregion

        #region ISerializable Support

        protected DuplicateElementException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ElementName = (string)info.GetValue("ElementName", typeof(string));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ElementName", ElementName);
        }

        #endregion
    }
}
