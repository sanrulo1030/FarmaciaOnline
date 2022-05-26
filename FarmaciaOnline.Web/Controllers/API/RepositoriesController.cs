using FarmaciaOnline.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmaciaOnline.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RepositoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetRepositories()
        {
            return Ok(_context.Repositories
                .Include(c => c.Medicines)
                .ThenInclude(d => d.Laboratories));
        }
    }
}
