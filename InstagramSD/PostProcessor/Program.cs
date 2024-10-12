using Common.ServiceBus;
using Common.Settings;
using Domain.Factories;
using Domain.Strategies;
using PostProcessor.Factories;
using PostProcessor.Repositories;
using PostProcessor.Services;
using PostProcessor.Settings;
using PostProcessor.Strategies;
using PostProcessor.Tasks;
using PostProcessor.TaskServices;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services
builder.Services.AddScoped<IPostTopicConsumerTaskService, PostTopicConsumerTaskService>();
builder.Services.AddScoped<IServiceBusConsumer, PostProcessorServiceBusConsumer>();
builder.Services.AddScoped<IProcessPostCreatedService, ProcessPostCreatedService>();
builder.Services.AddScoped<IRedisCacheRepository, RedisCacheRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IEventProcessorFactory, EventProcessorFactory>();
builder.Services.AddScoped<IEventProcessorStrategy, PostCreatedEventProcessorStrategy>();

//redis
var redisConnectionString = builder.Configuration["RedisSettings:ConnectionString"];
builder.Services.AddSingleton<IConnectionMultiplexer>(c => ConnectionMultiplexer.Connect(redisConnectionString));

//settings
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBusSettings"));
builder.Services.Configure<ConsumerTaskServiceSettings>(builder.Configuration.GetSection("ConsumerTaskServiceSettings"));
builder.Services.Configure<RankServiceHttpClientSettings>(builder.Configuration.GetSection("RankServiceHttpClientSettings"));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
builder.Services.Configure<PostProcessorServiceBusConsumerSettings>(builder.Configuration.GetSection("PostProcessorServiceBusConsumerSettings"));

//hostedServices
builder.Services.AddHostedService<ConsumerTaskHostedService>();

//httpclient
builder.Services.AddHttpClient(
    builder.Configuration["RankServiceHttpClientSettings:Name"],
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["RankServiceHttpClientSettings:BaseUrl"]);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

