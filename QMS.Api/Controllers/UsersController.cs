using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core.Data;
using QMS.Core.Models;
using System.Text.RegularExpressions;

namespace QMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/Users (Mendapatkan daftar orang yang sudah terdaftar)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TbRegisteredUser>>> GetUsers([FromQuery] string? search = null)
        {
            var query = _context.RegisteredUsers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(s) ||
                                         u.Username.ToLower().Contains(s) ||
                                         u.Email.ToLower().Contains(s) ||
                                         u.NoPeg.ToString().Contains(s) ||
                                         (u.Jabatan != null && u.Jabatan.ToLower().Contains(s)) ||
                                         (u.Fungsi != null && u.Fungsi.ToLower().Contains(s)));
            }

            var list = await query.OrderByDescending(u => u.Id).ToListAsync();
            return Ok(list);
        }

        // 2. GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TbRegisteredUser>> GetUser(int id)
        {
            var user = await _context.RegisteredUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User dengan ID {id} tidak ditemukan." });
            }

            return Ok(user);
        }

        // 3. POST: api/Users/register (Registrasi khusus Pegawai via akun Gmail)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest(new { message = "Nama Lengkap wajib diisi." });
            }

            if (request.NoPeg <= 0)
            {
                return BadRequest(new { message = "Nomor Pekerja (NIP) wajib diisi dengan benar." });
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { message = "Alamat Email wajib diisi." });
            }

            // Validasi: Wajib menggunakan Gmail
            var emailClean = request.Email.Trim().ToLower();
            if (!emailClean.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Registrasi harus menggunakan akun Gmail yang aktif (@gmail.com)." });
            }

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 4)
            {
                return BadRequest(new { message = "Kata sandi minimal 4 karakter." });
            }

            // Cek apakah email atau NoPeg sudah terdaftar di RegisteredUsers
            var existingEmail = await _context.RegisteredUsers.AnyAsync(u => u.Email.ToLower() == emailClean);
            if (existingEmail)
            {
                return BadRequest(new { message = $"Email '{request.Email}' sudah terdaftar. Silakan login atau gunakan email lain." });
            }

            var existingNoPeg = await _context.RegisteredUsers.AnyAsync(u => u.NoPeg == request.NoPeg);
            if (existingNoPeg)
            {
                return BadRequest(new { message = $"Nomor Pekerja (NIP) '{request.NoPeg}' sudah terdaftar." });
            }

            // Generate username jika belum ada
            var username = !string.IsNullOrWhiteSpace(request.Username) 
                ? request.Username.Trim().ToLower() 
                : emailClean.Replace("@gmail.com", "");

            var newUser = new TbRegisteredUser
            {
                FullName = request.FullName.Trim(),
                Username = username,
                Email = emailClean,
                NoPeg = request.NoPeg,
                Jabatan = string.IsNullOrWhiteSpace(request.Jabatan) ? "Pekerja Mutu" : request.Jabatan.Trim(),
                Fungsi = string.IsNullOrWhiteSpace(request.Fungsi) ? "Refinery Unit II Dumai" : request.Fungsi.Trim(),
                Phone = request.Phone?.Trim() ?? "-",
                Password = request.Password,
                Role = "Pegawai", // Khusus register otomatis sebagai pegawai
                RegisteredAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                PhotoUrl = request.PhotoUrl ?? ""
            };

            _context.RegisteredUsers.Add(newUser);

            // Simpan / sinkronisasi ke tabel tb_employee jika belum ada
            var empExists = await _context.TbEmployees.AnyAsync(e => e.NoPeg == request.NoPeg);
            if (!empExists)
            {
                _context.TbEmployees.Add(new TbEmployee
                {
                    NoPeg = request.NoPeg,
                    EmpName = newUser.FullName,
                    Email = newUser.Email,
                    Jabatan = newUser.Jabatan,
                    Fungsi = newUser.Fungsi,
                    Telpon = newUser.Phone,
                    Password = newUser.Password,
                    ResetPswd = newUser.Password
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                success = true, 
                message = "Registrasi pegawai berhasil! Anda dapat langsung login.", 
                user = newUser 
            });
        }

        // 4. POST: api/Users/login (Verifikasi Login Akun Pegawai atau Administrator)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var userInput = request.UsernameOrEmail.Trim().ToLower();
            var passInput = request.Password.Trim();

            // Cek di RegisteredUsers (bisa pakai username, email gmail, atau NoPeg)
            var user = await _context.RegisteredUsers
                .FirstOrDefaultAsync(u => (u.Username.ToLower() == userInput || 
                                           u.Email.ToLower() == userInput || 
                                           u.NoPeg.ToString() == userInput) && 
                                           u.Password == passInput);

            if (user != null)
            {
                return Ok(new
                {
                    success = true,
                    role = "User",
                    fullName = user.FullName,
                    email = user.Email,
                    noPeg = user.NoPeg,
                    photoUrl = user.PhotoUrl
                });
            }

            // Cek Administrator khusus
            if (userInput == "adminqms" || userInput == "admin")
            {
                if (passInput == "admin")
                {
                    return Ok(new
                    {
                        success = true,
                        role = "Administrator",
                        fullName = "Administrator QMS",
                        email = "admin.qms@pertamina.com",
                        noPeg = 700001,
                        photoUrl = ""
                    });
                }
            }

            return Unauthorized(new { success = false, message = "Username, NIP, atau Kata Sandi salah." });
        }

        // 5. PUT: api/Users/5 (Update profil / foto profil)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] TbRegisteredUser updated)
        {
            var user = await _context.RegisteredUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User dengan ID {id} tidak ditemukan." });
            }

            user.FullName = string.IsNullOrWhiteSpace(updated.FullName) ? user.FullName : updated.FullName;
            user.Jabatan = updated.Jabatan ?? user.Jabatan;
            user.Fungsi = updated.Fungsi ?? user.Fungsi;
            user.Phone = updated.Phone ?? user.Phone;
            if (updated.PhotoUrl != null)
            {
                user.PhotoUrl = updated.PhotoUrl;
            }

            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // 6. POST: api/Users/update-photo (Update / Hapus foto profil spesifik per email)
        [HttpPost("update-photo")]
        public async Task<IActionResult> UpdatePhoto([FromBody] UpdatePhotoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { message = "Email tidak boleh kosong." });
            }

            var emailClean = request.Email.Trim().ToLower();
            var user = await _context.RegisteredUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == emailClean);
            if (user != null)
            {
                user.PhotoUrl = request.PhotoUrl ?? "";
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Foto profil berhasil diperbarui di database.", photoUrl = user.PhotoUrl });
            }

            return Ok(new { success = true, message = "Foto disimpan secara lokal.", photoUrl = request.PhotoUrl });
        }

        // 7. DELETE: api/Users/5 (Hapus akun - Admin only)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.RegisteredUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User dengan ID {id} tidak ditemukan." });
            }

            _context.RegisteredUsers.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"User '{user.FullName}' berhasil dihapus." });
        }
    }

    public class UpdatePhotoRequest
    {
        public string Email { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
    }

    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string Email { get; set; } = string.Empty;
        public int NoPeg { get; set; }
        public string? Jabatan { get; set; }
        public string? Fungsi { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
    }

    public class LoginRequest
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
