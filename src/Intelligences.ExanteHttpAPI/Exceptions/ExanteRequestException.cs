using System;

namespace Intelligences.ExanteHttpAPI.Exceptions
{
    public class ExanteRequestException : Exception
    {
        public static string InvalidCredentials = "Entered credentials not valid";
        public static string InputDataInvalid = "Requested user data not valid";
        public static string ResourceNotFound = "Resource not found";

        public ExanteRequestException(string message) : base(message)
        {
        }

        public ExanteRequestException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public ExanteRequestException()
        {
        }
    }
}
