using Common.ServiceBus;
using Domain.Factories;
using Domain.Strategies;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using RankService.Data;
using RankService.Factories;
using RankService.Repositories;
using RankService.Services;
using RankService.Services.ServiceBusConsumers;
using RankService.Settings;
using RankService.Strategies;
using RankService.Tasks;
using RankService.TaskServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services

builder.Services.AddScoped<IFollowTopicConsumerTaskService, FollowTopicConsumerTaskService>();
builder.Services.AddKeyedScoped<IServiceBusConsumer, FollowTopicServiceBusConsumer>("FollowTopicServiceBusConsumer");
builder.Services.AddKeyedScoped<IServiceBusConsumer, LikeTopicServiceBusConsumer>("LikeTopicServiceBusConsumer");
builder.Services.AddScoped<IEventProcessorFactory, RankEventProcessorFactory>();
builder.Services.AddScoped<IEventProcessorStrategy, FollowCreatedEventProccessorStrategy>();
builder.Services.AddScoped<IEventProcessorStrategy, UnfollowCreatedEventProccessorStrategy>();
builder.Services.AddScoped<IEventProcessorStrategy, UnlikeCreatedEventProcessorStrategy>();
builder.Services.AddScoped<IEventProcessorStrategy, LikeCreatedEventProcessorStrategy>();
builder.Services.AddScoped<ILikeTopicConsumerTaskService, LikeTopicConsumerTaskService>();

builder.Services.AddScoped<IRankService, RankService.Services.RankService>();

//repositories
builder.Services.AddScoped<IRankRepository, RankRepository>();

//settings
builder.Services.Configure<FollowConsumerHostedServiceSettings>(builder.Configuration.GetSection("FollowConsumerHostedServiceSettings")); 
builder.Services.Configure<FollowTopicServiceBusConsumerSettings>(builder.Configuration.GetSection("FollowTopicServiceBusConsumerSettings"));
builder.Services.Configure<LikeConsumerHostedServiceSettings>(builder.Configuration.GetSection("LikeConsumerHostedServiceSettings"));
builder.Services.Configure<LikeTopicServiceBusConsumerSettings>(builder.Configuration.GetSection("LikeTopicServiceBusConsumerSettings"));

//hosted services
builder.Services.AddHostedService<FollowConsumerHostedService>();
builder.Services.AddHostedService<LikeConsumerHostedService>();

//database
var CosmosDbEndpoint = "AddCosmosDbEndpoint";
var CosmosDbAuthKey = "addCosmosDbAuthKey";
var CosmosDbName = "instagramdb";

builder.Services.AddDbContext<RankDbContext>(
   options => options.UseCosmos(
       CosmosDbEndpoint,
       CosmosDbAuthKey,
       CosmosDbName,
       cosmosCLientOptions =>
           new CosmosClientOptions
           {
               IdleTcpConnectionTimeout = new System.TimeSpan(1, 0, 0, 0)
           }));

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

