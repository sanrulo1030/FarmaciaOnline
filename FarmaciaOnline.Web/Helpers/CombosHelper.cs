using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using FarmaciaOnline.Web.Data;
using FarmaciaOnline.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmaciaOnline.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly ApplicationDbContext _context;

        public CombosHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboCategories()
        {
            List<SelectListItem> list = _context.Categories.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = $"{t.Id}"
            })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a category...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboLaboratories(int medicineId)

        {

            List<SelectListItem> list = new List<SelectListItem>();

            Medicine medicine = _context.Medicines

                .Include(d => d.Laboratories)

                .FirstOrDefault(d => d.Id == medicineId);

            if (medicine != null)

            {

                list = medicine.Laboratories.Select(t => new SelectListItem

                {

                    Text = t.Name,

                    Value = $"{t.Id}"

                })

                    .OrderBy(t => t.Text)

                    .ToList();

            }



            list.Insert(0, new SelectListItem

            {

                Text = "[Select a laboratory...]",

                Value = "0"

            });



            return list;

        }



        public IEnumerable<SelectListItem> GetComboRepositories()

        {

            List<SelectListItem> list = _context.Repositories.Select(t => new SelectListItem

            {

                Text = t.Name,

                Value = $"{t.Id}"

            })

                .OrderBy(t => t.Text)

                .ToList();



            list.Insert(0, new SelectListItem

            {

                Text = "[Select a repository...]",

                Value = "0"

            });



            return list;

        }



        public IEnumerable<SelectListItem> GetComboMedicines(int repositoryId)

        {

            List<SelectListItem> list = new List<SelectListItem>();

            Repository repository = _context.Repositories

                .Include(r => r.Medicines)

                .FirstOrDefault(c => c.Id == repositoryId);

            if (repository != null)

            {

                list = repository.Medicines.Select(t => new SelectListItem

                {

                    Text = t.Name,

                    Value = $"{t.Id}"

                })

                    .OrderBy(t => t.Text)

                    .ToList();

            }



            list.Insert(0, new SelectListItem

            {

                Text = "[Select a medicine...]",

                Value = "0"

            });



            return list;

        }
    }

}
