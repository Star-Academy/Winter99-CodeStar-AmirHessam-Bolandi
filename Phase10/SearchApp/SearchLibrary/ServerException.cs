using System;

namespace SearchLibrary
{
    public class ServerException : Exception
    {
        public ServerException(string message) : base(message)
        {
        }
    }
}