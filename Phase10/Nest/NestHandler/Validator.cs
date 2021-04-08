using System;
using Nest;

namespace NestHandler
{
    public static class Validator
    {
        public static T Validate<T>(T response) where T : IResponse
        {
            if (!response.IsValid)
                CheckExceptions(response);
            return response;
        }

        public static void CheckExceptions(IResponse response)
        {
            if (response.OriginalException != null)
            {
                throw response.OriginalException.InnerException;
            }
            else if (response.ServerError != null)
            {
                throw new ServerException("Sorry something is wrong with the server! status:" +
                                          response.ServerError.Status + " " + response.ServerError.Error);
            }
        }
        
        public class ServerException : Exception
        {
            public ServerException(string message) : base(message)
            {
            }
        }
    }
}