using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core.Data;
using QMS.Core.Models;

namespace QMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/Documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetDocuments([FromQuery] string? search = null, [FromQuery] int? menuId = null)
        {
            var query = from u in _context.TbUploads
                        join m in _context.TbMenus on u.MenuId equals m.MenuId into menuJoin
                        from m in menuJoin.DefaultIfEmpty()
                        select new
                        {
                            u.UploadId,
                            u.MenuId,
                            u.Creator,
                            u.FileName,
                            u.FileDir,
                            u.Size,
                            u.Type,
                            Category = m != null ? m.Menu : "Dokumen Mutu",
                            Submenu = m != null ? m.Submenu : "Umum"
                        };

            if (menuId.HasValue && menuId.Value > 0)
            {
                query = query.Where(d => d.MenuId == menuId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(d => d.FileName.ToLower().Contains(s) || 
                                         d.Creator.ToLower().Contains(s) || 
                                         d.Category.ToLower().Contains(s) || 
                                         d.Submenu.ToLower().Contains(s));
            }

            var list = await query.OrderByDescending(d => d.UploadId).ToListAsync();
            return Ok(list);
        }

        // 2. GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetDocument(int id)
        {
            var doc = await (from u in _context.TbUploads
                             where u.UploadId == id
                             join m in _context.TbMenus on u.MenuId equals m.MenuId into menuJoin
                             from m in menuJoin.DefaultIfEmpty()
                             select new
                             {
                                 u.UploadId,
                                 u.MenuId,
                                 u.Creator,
                                 u.FileName,
                                 u.FileDir,
                                 u.Size,
                                 u.Type,
                                 Category = m != null ? m.Menu : "Dokumen Mutu",
                                 Submenu = m != null ? m.Submenu : "Umum"
                             }).FirstOrDefaultAsync();

            if (doc == null)
            {
                return NotFound(new { message = $"Dokumen dengan ID {id} tidak ditemukan." });
            }

            return Ok(doc);
        }

        // 3. POST: api/Documents (Tambah Dokumen)
        [HttpPost]
        public async Task<ActionResult<TbUpload>> PostDocument([FromBody] TbUpload document)
        {
            if (string.IsNullOrWhiteSpace(document.FileName))
            {
                return BadRequest(new { message = "Nama file dokumen wajib diisi." });
            }

            if (string.IsNullOrWhiteSpace(document.Creator))
            {
                document.Creator = "admin";
            }

            if (string.IsNullOrWhiteSpace(document.FileDir))
            {
                document.FileDir = $"../document/{document.FileName}";
            }

            if (string.IsNullOrWhiteSpace(document.Type))
            {
                document.Type = document.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) 
                    ? "application/pdf" 
                    : "application/octet-stream";
            }

            if (document.UploadId == 0)
            {
                var maxId = await _context.TbUploads.AnyAsync() 
                    ? await _context.TbUploads.MaxAsync(u => u.UploadId) 
                    : 0;
                document.UploadId = maxId + 1;
            }

            _context.TbUploads.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocument), new { id = document.UploadId }, document);
        }

        // 4. PUT: api/Documents/5 (Edit Dokumen)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument(int id, [FromBody] TbUpload document)
        {
            if (id != document.UploadId)
            {
                return BadRequest(new { message = "ID URL tidak cocok dengan ID Dokumen." });
            }

            var existing = await _context.TbUploads.FindAsync(id);
            if (existing == null)
            {
                return NotFound(new { message = $"Dokumen dengan ID {id} tidak ditemukan." });
            }

            existing.FileName = document.FileName;
            existing.MenuId = document.MenuId;
            existing.Creator = string.IsNullOrWhiteSpace(document.Creator) ? existing.Creator : document.Creator;
            existing.Size = string.IsNullOrWhiteSpace(document.Size) ? existing.Size : document.Size;
            existing.Type = string.IsNullOrWhiteSpace(document.Type) ? existing.Type : document.Type;
            if (!string.IsNullOrWhiteSpace(document.FileDir))
            {
                existing.FileDir = document.FileDir;
            }

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // 5. DELETE: api/Documents/5 (Hapus Dokumen)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _context.TbUploads.FindAsync(id);
            if (document == null)
            {
                return NotFound(new { message = $"Dokumen dengan ID {id} tidak ditemukan." });
            }

            _context.TbUploads.Remove(document);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Dokumen {document.FileName} berhasil dihapus." });
        }
    }
}
