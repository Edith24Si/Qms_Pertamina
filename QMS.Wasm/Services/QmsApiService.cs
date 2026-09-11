using System.Net.Http.Json;
using QMS.Core.Models;

namespace QMS.Wasm.Services;

public class QmsApiService
{
    private readonly HttpClient _http;

    public QmsApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<QualityDocument>?> GetDocumentsAsync()
    {
        return await _http.GetFromJsonAsync<List<QualityDocument>>("api/QualityDocuments");
    }

    public async Task<bool> CreateDocumentAsync(QualityDocument document)
    {
        var response = await _http.PostAsJsonAsync("api/QualityDocuments", document);
        return response.IsSuccessStatusCode;
    }
}