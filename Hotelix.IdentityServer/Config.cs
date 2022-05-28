// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Hotelix.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("offer", "Hotelix Offer API")
                {
                    Scopes = { "offer.read", "offer.write" }
                },
                new ApiResource("reservations", "Hotelix Reservations API")
                {
                    Scopes = { "reservations.read", "reservations.write" }
                }
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                //new ApiScope("reservations.fullaccess"),
                new ApiScope("reservations.read"),
                new ApiScope("reservations.write"),
                new ApiScope("offer.fullaccess"),
                new ApiScope("offer.read"),
                new ApiScope("offer.write")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {

                // m2m client credentials flow client
                new Client
                {
                    ClientName = "Hotelix machine to machine client",
                    ClientId = "hotelixm2m",
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = { "offer.fullaccess" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientName = "Hotelix interactive client",
                    ClientId = "hotelixinteractive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                    RedirectUris = { "https://localhost:5000/signin-oidc" },
                    //FrontChannelLogoutUri = "https://localhost:5000/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5000/signout-callback-oidc" },

                    //AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "offer.read", "offer.write" }
                },

                new Client
                {
                    ClientName = "Hotelix client",
                    ClientId = "hotelix",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RedirectUris = { Startup.StaticConfiguration["ApiConfigs:Client:Uri"] + "signin-oidc" },
                    PostLogoutRedirectUris = { Startup.StaticConfiguration["ApiConfigs:Client:Uri"] + "signout-callback-oidc" },
                    AllowedScopes = {
                        "openid", "profile",
                        "offer.read", "offer.write",
                        "reservations.read", "reservations.write"
                    }
                },
            };
    }
}