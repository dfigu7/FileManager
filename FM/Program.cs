using BLL;
using BLL.Services;
using DataAccess;
using FMAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Register the configuration section for StorageSettings
builder.Services.Configure<StorageSettings>(builder.Configuration.GetSection("StorageSettings"));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

// Register DbContext with the connection string
builder.Services.AddDbContext<FileManagerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register unit of work and services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IFileService, FileService>();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();