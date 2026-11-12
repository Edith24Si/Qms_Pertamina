using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMS.Core.Data;
using QMS.Core.Models;

namespace QMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenusController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenusController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TbMenu>>> GetMenus()
        {
            var menus = await _context.TbMenus.OrderBy(m => m.Menu).ThenBy(m => m.Submenu).ToListAsync();
            return Ok(menus);
        }
    }
}
