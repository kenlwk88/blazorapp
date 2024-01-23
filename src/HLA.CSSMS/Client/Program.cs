global using HLA.CSSMS.Client;
global using HLA.CSSMS.Client.Helpers;
global using HLA.CSSMS.Client.Services.AuthService;
global using HLA.CSSMS.Client.Services.UserAccountManager;
global using HLA.CSSMS.Client.Services.IMS.Submission.IMSSubmissionManager;
global using HLA.CSSMS.Shared.Helpers;
global using HLA.CSSMS.Shared.Models;
global using HLA.CSSMS.Shared.Dtos;
global using HLA.CSSMS.Shared.Dtos.IMS;
global using HLA.CSSMS.Shared.Enums;
global using Blazored.LocalStorage;
global using System.Net.Http.Json;
global using System.Security.Claims;
global using Syncfusion.Blazor.Grids;
global using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

#region Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserAccountManager, UserAccountManager>();
builder.Services.AddScoped<IIMSSubmissionManager, IMSSubmissionManager>();
#endregion

#region Authorization and Authentication
builder.Services.AddAuthorizationCore(config =>
{
    config.AddPolicy(Policies.IsSuperAdmin, Policies.IsSuperAdminPolicy());
    config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
    config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
});
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
#endregion

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSyncfusionBlazor();

await builder.Build().RunAsync();
