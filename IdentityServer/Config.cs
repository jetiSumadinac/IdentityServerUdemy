using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client{ 
                    ClientId = "movieClient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { 
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "movieApi" }
                },
                new Client
                {
                    ClientId = "movies_mvc_client",
                    ClientName = "Movies Mvc Web App",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>(){
                        "https://localhost:5002/signout-callback-oidc"
                    },
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>(){ 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "movieApi",
                        "roles"
                    }
                },
                new Client
                {
                    ClientId = "movies_external_client",
                    ClientName = "Movies external client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>(){
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "movieApi",
                        "roles"
                    },
                    AlwaysSendClientClaims = true
                },
                new Client
                {
                    ClientId = "apiResource.client",
                    ClientName = "Movies API client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>(){
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        "movieApi",
                        "roles"
                    },
                    AlwaysSendClientClaims = true
                }
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("movieApi","Movie API")
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource{ 
                    Name = "apiResource",
                    Description = "Web API resource",
                    Scopes = { "movieApi" },
                    //ApiSecrets= new List<Secret>{ 
                    //    new Secret("secret", "Movies.Api resource secret")
                    //},
                    UserClaims = new List<string>{ JwtClaimTypes.Role }
                }
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "Your Role(s)", new List<string>(){ "role" })
            };
        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "10D0C40D-C98E-46B0-9FDD-3F92DE0FC1CD",
                    Username = "stefan",
                    Password = "stefan",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "Stefan"),
                        new Claim(JwtClaimTypes.FamilyName, "Vukovic")
                    }
                }
            };
    }
}
