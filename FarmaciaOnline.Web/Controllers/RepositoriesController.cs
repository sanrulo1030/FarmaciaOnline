using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FarmaciaOnline.Web.Data;
using FarmaciaOnline.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace FarmaciaOnline.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RepositoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepositoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repositories
        public async Task<IActionResult> Index()
        {
              return View(await _context.Repositories
                  .Include(c => c.Medicines)
                  .ToListAsync());
        }

        // GET: Repositories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Repositories == null)
            {
                return NotFound();
            }

            Repository repository = await _context.Repositories
                 .Include(c => c.Medicines)
                 .ThenInclude(d => d.Laboratories)
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (repository == null)
            {
                return NotFound();
            }

            return View(repository);
        }

        // GET: Repositories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Repositories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Repository repository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(repository);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "hay un registro con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                       dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(repository);
        }

        // GET: Repositories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Repositories == null)
            {
                return NotFound();
            }

            var repository = await _context.Repositories.FindAsync(id);
            if (repository == null)
            {
                return NotFound();
            }
            return View(repository);
        }

        // POST: Repositories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Repository repository)
        {
            if (id != repository.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repository);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if
                   (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "hay un registro con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                       dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(repository);
        }


        // GET: Repositories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Repository repository = await _context.Repositories
                .Include(c => c.Medicines)
                .ThenInclude(d => d.Laboratories)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (repository == null)
            {
                return NotFound();
            }
            _context.Repositories.Remove(repository);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //AddMedicine AddMedicine

        public async Task<IActionResult> AddMedicine(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Repository repository = await _context.Repositories.FindAsync(id);
            if (repository == null)
            {
                return NotFound();
            }
            Medicine model = new Medicine { IdRepository = repository.Id };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicine(Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                Repository repository = await _context.Repositories
                .Include(c => c.Medicines)
                .FirstOrDefaultAsync(c => c.Id == medicine.IdRepository);
                if (repository == null)
                {
                    return NotFound();
                }
                try
                {
                    medicine.Id = 0;
                    repository.Medicines.Add(medicine);
                    _context.Update(repository);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Details), new
                    {
                        Id = repository.Id
                    });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if
                   (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                       dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(medicine);
        }

        public async Task<IActionResult> EditMedicine(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Medicine medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            Repository repository = await _context.Repositories.FirstOrDefaultAsync(c => c.Medicines.FirstOrDefault(d => d.Id == medicine.Id) != null);
            medicine.IdRepository = repository.Id;
            return View(medicine);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedicine(Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new
                    {
                        Id = medicine.IdRepository
                    });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                       dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(medicine);
        }

        public async Task<IActionResult> DeleteMedicine(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Medicine medicine = await _context.Medicines
            .Include(d => d.Laboratories)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }
            Repository repository = await _context.Repositories.FirstOrDefaultAsync(c =>
           c.Medicines.FirstOrDefault(d => d.Id == medicine.Id) != null);
            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { Id = repository.Id });
        }

        public async Task<IActionResult> DetailsMedicine(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Medicine medicine = await _context.Medicines
            .Include(d => d.Laboratories)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }
            Repository repository = await _context.Repositories.FirstOrDefaultAsync(c =>
           c.Medicines.FirstOrDefault(d => d.Id == medicine.Id) != null);
            medicine.IdRepository = repository.Id;
            return View(medicine);
        }

        public async Task<IActionResult> AddLaboratory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Medicine medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            Laboratory model = new Laboratory { IdMedicine = medicine.Id };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLaboratory(Laboratory laboratory)
        {
            if (ModelState.IsValid)
            {
                Medicine medicine = await _context.Medicines
                .Include(d => d.Laboratories)
                .FirstOrDefaultAsync(c => c.Id == laboratory.IdMedicine);
                if (medicine == null)
                {
                    return NotFound();
                }
                try
                {
                    laboratory.Id = 0;
                    medicine.Laboratories.Add(laboratory);
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsMedicine), new { Id = medicine.Id });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                       dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(laboratory);
        }

        public async Task<IActionResult> EditLaboratory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Laboratory laboratory = await _context.Laboratories.FindAsync(id);
            if (laboratory == null)
            {
                return NotFound();
            }
            Medicine medicine = await _context.Medicines.FirstOrDefaultAsync(d =>
           d.Laboratories.FirstOrDefault(c => c.Id == laboratory.Id) != null);
            laboratory.IdMedicine = medicine.Id;
            return View(laboratory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLaboratory(Laboratory laboratory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laboratory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsMedicine), new { Id = laboratory.IdMedicine});
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                       dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(laboratory);
        }

        public async Task<IActionResult> DeleteLaboratory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Laboratory laboratory = await _context.Laboratories
            .FirstOrDefaultAsync(m => m.Id == id);
            if (laboratory == null)
            {
                return NotFound();
            }
            Medicine medicine = await _context.Medicines.FirstOrDefaultAsync(d
           => d.Laboratories.FirstOrDefault(c => c.Id == laboratory.Id) != null);
            _context.Laboratories.Remove(laboratory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(DetailsMedicine), new
            {
                Id = medicine.Id
            });
        }






        // POST: Repositories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Repositories == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Repositories'  is null.");
        //    }
        //    var repository = await _context.Repositories.FindAsync(id);
        //    if (repository != null)
        //    {
        //        _context.Repositories.Remove(repository);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


    }
}
