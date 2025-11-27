using Microsoft.EntityFrameworkCore;
using BibliotecaUniversitaria.Infrastructure.Data;
using BibliotecaUniversitaria.Infrastructure.Repositories;
using BibliotecaUniversitaria.Infrastructure.Factories;
using BibliotecaUniversitaria.Application.Interfaces;
using BibliotecaUniversitaria.Application.Services;
using BibliotecaUniversitaria.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Mapster
MapsterConfig.RegisterMappings();

builder.Services.AddScoped<IDatabaseFactory, DatabaseFactory>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
