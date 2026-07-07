using System.Text;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Services;
using System.IdentityModel.Tokens.Jwt;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    // O .NET 9 Nativo usa Transformers para adicionar segurança
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Sua API";
        document.Info.Version = "v1";
        
        var requirements = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                }] = Array.Empty<string>()
            }
        };
        document.SecurityRequirements = requirements;
        return Task.CompletedTask;
    });
});


builder.Services.AddCors(options => {

    options.AddPolicy("AllowFront", policy => {
        policy
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();

    });

});


builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });


var key = builder.Configuration["Jwt:Key"];

if(string.IsNullOrEmpty(key))
    throw new Exception("Null Jwt Key");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"]
            
        };
    });


builder.Services.AddAuthorization();


var defaultConnection = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMySql(
        defaultConnection,
        ServerVersion.AutoDetect(defaultConnection)
    );
});



builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();


if(app.Environment.IsDevelopment()) {
    app.MapOpenApi(); 
    app.MapScalarApiReference();
}


app.UseCors("AllowFront");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

