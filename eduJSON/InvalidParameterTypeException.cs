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
    /// Unexpected parameter type.
    /// </summary>
    [Serializable]
    public class InvalidParameterTypeException : ParameterException
    {
        #region Properties

        /// <summary>
        /// The error message
        /// </summary>
        public override string Message => String.Format(Resources.ErrorExpectedReceived, base.Message, ExpectedType, ProvidedType);

        /// <summary>
        /// The expected type of parameter
        /// </summary>
        public Type ExpectedType { get; }

        /// <summary>
        /// The provided type of parameter
        /// </summary>
        public Type ProvidedType { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="parameter">Parameter name</param>
        /// <param name="expected_type">Expected type</param>
        /// <param name="provided_type">Provided type</param>
        public InvalidParameterTypeException(string parameter, Type expected_type, Type provided_type) :
            this(Resources.ErrorInvalidParameterType, parameter, expected_type, provided_type)
        {
        }

        /// <summary>
        /// Constructs an exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="parameter">Parameter name</param>
        /// <param name="expected_type">Expected type</param>
        /// <param name="provided_type">Provided type</param>
        public InvalidParameterTypeException(string message, string parameter, Type expected_type, Type provided_type) :
            base(message, parameter)
        {
            ExpectedType = expected_type;
            ProvidedType = provided_type;
        }

        #endregion

        #region ISerializable Support

        protected InvalidParameterTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ExpectedType = (Type)info.GetValue("ExpectedType", typeof(Type));
            ProvidedType = (Type)info.GetValue("ProvidedType", typeof(Type));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ExpectedType", ExpectedType);
            info.AddValue("ProvidedType", ProvidedType);
        }

        #endregion
    }
}
