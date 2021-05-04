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
                        "movieApi"
                    }
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
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email()
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
