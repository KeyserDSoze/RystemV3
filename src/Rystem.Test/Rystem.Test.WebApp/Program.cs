using Rystem.Test.WebApp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServiceLocator();
builder.Services.AddScoped<RandomService>();
builder.Services.AddBuildCallback(serviceLocator =>
{
    return Task.FromResult(0);
});
builder.Services.AddBuildCallback(serviceLocator =>
{
    var service = serviceLocator.GetService<RandomService>();
    return Task.FromResult(0);
});
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
await app.RunCallbacksAfterBuild();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
