using backlogged_api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using backlogged_api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add PostgreSQL support
builder.Services.AddDbContext<BackloggedDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add ASP.NET Identity support
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<BackloggedDBContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Backlogged API",
        Description = "An ASP.NET Core Web API for Backlogged. You can find the repository at [https://github.com/tharropoulos/backlogged-api](here)",
        Contact = new OpenApiContact
        {
            Name = "Fanis Tharropoulos",
            Email = "ftharropoulos@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "Apache License",
            Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0")
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
var port = Environment.GetEnvironmentVariable("PORT") ?? "8081";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
