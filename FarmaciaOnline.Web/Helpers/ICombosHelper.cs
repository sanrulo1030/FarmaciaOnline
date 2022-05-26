using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace FarmaciaOnline.Web.Helpers
{ 

    public interface ICombosHelper
{
    IEnumerable<SelectListItem> GetComboCategories();

}
}


