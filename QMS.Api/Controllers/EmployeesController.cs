using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core.Data;
using QMS.Core.Models;

namespace QMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TbEmployee>>> GetEmployees([FromQuery] string? search = null, [FromQuery] int limit = 100)
        {
            var query = _context.TbEmployees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(e => e.EmpName.ToLower().Contains(s) || 
                                         e.NoPeg.ToString().Contains(s) || 
                                         (e.Email != null && e.Email.ToLower().Contains(s)) ||
                                         (e.Fungsi != null && e.Fungsi.ToLower().Contains(s)) ||
                                         (e.Jabatan != null && e.Jabatan.ToLower().Contains(s)));
            }

            var list = await query.OrderBy(e => e.EmpName).Take(limit).ToListAsync();
            return Ok(list);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TbEmployee>> GetEmployee(int id)
        {
            var emp = await _context.TbEmployees.FindAsync(id);
            if (emp == null)
            {
                return NotFound(new { message = $"Pegawai dengan No. Pekerja {id} tidak ditemukan." });
            }
            return Ok(emp);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<TbEmployee>> PostEmployee([FromBody] TbEmployee employee)
        {
            if (employee.NoPeg <= 0)
            {
                var maxNo = await _context.TbEmployees.AnyAsync() 
                    ? await _context.TbEmployees.MaxAsync(e => e.NoPeg) 
                    : 1000;
                employee.NoPeg = maxNo + 1;
            }

            if (await _context.TbEmployees.AnyAsync(e => e.NoPeg == employee.NoPeg))
            {
                return Conflict(new { message = $"No. Pekerja {employee.NoPeg} sudah terdaftar." });
            }

            if (string.IsNullOrWhiteSpace(employee.Password))
            {
                employee.Password = "user123";
                employee.ResetPswd = "user123";
            }

            _context.TbEmployees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.NoPeg }, employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromBody] TbEmployee employee)
        {
            if (id != employee.NoPeg)
            {
                return BadRequest(new { message = "ID URL tidak cocok dengan No. Pekerja." });
            }

            var existing = await _context.TbEmployees.FindAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = $"Pegawai dengan No. Pekerja {id} tidak ditemukan." });
            }

            existing.EmpName = employee.EmpName;
            existing.Email = employee.Email;
            existing.Fungsi = employee.Fungsi;
            existing.Jabatan = employee.Jabatan;
            existing.Telpon = employee.Telpon;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var emp = await _context.TbEmployees.FindAsync(id);
            if (emp == null)
            {
                return NotFound(new { message = $"Pegawai dengan No. Pekerja {id} tidak ditemukan." });
            }

            _context.TbEmployees.Remove(emp);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Pegawai {emp.EmpName} berhasil dihapus." });
        }
    }
}
