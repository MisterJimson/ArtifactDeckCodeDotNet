using System;

namespace ArtifactDeckCodeDotNet
{
    public class CardSetApiClientException : Exception
    {
        public CardSetApiClientException()
        {
        }

        public CardSetApiClientException(string message)
            : base(message)
        {
        }

        public CardSetApiClientException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
