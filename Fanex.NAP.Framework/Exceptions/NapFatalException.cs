// --------------------------------------------------------------------------
// <copyright file="OrchardFatalException.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.11">Created</change>
// </history>

namespace Fanex.NAP.Framework.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Localization;

    [Serializable]
    public class NapFatalException : Exception
    {
        private readonly LocalizedString _localizedMessage;

        public NapFatalException(LocalizedString message)
            : base(message.Text)
        {
            _localizedMessage = message;
        }

        public NapFatalException(LocalizedString message, Exception innerException)
            : base(message.Text, innerException)
        {
            _localizedMessage = message;
        }

        protected NapFatalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public LocalizedString LocalizedMessage { get { return _localizedMessage; } }
    }
}
