using System;

namespace AspNetCoreStarter.Exceptions
{
    public class BusinessException : Exception
    {
        public int StatusCode { get; private set; }

        public BusinessException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
