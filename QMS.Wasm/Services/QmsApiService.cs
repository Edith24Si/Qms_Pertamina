using System.Net.Http.Json;
using QMS.Core.Models;

namespace QMS.Wasm.Services
{
    public class QmsApiService
    {
        private readonly HttpClient _http;

        public QmsApiService(HttpClient http)
        {
            _http = http;
        }

        // ==========================================
        // DOKUMEN MUTU (TB_UPLOAD + TB_MENU)
        // ==========================================
        public async Task<List<DocumentDto>> GetDocumentsAsync(string? search = null, int? menuId = null)
        {
            try
            {
                var url = "api/Documents";
                var queryParams = new List<string>();
                if (!string.IsNullOrWhiteSpace(search))
                    queryParams.Add($"search={Uri.EscapeDataString(search)}");
                if (menuId.HasValue && menuId.Value > 0)
                    queryParams.Add($"menuId={menuId.Value}");

                if (queryParams.Any())
                    url += "?" + string.Join("&", queryParams);

                var result = await _http.GetFromJsonAsync<List<DocumentDto>>(url);
                return result ?? new List<DocumentDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching documents: {ex.Message}");
                return new List<DocumentDto>();
            }
        }

        public async Task<DocumentDto?> GetDocumentByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<DocumentDto>($"api/Documents/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateDocumentAsync(TbUpload document)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Documents", document);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateDocumentAsync(int id, TbUpload document)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/Documents/{id}", document);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteDocumentAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/Documents/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ==========================================
        // PEGAWAI & PEKERJA (TB_EMPLOYEE)
        // ==========================================
        public async Task<List<TbEmployee>> GetEmployeesAsync(string? search = null, int limit = 100)
        {
            try
            {
                var url = $"api/Employees?limit={limit}";
                if (!string.IsNullOrWhiteSpace(search))
                    url += $"&search={Uri.EscapeDataString(search)}";

                var result = await _http.GetFromJsonAsync<List<TbEmployee>>(url);
                return result ?? new List<TbEmployee>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching employees: {ex.Message}");
                return new List<TbEmployee>();
            }
        }

        public async Task<TbEmployee?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<TbEmployee>($"api/Employees/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateEmployeeAsync(TbEmployee emp)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Employees", emp);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(int id, TbEmployee emp)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/Employees/{id}", emp);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/Employees/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ==========================================
        // MASTER MENUS
        // ==========================================
        public async Task<List<TbMenu>> GetMenusAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<TbMenu>>("api/Menus");
                return result ?? new List<TbMenu>();
            }
            catch
            {
                return new List<TbMenu>();
            }
        }

        // ==========================================
        // USERS & REGISTRASI PEGAWAI (TB_REGISTERED_USER)
        // ==========================================
        public async Task<List<TbRegisteredUser>> GetUsersAsync(string? search = null)
        {
            try
            {
                var url = "api/Users";
                if (!string.IsNullOrWhiteSpace(search))
                    url += $"?search={Uri.EscapeDataString(search)}";

                var result = await _http.GetFromJsonAsync<List<TbRegisteredUser>>(url);
                return result ?? new List<TbRegisteredUser>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return new List<TbRegisteredUser>();
            }
        }

        public async Task<TbRegisteredUser?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<TbRegisteredUser>($"api/Users/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool success, string message, TbRegisteredUser? user)> RegisterAsync(object request)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Users/register", request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RegisterApiResponse>();
                    return (true, result?.Message ?? "Registrasi berhasil!", result?.User);
                }
                else
                {
                    var err = await response.Content.ReadFromJsonAsync<ErrorApiResponse>();
                    return (false, err?.Message ?? "Gagal melakukan registrasi.", null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error koneksi API: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string role, string fullName, string email, int noPeg, string? photoUrl)> LoginAsync(string usernameOrEmail, string password)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Users/login", new { UsernameOrEmail = usernameOrEmail, Password = password });
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginApiResponse>();
                    if (result != null && result.Success)
                    {
                        return (true, result.Role, result.FullName, result.Email, result.NoPeg, result.PhotoUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login API error: {ex.Message}");
            }

            return (false, "User", "", "", 0, null);
        }

        public async Task<bool> UpdateUserAsync(int id, TbRegisteredUser user)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"api/Users/{id}", user);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/Users/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserPhotoAsync(string email, string photoUrl)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Users/update-photo", new { Email = email, PhotoUrl = photoUrl });
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user photo: {ex.Message}");
                return false;
            }
        }
    }

    public class RegisterApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public TbRegisteredUser? User { get; set; }
    }

    public class LoginApiResponse
    {
        public bool Success { get; set; }
        public string Role { get; set; } = "User";
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int NoPeg { get; set; }
        public string? PhotoUrl { get; set; }
    }

    public class ErrorApiResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}
