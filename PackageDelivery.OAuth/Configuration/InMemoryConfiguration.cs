using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace PackageDelivery.OAuth.Configuration
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources() {
            return new[] {
                new ApiResource("packagedelivery", "Package Delivery") {
                    UserClaims = new [] {"email"}
                }
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources() {
            return new IdentityResource[] {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<Client> Clients() {
            return new[] {
                new Client {
                    ClientId = "packagedelivery",
                    ClientSecrets = new [] {new Secret("secret".Sha256()), },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] {"packagedelivery"}
                },
                new Client {
                    ClientId = "packagedelivery_implicit",
                    ClientSecrets = new [] {new Secret("secret".Sha256()), },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "packagedelivery"
                    },
                    RedirectUris = new [] { "http://localhost:59898/signin-oidc" },
                    PostLogoutRedirectUris = new [] { "http://localhost:59898/signout-callback-oidc" },
                    AllowAccessTokensViaBrowser = true
                },
                new Client {
                    ClientId = "packagedelivery_code",
                    ClientSecrets = new [] {new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowedScopes = new [] {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "packagedelivery"
                    },
                    AllowOfflineAccess = true,
                    RedirectUris = new [] { "http://localhost:59898/signin-oidc" },
                    PostLogoutRedirectUris = new [] { "http://localhost:59898/signout-callback-oidc" },
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}
