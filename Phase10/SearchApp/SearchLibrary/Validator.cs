using Nest;

namespace SearchLibrary
{
    public static class ValidatorClass
    {
        public static T Validate<T>(this T response) where T : IResponse
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
                throw new ServerException($"Sorry something is wrong with the server!\n status: {response.ServerError.Status} \n Error message: {response.ServerError.Error} ");
            }
        }
    }
}