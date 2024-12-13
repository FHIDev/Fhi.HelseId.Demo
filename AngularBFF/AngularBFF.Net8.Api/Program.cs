using AngularBFF.Net8.Api.Http;
using AngularBFF.Net8.Api.Weather;
using Fhi.HelseId.Web.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets("b1a93959-6172-416e-bd25-8d43347eb8f3");
}

builder.Configuration
.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
.AddUserSecrets("b1a93959-6172-416e-bd25-8d43347eb8f3")
.AddEnvironmentVariables();

builder.Services.AddLogging();
builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<BearerTokenHandler>();
builder.Services.ConfigureHttpClientDefaults(b => b.AddHttpMessageHandler<BearerTokenHandler>());

builder.Services.AddHttpClient("weatherApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7278/");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authBuilder = builder.AddHelseIdWebAuthentication()
    .UseJwkKeySecretHandler()
    .Build();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseHelseIdProtectedPaths();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
