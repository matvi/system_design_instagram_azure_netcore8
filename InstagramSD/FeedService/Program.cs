
using System.Text.Json;
using System.Text.Json.Serialization;
using FeedService.Factories;
using FeedService.Services;
using FeedService.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services
builder.Services.AddScoped<INewsFeedService, NewsFeedService>();
builder.Services.AddScoped<IFeedResponseFactory, FeedResponseFactory>();

//settings
builder.Services.Configure<PostHttpClientSettings>(builder.Configuration.GetSection("PostHttpClientSettings"));
builder.Services.Configure<PostProcessorHttpClientSettings>(builder.Configuration.GetSection("PostProcessorHttpClientSettings"));

//httpclient
builder.Services.AddHttpClient(
    builder.Configuration["PostHttpClientSettings:Name"],
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["PostHttpClientSettings:BaseUrl"]);
    });

builder.Services.AddHttpClient(
    builder.Configuration["PostProcessorHttpClientSettings:Name"],
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["PostProcessorHttpClientSettings:BaseUrl"]);
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();

    });
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

