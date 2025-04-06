using DS.Users.Application.Commands.CreateUser;
using DS.Users.Application.Interfaces;
using DS.Users.Infrastructure.Persistence;
using DS.Users.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DS.Users.Application.Commands.UpdateUser;
using DS.Shared.Logging;
using DS.Users.Infrastructure.Logging;
using DS.Users.Application.UseCases.CreateUser;
using DS.Users.Application.UseCases.DeleteUser;
using DS.Users.Application.UseCases.GetUser;
using DS.Users.Application.UseCases.UpdateUser;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Register MediatR and other handlers
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly));

// Register Use Cases and other services
builder.Services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
builder.Services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
builder.Services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
builder.Services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();

// Register Validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();

builder.Services.AddSingleton<DS.Shared.Logging.ILoggerManager, LoggerManager>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<DS.Users.API.Middleware.CustomExceptionMiddleware>();


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
