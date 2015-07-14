using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using NetatmoBot.Exceptions;
using NetatmoBot.Model;
using NetatmoBot.Services.AuthenticationModels;

namespace NetatmoBot.Services
{
    public class AuthenticationService
    {
        readonly Uri _uri = new Uri("https://api.netatmo.net/oauth2/token");
        private readonly string _clientId;
        private readonly string _clientSecret;

        public AuthenticationService()
        {
            _clientId = System.Configuration.ConfigurationManager.AppSettings["Netatmo.ClientId"];
            _clientSecret = System.Configuration.ConfigurationManager.AppSettings["Netatmo.ClientSecret"];
        }

        public AuthenticationToken AuthenticateUser()
        {
            string userName = System.Configuration.ConfigurationManager.AppSettings["Netatmo.UserName"];
            string password = System.Configuration.ConfigurationManager.AppSettings["Netatmo.Password"];
            return AuthenticateUser(userName, password);
        }

        public AuthenticationToken AuthenticateUser(string username, string password)
        {
            var client = new HttpClient();

            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                // Not needed for public data
                new KeyValuePair<string, string>("scope", "read_station")
            };

            HttpContent request = new FormUrlEncodedContent(keyValuePairs);
            HttpResponseMessage response = client.PostAsync(_uri, request).Result;
            if (!response.IsSuccessStatusCode)
            {
                Trace.WriteLine("Authentication Failed");
                throw new NetatmoReadException("Authentication failed. Status Code: " + response.StatusCode);
            }

            var authenticationResponse = response.Content.ReadAsAsync<AuthenticationResponse>().Result;
            Trace.WriteLine(authenticationResponse.access_token);
            Trace.WriteLine(authenticationResponse.expires_in);
            Trace.WriteLine(authenticationResponse.refresh_token);

            return new AuthenticationToken
            {
                Token = authenticationResponse.access_token,
                RefreshToken = authenticationResponse.refresh_token,
                ExpiresAt = DateTime.UtcNow.AddSeconds(authenticationResponse.expires_in)
            };
        }

        public AuthenticationToken RefreshToken(AuthenticationToken authenticationToken)
        {
            var client = new HttpClient();

            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("refresh_token", authenticationToken.RefreshToken),
            };

            HttpContent request = new FormUrlEncodedContent(keyValuePairs);
            HttpResponseMessage response = client.PostAsync(_uri, request).Result;
            if (!response.IsSuccessStatusCode)
            {
                Trace.WriteLine("Authentication Refresh Failed");
                throw new NetatmoReadException("Authentication refresh token failed. Status Code: " + response.StatusCode);
            }

            var authenticationResponse = response.Content.ReadAsAsync<AuthenticationResponse>().Result;
            Trace.WriteLine(authenticationResponse.access_token);
            Trace.WriteLine(authenticationResponse.expires_in);
            Trace.WriteLine(authenticationResponse.refresh_token);

            // Update the existing token
            authenticationToken.Token = authenticationResponse.access_token;
            authenticationToken.ExpiresAt = DateTime.UtcNow.AddSeconds(authenticationResponse.expires_in);
            authenticationToken.RefreshToken = authenticationResponse.refresh_token;

            return authenticationToken;
        }
    }
}