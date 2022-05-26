using System;
using System.Threading.Tasks;
using FarmaciaOnline.Web.Models;

namespace FarmaciaOnline.Web.Helpers
{
    public interface IConverterHelper
    {
        Category ToCategory(CategoryViewModel model, Guid imageId, bool isNew);
        ProductViewModel ToProductViewModel(Product product);


        CategoryViewModel ToCategoryViewModel(Category category);
        Task<Product> ToProductAsync(ProductViewModel model, bool v);
    }

}
