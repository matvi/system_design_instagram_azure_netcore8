using PostService.Settings;
using Azure.Identity;
using Azure.Storage.Blobs;
using PostService.Services;
using PostService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using PostService.Repositories;
using Common.Settings;
using Common.ServiceBus;
using PostService.TaskServices;
using PostService.Tasks;
using PostService.Strategies;
using Domain.Strategies;
using Domain.Factories;
using PostService.Factories;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//When the above code is run on your local workstation during local development, it will look in the environment variables for an application service principal or at Visual Studio, VS Code, the Azure CLI, or Azure PowerShell for a set of developer credentials, either of which can be used to authenticate the app to Azure resources during local development.
//https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication/?tabs=command-line#defaultazurecredential

//services
builder.Services.AddSingleton<BlobServiceClient>(x =>
    new BlobServiceClient(
        new Uri("https://dmvsystemdesign.blob.core.windows.net"),
        new DefaultAzureCredential()));
builder.Services.AddScoped<IPostService, PostInstagramService>();
builder.Services.AddScoped<IEventBus, EventBus>();
builder.Services.AddKeyedScoped<IServiceBusConsumer, LikeTopicServiceBusConsumer>("LikeTopicServiceBusConsumer");
builder.Services.AddScoped<ILikeTopicConsumerTaskService, LikeTopicConsumerTaskService>();
builder.Services.AddScoped<ILikeCreatedProcessorService, LikeCreatedProcessorService>();
builder.Services.AddScoped<IUnlikeCreatedProcessorService, UnlikeCreatedProcessorService>();
builder.Services.AddScoped<IEventProcessorStrategy, LikeCreatedEventProcessorStrategy>();
builder.Services.AddScoped<IEventProcessorStrategy, UnlikeCreatedEventProcessorStrategy>();
builder.Services.AddScoped<IEventProcessorFactory, EventProcessorFactory>();

builder.Services.AddSingleton<IServiceProvider>(provider => provider);

//repositories
builder.Services.AddScoped<IPostRepository, PostRepository>();

//hostedServices
builder.Services.AddHostedService<PostConsumerHostedService>();

//database
var CosmosDbEndpoint = "AddCosmosDbEndpoint";
var CosmosDbAuthKey = "addCosmosDbAuthKey";
var CosmosDbName = "instagramdb";

builder.Services.AddDbContext<AppDbContext>(
   options => options.UseCosmos(
       CosmosDbEndpoint,
       CosmosDbAuthKey,
       CosmosDbName,
       cosmosCLientOptions =>
           new CosmosClientOptions
           {
               IdleTcpConnectionTimeout = new System.TimeSpan(1, 0, 0, 0)
           }));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<BlobSettings>(builder.Configuration.GetSection("BlobSettings"));
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBusSettings"));
builder.Services.Configure<PostConsumerHostedServiceSettings>(builder.Configuration.GetSection("PostConsumerHostedServiceSettings"));
builder.Services.Configure<LikeTopicServiceBusConsumerSettings>(builder.Configuration.GetSection("LikeTopicServiceBusConsumerSettings"));
//builder.Services.Configure<UnlikePostServiceBusConsumerSettings>(builder.Configuration.GetSection("UnlikePostServiceBusConsumerSettings"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();

    });
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

