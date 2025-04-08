using Microsoft.EntityFrameworkCore;
using WalletApp.Application.Interfaces;
using WalletApp.Application.Services;
using WalletApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// CONN SQL
builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección dep.
builder.Services.AddScoped<IWalletService, WalletService>();

builder.Services.AddControllers()
    .AddNewtonsoftJson();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
