// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Hotelix.IdentityServer.Data;
using Hotelix.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Hotelix.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var pracownik = roleMgr.FindByNameAsync("Pracownik").Result;
                    if (pracownik == null)
                    {
                        pracownik = new IdentityRole
                        {
                            Name = "Pracownik"
                        };
                        _ = roleMgr.CreateAsync(pracownik).Result;
                    }

                    var administrator = roleMgr.FindByNameAsync("Administrator").Result;
                    if (administrator == null)
                    {
                        administrator = new IdentityRole
                        {
                            Name = "Administrator"
                        };
                        _ = roleMgr.CreateAsync(administrator).Result;
                    }

                    var klient = roleMgr.FindByNameAsync("Klient").Result;
                    if (klient == null)
                    {
                        klient = new IdentityRole
                        {
                            Name = "Klient"
                        };
                        _ = roleMgr.CreateAsync(klient).Result;
                    }

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    // ALICE
                    var alice = userMgr.FindByNameAsync("alice").Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = "alice",
                            Email = "AliceSmith@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("alice created");
                    }
                    else
                    {
                        Log.Debug("alice already exists");
                    }

                    if (!userMgr.IsInRoleAsync(alice, pracownik.Name).Result)
                    {
                        _ = userMgr.AddToRoleAsync(alice, pracownik.Name).Result;
                    }

                    // BOB

                    var bob = userMgr.FindByNameAsync("bob").Result;
                    if (bob == null)
                    {
                        bob = new ApplicationUser
                        {
                            UserName = "bob",
                            Email = "BobSmith@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("bob created");
                    }
                    else
                    {
                        Log.Debug("bob already exists");
                    }

                    if (!userMgr.IsInRoleAsync(bob, klient.Name).Result)
                    {
                        _ = userMgr.AddToRoleAsync(bob, klient.Name).Result;
                    }

                    // JOHN
                    var john = userMgr.FindByNameAsync("john").Result;
                    if (john == null)
                    {
                        john = new ApplicationUser
                        {
                            UserName = "john",
                            Email = "JohnSmith@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(john, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(john, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "John Smith"),
                            new Claim(JwtClaimTypes.GivenName, "John"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://john.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("john created");
                    }
                    else
                    {
                        Log.Debug("john already exists");
                    }

                    if (!userMgr.IsInRoleAsync(john, administrator.Name).Result)
                    {
                        _ = userMgr.AddToRoleAsync(john, administrator.Name).Result;
                    }

                }
            }
        }
    }
}
