using DataAccessRedPaw.UserAccessData;
using DBAccess.DBAccess;
using RedPawChat.Server;
using RedPawChat.Server.DataAccess.DapperContext;
using Microsoft.AspNetCore.Identity;
using RedPaw.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using RedPawChat.Server.Services;
using WebApp.Models;
using WebApp.Data;
using System.Security.Claims;
using RedPawChat.Server.DataAccess.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
builder.Services.AddIdentity<User, ApplicationRole>();
builder.Services.AddTransient<LoginInfo>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    //options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
});
builder.Services.AddDataProtection();
builder.Services.AddAuthentication("RedPawAuth").AddCookie("RedPawAuth", options =>
{
    options.Cookie.Name = "RedPawAuth";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
}) ;

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccessChatResources", policy =>
    {
        policy.RequireClaim("Role", "User");
    });   
});

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
builder.Services.AddCors(options=>
    options.AddPolicy("AllowSpecificOrigin",
            builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
        ));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

//app.UseCors(builder => builder.AllowAnyOrigin());
app.UseCors("AllowSpecificOrigin");
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //app.MapHub<ChatHub>("/chat");
});

//app.MapFallbackToFile("/index.html");

app.Run();
