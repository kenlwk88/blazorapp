global using HLA.CSSMS.Shared.Models;
global using HLA.CSSMS.Shared.Dtos;
global using HLA.CSSMS.Shared.Dtos.IMS;
global using HLA.CSSMS.Shared.Enums;
global using Microsoft.EntityFrameworkCore;
global using HLA.CSSMS.Server.Data;
global using HLA.CSSMS.Shared.Helpers;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using HLA.CSSMS.Server.Services.AuthService;
global using HLA.CSSMS.Server.Services.UserAccountService;
global using HLA.CSSMS.Server.Services.IMS;
global using System.Data;
global using Dapper;
global using HLA.CSSMS.Server.Data.IMS;


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
string jwtIssuer = builder.Configuration["JwtIssuer"];
string jwtAudience = builder.Configuration["JwtAudience"];
string jwtSecurityKey = builder.Configuration["JwtSecurityKey"];


#region Entity Framework Core
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>();
#endregion

#region Repository
builder.Services.AddSingleton<ApplicationDbContext>();
builder.Services.AddSingleton<SubmissionRepo>();
#endregion

#region Authorization and Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtIssuer,
				ValidAudience = jwtAudience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecurityKey)),
				ClockSkew = TimeSpan.FromSeconds(0)
			};
		});

// Define Policy in Authorization
builder.Services.AddAuthorization(config =>
{
	config.AddPolicy(Policies.IsSuperAdmin, Policies.IsSuperAdminPolicy());
	config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
	config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
});
#endregion

#region Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IIMSSubmissionService, IMSSubmissionService>();
#endregion

#region Configuration
builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
#endregion

app.Run();
