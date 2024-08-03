using ExpenseTracker.Components;
using ExpenseTracker.Data;
using ExpenseTracker.Repositories;
using ExpenseTracker.Services;
using ExpenseTracker.Utils;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "http://localhost:5100") });
builder.Services.AddSqlite<ExpenseContext>("Data Source=expenses.db");
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IExpenseTagService, ExpenseTagService>();
builder.Services.AddScoped<IExpenseAnalyticsService, ExpenseAnalyticsService>();
builder.Services.AddScoped<IClock, Clock>();
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();


builder.Services.AddMvc(opt =>
{
    opt.EnableEndpointRouting = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseMvcWithDefaultRoute();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ExpenseContext>();
}

app.Run();
