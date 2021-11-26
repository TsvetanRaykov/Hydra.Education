namespace Hydra.Module.Video.Backend
{
    public class ModuleVideoConfiguration
    {
        public string ConnectionString { get; set; }

        public ApiSettings ApiSettings { get; set; }

        public JwtConfig JwtConfig { get; set; }
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