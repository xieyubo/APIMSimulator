using System;

namespace APIMSimulator
{
    internal class ReturnResponseException : Exception
    {
        public HttpResponse Response { get; }

        public ReturnResponseException(HttpResponse response)
        {
            Response = response;
        }
    }
}
