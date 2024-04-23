namespace Bookify.web.Core.Services
{
    public interface IEmailBodyBuilder
    {
        string GetEmailBuilder(string template, Dictionary<string, string> placeholders);
    }
}
