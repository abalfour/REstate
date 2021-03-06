﻿using REstate;
using REstateClient.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace REstateClient
{
    public class REstateAuthClient
    {
        protected readonly Uri ApiKeyAuthAddress;
        private const string Json = "application/json";
        protected StringSerializer StringSerializer { get; }

        public REstateAuthClient(StringSerializer stringSerializer, string apiKeyAuthAddress)
        {
            StringSerializer = stringSerializer;

            if (string.IsNullOrWhiteSpace(apiKeyAuthAddress)) throw new ArgumentNullException(nameof(apiKeyAuthAddress));

            Uri baseUri;
            if (!Uri.TryCreate(apiKeyAuthAddress, UriKind.RelativeOrAbsolute, out baseUri)) throw new ArgumentException("Not a valid Uri", nameof(apiKeyAuthAddress));
            ApiKeyAuthAddress = baseUri;
        }

        public REstateAuthClient(StringSerializer stringSerializer, Uri apiKeyAuthAddress)
        {
            StringSerializer = stringSerializer;

            if (apiKeyAuthAddress == null) throw new ArgumentNullException(nameof(apiKeyAuthAddress));

            ApiKeyAuthAddress = apiKeyAuthAddress;
        }

        public async Task<string> GetAuthenticatedSessionTokenAsync(string apiKey, CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Json));

                var response = await httpClient.PostAsync(ApiKeyAuthAddress,
                    new StringContent($"{{ \"apiKey\": \"{apiKey}\" }}", Encoding.UTF8, Json)).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode) return null;

                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var tokenResponse = StringSerializer.Deserialize<JwtResponse>(responseBody);

                return tokenResponse.Jwt;
            }
        }
    }
}
