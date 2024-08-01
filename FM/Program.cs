using System.Text;
using BLL;
using BLL.Services;
using DataAccess;
using DataAccess.Services;
using FMAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Register the configuration section for StorageSettings
builder.Services.Configure<StorageSettings>(builder.Configuration.GetSection("StorageSettings"));

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Configure JWT Bearer authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var value = builder.Configuration.GetSection("Jwt:Key").Value;
        if (value != null)
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
    });
builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:5273").AllowAnyMethod().AllowAnyHeader();
    }));




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IFileItemService, FileItemService>();
builder.Services.AddScoped<IViewService, ViewService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IFileItemService, FileItemService>();
builder.Services.AddScoped<IFileItemRepository, FileItemRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();

// Register DbContext with the connection string
builder.Services.AddDbContext<FileManagerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "File Manager API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("NgOrigins");
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.Run();
