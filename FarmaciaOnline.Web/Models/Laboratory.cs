using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FarmaciaOnline.Web.Models
{
    public class Laboratory
    {
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener al menos un caracter")]
        [Required]
        public string Name { get; set; }
        [JsonIgnore] //lo ignora en la respuesta json
        [NotMapped] //no se crea en la base de datos
        public int IdMedicine { get; set; }
        [JsonIgnore] 
        public Medicine Medicine { get; set; } 
    }
}
