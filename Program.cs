using Microsoft.EntityFrameworkCore;
using ProductApp.Data;
using ProductApp.Security; // <= importa o helper
using MySqlConnector;      // opcional, só pra usar o builder de connection string

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// Recupera dados básicos de conexão (sem senha)
var dbSection = builder.Configuration.GetSection("Database");
var dbServer  = dbSection["Server"];
var dbPort    = int.Parse(dbSection["Port"] ?? "3306");
var dbName    = dbSection["Name"];
var dbUser    = dbSection["User"];

// Busca a senha no Credential Provider (CyberArk)
var dbPassword = CyberArkPasswordProvider.GetDatabasePassword(builder.Configuration);

// Monta connection string de forma segura
var csBuilder = new MySqlConnectionStringBuilder
{
    Server   = dbServer,
    Port     = (uint)dbPort,
    Database = dbName,
    UserID   = dbUser,
    Password = dbPassword,
    SslMode  = MySqlSslMode.None
};

var connectionString = csBuilder.ConnectionString;

// Configura EF Core com MariaDB via Pomelo
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

var app = builder.Build();

// Pipeline padrão
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();

