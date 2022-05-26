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
        ? $"https://farmaciaonlinedemo.blob.core.windows.net/categories/{ImageId}"
        : $"https://farmaciaonlinedemo.blob.core.windows.net/products/{ImageId}";
    }
}
