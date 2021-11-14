using System.Text.Json.Serialization;

namespace Hydra.Server.Auth.Models
{
    public class SendInBlueEmailMessage
    {
        [JsonPropertyName("sender")]
        public EmailItem Sender { get; set; }

        [JsonPropertyName("to")]
        public EmailItem[] To { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("htmlContent")]
        public string HtmlContent { get; set; }


    }
    public class EmailItem
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}