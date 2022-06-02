using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using FarmaciaOnline.Web.Helpers;
using FarmaciaOnline.Web.Models;
using Microsoft.AspNetCore.Authorization;
using FarmaciaOnline.Web.Data;
using Microsoft.EntityFrameworkCore;
using System;
using FarmaciaOnline.Web.Data.Entities;
using FarmaciaOnline.Web.Enums;

namespace FarmaciaOnline.Web.Controllers
{

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;

        public AccountController(ApplicationDbContext context,IUserHelper userHelper,
        ICombosHelper combosHelper,IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email or password incorrect.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Register()

        {

            AddUserViewModel model = new AddUserViewModel

            {

                Repositories = _combosHelper.GetComboRepositories(),

                Medicines = _combosHelper.GetComboMedicines(0),

                Laboratories = _combosHelper.GetComboLaboratories(0),

            };



            return View(model);

        }



        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(AddUserViewModel model)

        {

            if (ModelState.IsValid)

            {

                Guid imageId = Guid.Empty;



                if (model.ImageFile != null)

                {

                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");

                }



                User user = await _userHelper.AddUserAsync(model, imageId, UserType.User);

                if (user == null)

                {

                    ModelState.AddModelError(string.Empty, "This email is already used.");

                    model.Repositories = _combosHelper.GetComboRepositories();

                    model.Medicines = _combosHelper.GetComboMedicines(model.RepositoryId);

                    model.Laboratories = _combosHelper.GetComboLaboratories(model.MedicineId);

                    return View(model);

                }



                LoginViewModel loginViewModel = new LoginViewModel

                {

                    Password = model.Password,

                    RememberMe = false,

                    Username = model.Username

                };



                var result2 = await _userHelper.LoginAsync(loginViewModel);



                if (result2.Succeeded)

                {

                    return RedirectToAction("Index", "Home");

                }

            }



            model.Repositories = _combosHelper.GetComboRepositories();

            model.Medicines = _combosHelper.GetComboMedicines(model.RepositoryId);

            model.Laboratories = _combosHelper.GetComboLaboratories(model.MedicineId);

            return View(model);

        }





        public JsonResult GetMedicines(int repositoryId)

        {

            Repository repository = _context.Repositories

                .Include(r => r.Medicines)

                .FirstOrDefault(m => m.Id == repositoryId);

            if (repository == null)

            {

                return null;

            }



            return Json(repository.Medicines.OrderBy(m => m.Name));

        }



        public JsonResult GetLaboratories(int medicineId)

        {

            Medicine medicine = _context.Medicines

                .Include(d => d.Laboratories)

                .FirstOrDefault(d => d.Id == medicineId);

            if (medicine == null)

            {

                return null;

            }



            return Json(medicine.Laboratories.OrderBy(c => c.Name));

        }

    

}
}

