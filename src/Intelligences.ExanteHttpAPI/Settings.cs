using Intelligences.ExanteHttpAPI.Enum;

namespace Intelligences.ExanteHttpAPI
{
    public class Settings
    {
        private readonly HttpApiMode mode;
        private readonly string host = "https://api-{0}.exante.eu";
        private readonly string clientId;
        private readonly string appId;
        private readonly string sharedKey;
        private readonly int ttl;

        public Settings(HttpApiMode mode, string clientId, string appId, string sharedKey, int ttl = 3600)
        {
            this.mode = mode;

            switch (mode)
            {
                case HttpApiMode.Demo:
                    this.host = string.Format(this.host, HttpApiMode.Demo.ToString().ToLower());
                    break;
                case HttpApiMode.Live:
                    this.host = string.Format(this.host, HttpApiMode.Live.ToString().ToLower());
                    break;
            }

            this.clientId = clientId;
            this.appId = appId;
            this.sharedKey = sharedKey;
            this.ttl = ttl;
        }

        public HttpApiMode GetMode()
        {
            return this.mode;
        }

        public string GetHost()
        {
            return this.host;
        }

        public string GetClientId()
        {
            return this.clientId;
        }

        public string GetAppId()
        {
            return this.appId;
        }

        public string GetSharedKey()
        {
            return this.sharedKey;
        }

        public int GetTTL()
        {
            return this.ttl;
        }
    }
}
