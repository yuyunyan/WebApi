using System;

namespace Sourceportal.Domain.Models.Services.ErrorManagement
{
    public class GlobalApiException : Exception
    {
        

        public GlobalApiException()
        {
            
        }

        public GlobalApiException(string message) : base(message)
        {
            
        }

        public GlobalApiException(string message, Exception inner) : base(message, inner)
        {
            
        }

        public GlobalApiException(string message, string source) : base(message)
        {
            base.Source = source;
        }
    }
}
