﻿using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MovieApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<Movie> CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();

        }

        public Task<Movie> GetMovie(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            //WAY 1
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Movies");
            
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            
             var content = await response.Content.ReadAsStringAsync();
            var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);

            return movieList;

            //WAY 2
            
            //// 1 - Get token from Identity server
            //// 2 - send reguest to protected API
            //// 3 - Deserialize object to MovieList

            ////1 . "retrieve" our API credentials.  this must be registered on identity server
            //var apiCredentials = new ClientCredentialsTokenRequest
            //{
            //    Address = "Https://localhost:44380/connect/token",
            //    ClientId = "movieClient",
            //    ClientSecret = "secret",

            //    ////this is the scope our API requires
            //    Scope = "movieApi",
            //};

            //var client = new HttpClient();
            //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44380");
            //if (disco.IsError)
            //    return null;

            ////2. authenticate and get an access token from identity server
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiCredentials);
            //if (tokenResponse.IsError)
            //    return null;

            ////3. set the access_token in the request authorization : Bearer <token>
            //var apiCLient = new HttpClient();
            //apiCLient.SetBearerToken(tokenResponse.AccessToken);

            ////4. send request to our ProtectedAPI
            //var response = await apiCLient.GetAsync("https://localhost:44364/api/movies");
            //response.EnsureSuccessStatusCode();


            //var content = await response.Content.ReadAsStringAsync();
            //List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);

            //return movieList;
        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var ipdCLient = _httpClientFactory.CreateClient("IDPClient");
            var metaDataResponse = await ipdCLient.GetDiscoveryDocumentAsync();

            if (metaDataResponse.IsError)
                throw new HttpRequestException("something went wrong while requesting the access token");

            var accessToken = await _httpContextAccessor
                .HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfoResponse = await ipdCLient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = metaDataResponse.UserInfoEndpoint,
                Token = accessToken
            });

            if (userInfoResponse.IsError)
                throw new HttpRequestException("something went wrong whle getting user info");

            var userInfoDictionary = new Dictionary<string, string>();
            foreach (var claim in userInfoResponse.Claims)
                userInfoDictionary.Add(claim.Type, claim.Value);

            return new UserInfoViewModel(userInfoDictionary);
        }

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
