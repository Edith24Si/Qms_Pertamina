using System.Net.Http.Json;
using QMS.Core.Models;

namespace QMS.Wasm.Services;

public class SasaranMutuService : ISasaranMutuService
{
    private readonly HttpClient _http;

    public SasaranMutuService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<KpiModel>> GetKpisAsync() =>
        await _http.GetFromJsonAsync<List<KpiModel>>("api/sasaran-mutu/kpi") ?? new();

    public async Task<bool> AddKpiAsync(KpiModel kpi) =>
        (await _http.PostAsJsonAsync("api/sasaran-mutu/kpi", kpi)).IsSuccessStatusCode;

    public async Task<bool> UpdateKpiAsync(KpiModel kpi) =>
        (await _http.PutAsJsonAsync($"api/sasaran-mutu/kpi/{kpi.Id}", kpi)).IsSuccessStatusCode;

    public async Task<bool> DeleteKpiAsync(int id) =>
        (await _http.DeleteAsync($"api/sasaran-mutu/kpi/{id}")).IsSuccessStatusCode;

    public async Task<List<GugusMutuModel>> GetGugusListAsync() =>
        await _http.GetFromJsonAsync<List<GugusMutuModel>>("api/sasaran-mutu/gugus") ?? new();

    public async Task<bool> AddGugusAsync(GugusMutuModel gugus) =>
        (await _http.PostAsJsonAsync("api/sasaran-mutu/gugus", gugus)).IsSuccessStatusCode;

    public async Task<bool> UpdateGugusAsync(GugusMutuModel gugus) =>
        (await _http.PutAsJsonAsync($"api/sasaran-mutu/gugus/{gugus.Id}", gugus)).IsSuccessStatusCode;

    public async Task<bool> DeleteGugusAsync(int id) =>
        (await _http.DeleteAsync($"api/sasaran-mutu/gugus/{id}")).IsSuccessStatusCode;
}