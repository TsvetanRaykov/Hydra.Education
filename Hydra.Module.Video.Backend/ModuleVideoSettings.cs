namespace Hydra.Module.Video.Backend
{
    public class ModuleVideoSettings
    {
        public string ConnectionString { get; set; }
        public string StaticFilesLocation { get; set; }
        public ApiSettings ApiSettings { get; set; }
        public JwtConfig JwtConfig { get; set; }
        public ApiClient[] ApiClients { get; set; }
    }

    public class ApiClient
    {
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string[] Roles { get; set; }
    }

    public class ApiSettings
    {
        public string BaseUrl { get; set; }
    }

    public class JwtConfig
    {
        public string Key { get; set; }
    }

}