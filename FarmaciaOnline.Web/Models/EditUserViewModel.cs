using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FarmaciaOnline.Web.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string Document { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://tiendaonlineweb.azurewebsites.net/images/noimage.png"
            : $"https://tiendaonlinedemo.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [Display(Name = "Repository")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Repository.")]
        public int RepositoryId { get; set; }

        public IEnumerable<SelectListItem> Repositories { get; set; }

        [Required]
        [Display(Name = "Medicine")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a medicine.")]
        public int MedicineId { get; set; }

        public IEnumerable<SelectListItem> Medicines { get; set; }

        [Required]
        [Display(Name = "Laboratory")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Laboratory.")]
        public int LaboratoryId { get; set; }

        public IEnumerable<SelectListItem> Laboratories { get; set; }
    }

}
