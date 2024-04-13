// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Eshopping.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalogapi"),
                 new ApiScope("basketapi"),
                 new ApiScope("catalogapi.read"),
                 new ApiScope("catalogapi.write"),
                 new ApiScope("eShoppinggateway")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                //List of Microservices can go here.
                new ApiResource("Catalog","Catalog.API")
                {
                    Scopes={ "catalogapi.read", "catalogapi.write" }
                },
                new ApiResource("Basket","Basket.API")
                {
                    Scopes={ "basketapi" }
                },
                new ApiResource("EShoppingGateway","EShopping Gateway")
                {
                    Scopes={ "eShoppinggateway" }
                }

            };
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientName="Catalog API Client",
                    ClientId="CatalogApiClient",
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "catalogapi.read", "catalogapi.write" }

                },
                new Client
                {
                    ClientName="Basket API Client",
                    ClientId="BasketApiClient",
                    ClientSecrets = { new Secret("511536EF-F270-6789-80CA-1C89C192F69A".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={  "basketapi" }

                },
                new Client
                {
                    ClientName="EShopping Gateway Client",
                    ClientId="EShoppingGatewayClient",
                    ClientSecrets = { new Secret("511536EF-F270-1256-80CA-1C89C192F69A".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "eShoppinggateway", "basketapi" }

                }
                //new Client
                //{
                //    ClientId = "m2m.client",
                //    ClientName = "Client Credentials Client",

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //    AllowedScopes = { "scope1" }
                //},

                //// interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //},
            };
    }
}