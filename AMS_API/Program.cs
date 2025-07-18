using System.Text;
using BLL;
using BLL.IService;
using BLL.Service;
using DAL.IRepository;
using DAL.Repository;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog for logging
Serilog.Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/AMSlogs.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

// get connection string
string conn = builder.Configuration.GetConnectionString("Default")!;

// Add services to the container.
builder.Services.AddDbContext<AMSContext>(q => q.UseNpgsql(conn));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// add automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddAuthentication("Bearer")
   .AddJwtBearer("Bearer", options =>
   {
       options.RequireHttpsMetadata = false;
       options.SaveToken = true;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["Jwt:Issuer"],
           ValidAudience = builder.Configuration["Jwt:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
           ClockSkew = TimeSpan.Zero
       };
   });
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("amsfrontend", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
                .AllowCredentials()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field. Example: 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
      {
        new OpenApiSecurityScheme{
          Reference = new OpenApiReference{
            Type=ReferenceType.SecurityScheme,
            Id="Bearer"
          }
        },
        new string[]{}
      }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("amsfrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
