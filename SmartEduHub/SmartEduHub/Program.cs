using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartEduHub.CommonLibrary;
using SmartEduHub.Data;
using SmartEduHub.Interface;
using System.Text;
using Microsoft.OpenApi.Models;
using SmartEduHub.Repository;

var builder = WebApplication.CreateBuilder(args);

// ===== Add DbContext =====
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// ===== Add Controllers =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ===== Add Swagger with JWT configuration =====
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartEduHub API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey, 
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Description = "Please enter into field the word 'Bearer' followed by space and JWT token"
    });

    c.OperationFilter<AuthorizeCheckOperationFilter>();

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    c.CustomSchemaIds(x => x.FullName);
});

// ===== Your Application Services =====
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ITenantContextAccessor, TenantContextAccessor>();

// ===== JWT Authentication =====
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ===== Configure Middleware =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<TenantMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

