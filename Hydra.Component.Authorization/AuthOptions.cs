namespace Hydra.Component.Authorization
{
    public class AuthOptions
    {
        /// <summary>
        /// The Name of the HttpClient that will be used for authorized requests in your module
        /// </summary>
        public string HttpClientName { get; set; }

        /// <summary>
        /// The Base Url of the HttpClient web requests
        /// </summary>
        public string HttpClientBaseAddress { get; set; }
    }
}