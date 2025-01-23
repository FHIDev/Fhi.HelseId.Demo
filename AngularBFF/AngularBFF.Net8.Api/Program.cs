using AngularBFF.Net8.Api.Weather;
using Fhi.HelseId.Refit;
using Fhi.HelseId.Web.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets("b1a93959-6172-416e-bd25-8d43347eb8f3");
}
builder.Services.AddLogging();
builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authBuilder = builder.AddHelseIdWebAuthentication()
    .UseJwkKeySecretHandler()
    .Build();

// Sample using HttpClient (Comment in WithHttpClient, comment out WithRefit)
////builder.WithHttpClient(authBuilder);

// Sample of using Refit (Comment in WithRefit, comment out WithHttpClient)
builder.WithRefit();

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


/// <summary>
/// The below extension methods illustrating two ways of downstream API call with access token. 
/// </summary>
internal static class ApiExtensions
{
    /// <summary>
    /// Using refit to add access token to the API call
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    internal static WebApplicationBuilder WithRefit(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IWeatherForecastService, WeatherForecastServiceWithRefit>();
        builder
            .AddHelseidRefitBuilder()
            .AddRefitClient<IWeatherForcastApi>(nameof(WeatherForecastServiceWithRefit));

        return builder;
    }

    /// <summary>
    /// Using HttpClient to add access token to the API call
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="authBuilder"></param>
    /// <returns></returns>
    internal static WebApplicationBuilder WithHttpClient(this WebApplicationBuilder builder, HelseIdWebAuthBuilder authBuilder)
    {
        builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();
        authBuilder
             .AddOutgoingApis()
             .WithHttpClients();

        return builder;
    }
}