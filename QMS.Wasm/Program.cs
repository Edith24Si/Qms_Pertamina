using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QMS.Services;
using QMS.Wasm;
using QMS.Wasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<QmsApiService>();
builder.Services.AddScoped<UserState>();

// Base Address dari QMS.Api (Port http://localhost:5260 untuk akses cepat tanpa kendala sertifikat SSL di Mac)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5260/") });
builder.Services.AddScoped<ISasaranMutuService, SasaranMutuService>();

await builder.Build().RunAsync();
