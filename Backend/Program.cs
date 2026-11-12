using Microsoft.EntityFrameworkCore;
using QMS.Core.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Konfigurasi Database (Microsoft SQL Server Default & SQLite Fallback)
var sqlServerConnection = builder.Configuration.GetConnectionString("SqlServerConnection") 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Server=(localdb)\\MSSQLLocalDB;Database=db_qms;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

var sqliteConnection = builder.Configuration.GetConnectionString("SqliteConnection") 
                    ?? "Data Source=qms.db";

var provider = builder.Configuration["DatabaseProvider"] ?? "SqlServer";

// Jalur absolut SQLite jika fallback diperlukan
string finalSqlitePath = Path.Combine(builder.Environment.ContentRootPath, "qms.db");
if (!File.Exists(finalSqlitePath))
{
    var rootDb = Path.Combine(Directory.GetCurrentDirectory(), "qms.db");
    if (File.Exists(rootDb)) finalSqlitePath = rootDb;
    else
    {
        var parentDb = Path.Combine(Directory.GetCurrentDirectory(), "..", "qms.db");
        if (File.Exists(parentDb)) finalSqlitePath = parentDb;
    }
}

// Cek apakah SQL Server dapat dihubungi
bool useSqlServer = provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase) && 
                    !string.IsNullOrWhiteSpace(sqlServerConnection) && 
                    sqlServerConnection.Contains("Server=");

if (useSqlServer)
{
    try
    {
        // Tes koneksi cepat (timeout 2 detik)
        var builderConn = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(sqlServerConnection)
        {
            ConnectTimeout = 2
        };
        using var testConn = new Microsoft.Data.SqlClient.SqlConnection(builderConn.ConnectionString);
        testConn.Open();
        testConn.Close();
        Console.WriteLine("----------------------------------------------------------------------");
        Console.WriteLine("✅ Terhubung ke Microsoft SQL Server: " + builderConn.DataSource);
        Console.WriteLine("----------------------------------------------------------------------");
    }
    catch
    {
        useSqlServer = false;
        Console.WriteLine("----------------------------------------------------------------------");
        Console.WriteLine("ℹ️  Microsoft SQL Server tidak terdeteksi aktif pada host saat ini.");
        Console.WriteLine("🔄 Menggunakan database SQLite (qms.db) sebagai fallback agar API tetap aktif.");
        Console.WriteLine("💡 Saat dibuka di Windows/Visual Studio dengan SQL Server aktif, API otomatis beralih ke SQL Server.");
        Console.WriteLine("----------------------------------------------------------------------");
    }
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (useSqlServer)
    {
        options.UseSqlServer(sqlServerConnection);
    }
    else
    {
        options.UseSqlite($"Data Source={finalSqlitePath}");
    }
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Mengizinkan CORS untuk Blazor WASM Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Otomatis inisialisasi schema database jika belum ada
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Inisialisasi database: " + ex.Message);
    }
}

// Aktifkan Swagger UI (selalu aktif untuk dokumentasi REST API)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QMS Pertamina REST API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "QMS Pertamina - REST API Documentation";
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseCors("AllowBlazorApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
