using System.ComponentModel.DataAnnotations;
namespace FarmaciaOnline.Web
{

    public class EmailRequest

    {
        [EmailAddress]
        [Required]

        public string Email { get; set; }

    }
}
