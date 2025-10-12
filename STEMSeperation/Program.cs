using System.Text;
using BusinessLayerLogic.ExternalProcesses;
using BusinessLayerLogic.Services;
using BusinessLayerLogic.Services.Contracts;
using DatabaseLayerLogic.Models;
using DatabaseLayerLogic.Repositories;
using DatabaseLayerLogic.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; 
builder.Services.AddDbContext<StemseperationContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")
));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IConsoleAppRunner, ConsoleAppRunner>();
builder.Services.AddScoped<IUserFileService, UserFileService>(); 
builder.Services.AddScoped<IFilesRepository,FilesRepository>();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            // policy.WithOrigins("http://localhost:3000");
            policy.AllowAnyOrigin(); 
            policy.AllowAnyHeader(); 
            policy.AllowAnyMethod();
        }
    ); 
}
); 
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
})
    .AddGoogle(options =>
    {
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = googleAuthNSection["ClientId"] ?? throw new InvalidOperationException("Google ClientId is not configured.");
        options.ClientSecret = googleAuthNSection["ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret is not configured.");
        options.CallbackPath = "/api/LoginAndRegister/GoogleCallback";
    });

builder.Services.AddAuthorization();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "STEMSeperation API V1");
    c.RoutePrefix = "swagger"; // This is default, but you can set to "" to load at root
});
// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    //app.UseExceptionHandler("/Home/Error");
//    //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    //app.UseHsts();
//}

//app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    await next();
});
app.UseStaticFiles();

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins); 
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();
