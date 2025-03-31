using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagmentApp.Common.Exceptions
{
    public  class CustomException : Exception
    {
        public int ErrorCode { get; } = 400;

        public CustomException()
            : base("Custom exception occurred.") { }

        public CustomException(int errorCode)
            : base("Custom exception occurred.") 
        {
            ErrorCode = errorCode;
        }

        public CustomException(string message)
            : base(message) { }

        public CustomException(string message, int errorCode)
            : base(message) 
        {
            ErrorCode = errorCode;
        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException) { }

        public CustomException(string message, int errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
