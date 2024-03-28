namespace Bookify.web.Core.Services
{
    public interface IEmailBodyBuilder
    {
        string GetEmailBuilder(string imageUrl, string header ,string linkTitle ,string body ,string url);
    }
}
