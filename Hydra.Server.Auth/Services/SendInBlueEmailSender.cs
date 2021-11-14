namespace Hydra.Server.Auth.Services
{
    using Authorization;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;
    using Models;
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class SendInBlueEmailSender : IEmailSender
    {
        private readonly string _apiUrl;
        private readonly HttpClient _httpClient;

        public SendInBlueEmailSender(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            var apiKey = configuration["SendInBlueApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException("ApiKey");
            };

            _apiUrl = configuration["SendInBlueApiUrl:Send"];
            if (string.IsNullOrWhiteSpace(_apiUrl))
            {
                throw new ArgumentNullException("ApiUrl");
            };

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add("api-key", apiKey);
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var message = new SendInBlueEmailMessage
            {
                HtmlContent = htmlMessage,
                Sender = new EmailItem { Name = GlobalConstants.SystemEmail.SendFromName, Email = GlobalConstants.SystemEmail.SendFromEmail },
                To = new[] { new EmailItem { Name = email, Email = email } },
                Subject = subject
            };


            return _httpClient.PostAsJsonAsync(_apiUrl, message);

        }
    }
}