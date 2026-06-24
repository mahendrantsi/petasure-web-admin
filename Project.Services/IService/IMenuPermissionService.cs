
namespace SmartPay.Services.IService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SmartPay.Data.DBEntities;
    using SmartPay.Models.AdminModel;
    using SmartPay.Models.CommonModel;
    using SmartPay.Services.ServiceEntities;

    public interface IMenuPermissionService
    {
        /// <summary>
        /// Create Dynamic Menu.
        /// </summary>
        /// <param name="menuViewModel">Menu View Model.</param>
        /// <returns>Created Menu Model.</returns>
        Task<ServiceResponse<MenuViewModel>> Create(MenuViewModel menuViewModel);

        /// <summary>
        /// Get Parent Menu.
        /// </summary>
        /// <returns>Parent Menu List.</returns>
        SelectList GetParentMenus();

        /// <summary>
        /// Get Menu List by Id.
        /// </summary>
        /// <param name="id">Menu Id.</param>
        /// <returns>Menu List.</returns>
        List<MenuViewModel> GetMenuList(long id);

        ServiceResponse<List<MenuListViewModel>> GetMenuResult(JQueryDataTableModel requestParam);

        Task<ServiceResponse<MenuViewModel>> GetMenuById(long menuId);

        Task<ServiceResponse<MenuViewModel>> Edit(MenuViewModel menuViewModel);

        ServiceResponse<MenuListViewModel> GetMenuDetailById(long menuId);
    }
}
