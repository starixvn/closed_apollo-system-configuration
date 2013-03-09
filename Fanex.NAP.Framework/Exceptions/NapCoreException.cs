


namespace Fanex.NAP.Framework.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Localization;

    [Serializable]
    public class NapCoreException : Exception
    {
        private readonly LocalizedString _localizedMessage;

        public NapCoreException(LocalizedString message)
            : base(message.Text)
        {
            _localizedMessage = message;
        }

        public NapCoreException(LocalizedString message, Exception innerException)
            : base(message.Text, innerException)
        {
            _localizedMessage = message;
        }

        protected NapCoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public LocalizedString LocalizedMessage { get { return _localizedMessage; } }
    }
}