using DataAccessRedPaw.UserAccessData;
using DBAccess.DBAccess;
using RedPawChat.Server.DataAccess.DapperContext;
using Microsoft.AspNetCore.Identity;
using RedPaw.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using RedPawChat.Server.Services;
using WebApp.Models;
using WebApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using Microsoft.AspNetCore.Builder;
using RedPawChat.Server.DataAccess.Models.DTO;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
builder.Services.AddIdentity<User, ApplicationRole>();
builder.Services.AddTransient<LoginInfo>();
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
    options.Lockout.AllowedForNewUsers = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-." +
    "_@+àáâã´äåºæçè³¿éêëìíîïðñòóôõö÷øùüþÿÀÁÂÃ¥ÄÅªÆÇÈ²¯ÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÜÞß";
});
builder.Services.ConfigureApplicationCookie(options => 
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSignalR().AddAzureSignalR();
builder.Services.AddTransient<IDapperContext, DapperContext>();
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<IAdminDataAccess, AdminDataAccess>();
builder.Services.AddTransient<IUserDataAccess, UserDataAccess>();
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll",
            builder => builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials()
        ));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
app.UseSwaggerUI();
}
app.UseCors("AllowAll");


app.UseAuthentication();
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};
app.UseCookiePolicy(cookiePolicyOptions);
app.UseAuthorization();


app.UseRouting();
app.MapControllers();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action}/{id?}",
        defaults: new { controller = "Account", action = "Login" }
    );
    //app.MapHub<ChatHub>("/chat");
});

app.MapFallbackToFile("/index.html");

app.Run();
