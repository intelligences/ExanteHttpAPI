using Intelligences.ExanteHttpAPI.Enum;
using Intelligences.ExanteHttpAPI.Exceptions;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;

namespace Intelligences.ExanteHttpAPI.Service
{
    internal class HttpClientService
    {
        private readonly Settings settings;
        private readonly RestClient restClient;
        private string token;

        public HttpClientService(Settings settings)
        {
            this.settings = settings;

            this.restClient = new RestClient(this.settings.GetHost());
        }

        public string SendRequest(HttpMethod method, string uri, Dictionary<string, dynamic> parameters)
        {
            this.makeToken();

            this.restClient.Authenticator = new JwtAuthenticator(this.token);

            RestRequest request = new RestRequest(uri, DataFormat.Json);

            foreach(KeyValuePair<string, dynamic> pair in parameters)
            {
                string key = pair.Key;
                dynamic value = pair.Value;

                request.AddParameter(key, value);
            }

            IRestResponse response = null;

            switch (method)
            {
                case HttpMethod.GET:
                    response = this.restClient.Get(request);
                    break;
                case HttpMethod.POST:
                    response = this.restClient.Post(request);
                    break;
            }
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new ExanteRequestException(ExanteRequestException.InvalidCredentials);
                    break;
                case HttpStatusCode.BadRequest:
                    throw new ExanteRequestException(ExanteRequestException.InputDataInvalid);
                    break;
                case HttpStatusCode.NotFound:
                    throw new ExanteRequestException(ExanteRequestException.ResourceNotFound);
                    break;
            }

            return response.Content;
        }

        private void makeToken()
        {
            if (this.isTokenValid())
            {
                return;
            }

            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string clientId = this.settings.GetClientId();
            string appId = this.settings.GetAppId();
            int ttl = this.settings.GetTTL();
            string sharedKey = this.settings.GetSharedKey();
            List<string> scopes = new List<string>();

            scopes.Add("ohlc");

            var payload = new Dictionary<string, object>
            {
                { "iss", clientId },
                { "sub", appId },
                { "iat", timestamp },
                { "exp", timestamp + ttl },
                { "aud", scopes },
            };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            this.token = encoder.Encode(payload, sharedKey);
        }

        /// <summary>
        /// Check is token valid
        /// </summary>
        /// <returns>true -valid, false - invalid</returns>
        private bool isTokenValid()
        {
            if (String.IsNullOrEmpty(this.token))
            {
                return false;
            }

            string sharedKey = this.settings.GetSharedKey();

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                decoder.Decode(this.token, sharedKey, verify: true);

                return true;
            }
            catch (TokenExpiredException)
            {
                return false;
            }
            catch (SignatureVerificationException)
            {
                return false;
            }
        }
    }
}
