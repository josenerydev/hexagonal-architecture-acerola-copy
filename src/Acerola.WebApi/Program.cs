using Acerola.WebApi.Filters;

using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add<CustomExceptionFilter>();
//    options.Filters.Add<ValidateModelAttribute>();
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Switch to Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        // Add the configuration to the ConfigurationBuilder.
        var config = new ConfigurationBuilder();
        // config.AddJsonFile comes from Microsoft.Extensions.Configuration.Json
        config.AddJsonFile("autofac.json");
        config.AddJsonFile("autofac.entityframework.json");

        // Register the ConfigurationModule with Autofac.
        var module = new ConfigurationModule(config.Build());
        containerBuilder.RegisterModule(module);

        // Populate the services from the builder to the container
        containerBuilder.Populate(builder.Services);
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