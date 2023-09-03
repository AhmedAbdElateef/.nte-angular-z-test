using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<PostService>();
builder.Services.AddSingleton<ChatService>();
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddCors();

// grpc
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();




var jwtsecret = builder.Configuration.GetSection("JwtSecret")["Secret"] ??
    throw new InvalidOperationException();
// Console.WriteLine(jwtsecret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = "https://localhost:5001",
        ValidAudience = "https://localhost:5001",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((jwtsecret))),
        ClockSkew= TimeSpan.Zero,
    };
});



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();
// grpc

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    // .WithOrigins("https://localhost","http://localhost:4200","https://localhost:8000","https://localhost:8090")
    .AllowCredentials()
);

app.MapGrpcService<RealtimechatService>();
app.MapGrpcReflectionService();
// grpc



app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();



app.MapControllers();
app.UseHttpsRedirection();


app.MapControllers();



// var frontendUrl = builder.Configuration.GetSection("frontendUrl")["Url"] ??
//     throw new InvalidOperationException();



app.Run();
