using DataAccessRedPaw.UserAccessData;
using DBAccess.DBAccess;
using RedPawChat.Server;
using RedPawChat.Server.DataAccess.DapperContext;
using Microsoft.AspNetCore.Identity;
using RedPaw.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using RedPawChat.Server.Services;
using AspNetCore.Identity.Dapper;
using WebApp.Models;
using WebApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();

builder.Services.AddIdentity<User, ApplicationRole>();

builder.Services.AddAuthentication("RedPawAuth").AddCookie("RedPawAuth", options =>
{
    options.Cookie.Name = "RedPawAuth";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
}
) ;

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccessChatResources", policy =>
    {
        policy.RequireRole("Admin", "User");
    });   
});
builder.Services.AddControllers();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR().AddAzureSignalR();
builder.Services.AddTransient<IDapperContext, DapperContext>();
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<IAdminDataAccess, AdminDataAccess>();
builder.Services.AddTransient<IUserDataAccess, UserDataAccess>();
builder.Services.AddCors();

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

app.UseCors(builder => builder.AllowAnyOrigin());
app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    app.MapHub<ChatHub>("/chat");
});

app.MapFallbackToFile("/index.html");

app.Run();
