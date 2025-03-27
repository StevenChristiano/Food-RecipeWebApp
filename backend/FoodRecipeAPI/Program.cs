using FoodRecipeAPI.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MediatR;
using System.Reflection;
using FoodRecipeAPI.Middleware;
using FluentValidation;
using FoodRecipeAPI.Application.Validators;
using FoodRecipeAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("logs/Log-.txt", rollingInterval: RollingInterval.Day));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<RecipeQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRecipeValidator>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs",
        policy => policy.WithOrigins("http://localhost:3000", "http://localhost:5267") // Adjust this URL
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowNextJs");
app.UseMiddleware<ExceptionHandlingMiddleware>();


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
