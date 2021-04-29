using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
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
            };
        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
            };
    }
}
