using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FarmaciaOnline.Web.Models
{
    public class Repository
    {

        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        [Required]
        public string Name { get; set; }
        public ICollection<Medicine> Medicines { get; set; }
        [DisplayName("Medicines Number")]
        public int MedicinesNumber => Medicines == null ? 0 : Medicines.Count;

    }
}
