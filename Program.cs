using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Data;

var builder = WebApplication.CreateBuilder(args);

// Safely retrieve the connection string from any location Azure might store it
var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") 
    ?? builder.Configuration.GetConnectionString("MyDbConnection") 
    ?? Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");

// Add database context using the resolved string
builder.Services.AddDbContext<MyDatabaseContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add App Service logging
builder.Logging.AddAzureWebAppDiagnostics();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todos}/{action=Index}/{id?}");

app.Run();
