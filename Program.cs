using mini_project_csharp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configuração da autenticação por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout"; 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
    });

// Configuração da base de dados (MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

builder.Services.AddControllersWithViews(); // Ativa suporte para controladores e views

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts(); 
}

app.UseHttpsRedirection(); // Redireciona para HTTPS
app.UseStaticFiles(); // Permite acesso a arquivos estáticos
app.UseRouting();
app.UseAuthentication(); // Middleware de autenticação
app.UseAuthorization(); // Middleware de autorização

// Define a rota padrão da aplicação
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Inicia a aplicação
