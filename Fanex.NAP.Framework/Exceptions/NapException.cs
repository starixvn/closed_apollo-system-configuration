

namespace Fanex.NAP.Framework.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Localization;

    [Serializable]
    public class NapException : ApplicationException
    {
        private readonly LocalizedString _localizedMessage;

        public NapException(LocalizedString message)
            : base(message.Text)
        {
            _localizedMessage = message;
        }

        public NapException(LocalizedString message, Exception innerException)
            : base(message.Text, innerException)
        {
            _localizedMessage = message;
        }

        protected NapException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public LocalizedString LocalizedMessage { get { return _localizedMessage; } }
    }
}