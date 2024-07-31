using BLL;
using BLL.Services;
using DataAccess;
using FMAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IFileItemService, FileItemService>();
builder.Services.AddScoped<IViewService, ViewService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFolderService, FolderService>();
builder.Services.AddScoped<IFileItemService, FileItemService>();
builder.Services.AddScoped<IFileItemRepository, FileItemRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();


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

app.UseHttpsRedirection();

app.MapControllers();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.Run();