using Microsoft.EntityFrameworkCore;
using WalletApp.API;
using WalletApp.Application.Interfaces;
using WalletApp.Application.Services;
using WalletApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigurarServicios();

/*************************************/
var app = builder.Build();
// Middleware
app.ConfigurarPipeline();
/*************************************/
app.Run();
