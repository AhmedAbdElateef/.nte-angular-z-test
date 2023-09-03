using realTimeServices.Hubs;
using realTimeServices.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ChatService>();

builder.Services.AddSignalR();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    // .WithOrigins("https://localhost","http://localhost:4200","https://localhost:7288","https://localhost:8090")
    .AllowCredentials()
);


var frontendUrl = builder.Configuration.GetSection("frontendUrl")["Url"] ??
    throw new InvalidOperationException();



// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");
app.UseHttpsRedirection();

app.Run();