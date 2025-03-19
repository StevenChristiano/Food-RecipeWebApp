using FoodRecipeAPI.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MediatR;
using System.Reflection;
using FoodRecipeAPI.Middleware;
using FluentValidation;
using FoodRecipeAPI.Application.Validators;
using FoodRecipeAPI.Application.Commands;
using FoodRecipeAPI.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/Log-.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<RecipeQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRecipeValidator>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
