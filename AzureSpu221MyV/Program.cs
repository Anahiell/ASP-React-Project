using ASP_SPU221_HMW.Services.Upload;
using AzureSpu221MyV.Middleware;
using AzureSpu221MyV.Services.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("azuresettings.json");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IContainerProvider, CosmosDbContainerProvider>();
builder.Services.AddSingleton<IUploadService, UploadServiceV1>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

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
app.UseSession();
app.UseMiddleware<SessionAuthMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
