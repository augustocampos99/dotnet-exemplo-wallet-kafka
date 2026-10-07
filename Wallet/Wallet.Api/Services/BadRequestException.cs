namespace Wallet.Api.Services
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {            
        }

        public BadRequestException(string message) : base(message)
        {            
        }
    }
}
