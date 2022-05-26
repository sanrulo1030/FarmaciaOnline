using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmaciaOnline.Web;
using FarmaciaOnline.Web.Data;
using FarmaciaOnline.Web.Data.Entities;
using FarmaciaOnline.Web.Enums;
using FarmaciaOnline.Web.Helpers;
using FarmaciaOnline.Web.Models;

namespace FarmaciaOnline.Data {
public class SeedDb
{
    private readonly ApplicationDbContext _context;
    private readonly IUserHelper _userHelper;

    public SeedDb(ApplicationDbContext context, IUserHelper userHelper)
    {
        _context = context;
        _userHelper = userHelper;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckRepositoriesAsync();
        await CheckCategoriesAsync();
        await CheckRolesAsync();
        await CheckUserAsync("1", "Santiago", "A", "sanrulo@hotmail.com", "3000000000", "Curva del diablo", UserType.Admin);

    }
    private async Task CheckRolesAsync()
    {
        await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
        await _userHelper.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckUserAsync(
        string document,
        string firstName,
        string lastName,
        string email,
        string phone,
        string address,
        UserType userType)
    {
        User user = await _userHelper.GetUserAsync(email);
        if (user == null)
        {
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Address = address,
                Document = document,
                Laboratory = _context.Laboratories.FirstOrDefault(),
                UserType = userType
            };

            await _userHelper.AddUserAsync(user, "123456"); //password debe tener una longitud de 6 caracteres
            await _userHelper.AddUserToRoleAsync(user, userType.ToString());
        }

        return user;
    }


        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Antiespasmos" });
                _context.Categories.Add(new Category { Name = "Hipertension" });
                _context.Categories.Add(new Category { Name = "Antialergicos" });
                _context.Categories.Add(new Category { Name = "Vitaminas" });
                _context.Categories.Add(new Category { Name = "Minerales" });
            }
            await _context.SaveChangesAsync();
        }


        private async Task CheckRepositoriesAsync()
    {
            if (!_context.Repositories.Any())
            {
                _context.Repositories.Add(new Repository
                {
                    Name = "TiendaMedellín",
                    Medicines = new List<Medicine>
                {
                    new Medicine
                    {
                        Name = "Loratadina",
                        Laboratories = new List<Laboratory>
                        {
                            new Laboratory { Name = "Bayer MK" },
                            new Laboratory { Name = "MG" },
                            new Laboratory { Name = "DCC" }
                        }
                    },
                    new Medicine
                    {
                        Name = "Oftamotrizol",
                        Laboratories = new List<Laboratory>
                        {
                            new Laboratory { Name = "MGN" }
                        }
                    },
                    new Medicine
                    {
                        Name = "Nariño",
                        Laboratories = new List<Laboratory>
                        {
                            new Laboratory { Name = "Pfizer" },
                            new Laboratory { Name = "Aztrazeneca" },
                            new Laboratory { Name = "Logistic" }
                        }
                    }
                }
                });
                _context.Repositories.Add(new Repository
                {
                    Name = "TiendaCali",
                    Medicines = new List<Medicine>
                {
                    new Medicine
                    {
                        Name = "Codeina",
                        Laboratories = new List<Laboratory>
                        {
                            new Laboratory { Name = "Mar Del Plata" },
                            new Laboratory { Name = "Quilmes" },
                            new Laboratory { Name = "Lanús" }
                        }
                    },
                    new Medicine
                    {
                        Name = "Cetirizina",
                        Laboratories = new List<Laboratory>
                        {
                            new Laboratory { Name = "Fallcom" },
                            new Laboratory { Name = "Medick" },
                        }
                    },
                }
                });
            }
            await _context.SaveChangesAsync();
        }


    }
}


