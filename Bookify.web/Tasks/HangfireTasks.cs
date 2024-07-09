using Bookify.web.Core.Models;
using Bookify.web.Core.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Bookify.web.Tasks
{
    public class HangfireTasks
    {
        private readonly ApplicationDbContext _context;

        private readonly IWhatsAppClient _whatsAppClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailBodyBuilder _emailBodyBuilder;
        private readonly IEmailSender _emailSender;
        public HangfireTasks(ApplicationDbContext context, IWhatsAppClient whatsAppClient, IWebHostEnvironment webHostEnvironment, IEmailBodyBuilder emailBodyBuilder, IEmailSender emailSender)
        {
            _context = context;
            _whatsAppClient = whatsAppClient;
            _webHostEnvironment = webHostEnvironment;
            _emailBodyBuilder = emailBodyBuilder;
            _emailSender = emailSender;
        }

        public async Task PrepareExpirationAlert()
        {

            var subscribers = _context.Subscripers.Include(_s => _s.Subscriptions)
                    .Where(s => s.Subscriptions.OrderByDescending(s => s.EndDate).First().EndDate == DateTime.Today.AddDays(5));

            foreach (var subscriber in subscribers)
            {
                var endDate = subscriber.Subscriptions.Last().EndDate.ToString("d MMM,yyyy");
                //send welcome email
                var placeholders = new Dictionary<string, string>()
                {
                    { "imageUrl", "https://console.cloudinary.com/console/c-5675b9400f1c10da22b1e54da59289/media_library/homepage/asset/77ea6720c413fab405bff268e62f3468/manage?context=manage" },
                    { "header", $"Hi {subscriber.FirstName}" },
                    { "body", $"Your subscribtion will be expired by {endDate}" }
                };
                var body = _emailBodyBuilder.GetEmailBuilder(EmailTemplate.Notification, placeholders);

                await _emailSender.SendEmailAsync(subscriber.Email, "Bookify Subscribtion Expiration", body);


                //send welcome message using whatsApp
                if (subscriber.HasWhatsApp)
                {

                    var components = new List<WhatsAppComponent>
            {
                new WhatsAppComponent
                {
                    Type="body",
                    Parameters=new List<object>
                    {
                        new WhatsAppTextParameter{Text=subscriber.FirstName},
                        new WhatsAppTextParameter {Text=endDate}
                    }
                }
            };
                    var mobilNumber = _webHostEnvironment.IsDevelopment() ? "01033500507" : subscriber.MobileNumber;

                    var result = await _whatsAppClient.SendMessage($"2{mobilNumber}",
                        WhatsAppLanguageCode.English_US, Core.Consts.WhatsAppTemplate.SubscribtionExpirationAlert, components);
                }
            }

        }
    }
}
