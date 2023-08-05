using Acerola.Application.Commands.CloseAccount;
using Acerola.Application.Commands.Deposit;
using Acerola.Application.Commands.Register;
using Acerola.Application.Commands.Withdraw;
using Acerola.Application.Queries;
using Acerola.Application.Repositories;
using Acerola.Infrastructure.EntityFrameworkDataAccess;
using Acerola.Infrastructure.EntityFrameworkDataAccess.Queries;
using Acerola.Infrastructure.EntityFrameworkDataAccess.Repositories;
using Acerola.WebApi.Filters;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging(true);
});

builder.Services.AddScoped<CustomExceptionFilter>();
builder.Services.AddScoped<ValidateModelAttribute>();

builder.Services.AddScoped<IAccountReadOnlyRepository, AccountRepository>();
builder.Services.AddScoped<IAccountWriteOnlyRepository, AccountRepository>();
builder.Services.AddScoped<ICustomerReadOnlyRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerWriteOnlyRepository, CustomerRepository>();

builder.Services.AddScoped<IAccountsQueries, AccountsQueries>();
builder.Services.AddScoped<ICustomersQueries, CustomersQueries>();

builder.Services.AddScoped<ICloseAccountUseCase, CloseAccountUseCase>();
builder.Services.AddScoped<IDepositUseCase, DepositUseCase>();
builder.Services.AddScoped<IRegisterUseCase, RegisterUseCase>();
builder.Services.AddScoped<IWithdrawUseCase, WithdrawUseCase>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
    options.Filters.Add<ValidateModelAttribute>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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