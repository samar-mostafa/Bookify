using Microsoft.AspNetCore.Hosting;
using System.Text.Encodings.Web;

namespace Bookify.web.Core.Services
{
    public class EmailBodyBuilder : IEmailBodyBuilder
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmailBodyBuilder(IWebHostEnvironment webHostEnvironment)
        {
                _webHostEnvironment = webHostEnvironment;
        }
        public string GetEmailBuilder(string imageUrl, string header, string linkTitle, string body, string url)
        {
            var filePath = $"{_webHostEnvironment.WebRootPath}/templates/email.html";
            StreamReader str = new(filePath);
            var template = str.ReadToEnd();
            str.Close();

           return template.Replace("[imageUrl]", imageUrl)
                .Replace("[header]", header)
                .Replace("[linkTitle]", linkTitle)
                .Replace("[body]", body)
                .Replace("[url]", url);           
        }
    }
}
