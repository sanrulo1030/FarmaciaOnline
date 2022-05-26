using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FarmaciaOnline.Web.Models
{
    public class Medicine
    {
     

            public int Id { get; set; }
            [MaxLength(50, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
            [Required]
            public string Name { get; set; }
            public DateTime ExpeditionDate { get; set; }
            public DateTime ExpirationDate { get; set; }
            public string  RouteAdministration { get; set; }
            public string  Presentation { get; set; }
            public ICollection<Laboratory> Laboratories { get; set; }
            [DisplayName("Laboratories Number")]
            public int LaboratoriesNumber => Laboratories == null ? 0 : Laboratories.Count;
            [JsonIgnore] //lo ignora en la respuesta json
            [NotMapped] //no se crea en la base de datos
            public int IdRepository { get; set; }
            [JsonIgnore] 
            public Repository Repository { get; set; } 

    }
}
