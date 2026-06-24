using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models.Master;
using System.Collections.Generic;
using System.Linq;

namespace Project.Web.UiUtility
{
    public static class Utility
    {
        public static IEnumerable<SelectListItem> GetDocTypeDDL(List<DocumentTypeViewModel> list)=>list.Select(x=> new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).AsEnumerable();
             
         
    }
}
