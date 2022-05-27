using System;
using System.ComponentModel.DataAnnotations;

namespace FarmaciaOnline.Web.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        [Display(Name = "Image")]
        public Guid ImageId { get; set; }
        //TODO: Pending to put the correct paths
        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
        ? $"https://farmaciaonlineweb.azurewebsites.net/images/noimage.png/{ImageId}"
        : $"https://farmaciaonlinedemo.blob.core.windows.net/products/{ImageId}";
    }
}
