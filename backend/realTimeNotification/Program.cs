using realTimeNotification.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddCors();

// grpc
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();


var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    // .WithOrigins("https://localhost","http://localhost:4200","https://localhost:8000","https://localhost:8090")
    .AllowCredentials()
);

// grpc
app.MapGrpcService<NotificationService>();
app.MapGrpcReflectionService();
// grpc


// var frontendUrl = builder.Configuration.GetSection("frontendUrl")["Url"] ??
//     throw new InvalidOperationException();
    
// app.UseCors(x => x.AllowAnyHeader()
//                   .AllowAnyMethod()
//                   .AllowCredentials()
//     .WithOrigins("http://localhost:4200")
//                   );



app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/Notifications");
app.UseHttpsRedirection();

app.Run();
