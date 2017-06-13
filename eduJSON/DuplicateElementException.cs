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
    /// ErrorDuplicateElement
    /// </summary>
    [Serializable]
    public class DuplicateElementException : JSONException
    {
        public DuplicateElementException(string name, string code, int start) :
            base(String.Format(Resources.ErrorDuplicateElement, name), code, start)
        {
            ElementName = name;
        }

        public DuplicateElementException(string message, string name, string code, int start) :
            base(message, code, start)
        {
            ElementName = name;
        }

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

        /// <summary>
        /// Missing closing name
        /// </summary>
        public string ElementName { get; }
    }
}
