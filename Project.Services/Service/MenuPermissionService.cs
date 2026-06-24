
namespace SmartPay.Services.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SmartPay.Core.Extension;
    using SmartPay.Data.DBEntities;
    using SmartPay.Models.AdminModel;
    using SmartPay.Models.CommonModel;
    using SmartPay.Services.IService;
    using SmartPay.Services.ServiceEntities;
    using SmartPay.Persistence.UOW;

    public class MenuPermissionService : BaseService, IMenuPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuPermissionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Create Dynamic Menu.
        /// </summary>
        /// <param name="menuViewModel">Menu View Model.</param>
        /// <returns>Created Menu Model.</returns>
        public async Task<ServiceResponse<MenuViewModel>> Create(MenuViewModel menuViewModel)
        {
            ServiceResponse<MenuViewModel> objReturn = new ServiceResponse<MenuViewModel>();
            try
            {
                MenuMaster menuModel = new MenuMaster();
                menuModel = _mapper.Map<MenuViewModel, MenuMaster>(menuViewModel);
                menuModel.CreatedOn = DateTime.UtcNow;
                menuModel.IsActive = true;
                this._unitOfWork.GenericRepository<MenuMaster>().Add(menuModel);
                await this._unitOfWork.SaveChangesAsync();
                if (menuModel.Id > 0)
                {
                    objReturn = this.SetResultStatus<MenuViewModel>(menuViewModel, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<MenuViewModel>(null, MessageStatus.Fail, false);
                }
            }
            catch
            {
                objReturn = this.SetResultStatus<MenuViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        /// <summary>
        /// Get Parent Menu.
        /// </summary>
        /// <returns>Parent Menu List.</returns>
        public SelectList GetParentMenus()
        {
            var parentList = this._unitOfWork.GenericRepository<MenuMaster>().Get(x => x.IsActive == true && (x.ParentId == null || x.ParentId == 0));
            return new SelectList(parentList, "Id", "MenuName");
        }

        /// <summary>
        /// Get Menu List by Id.
        /// </summary>
        /// <param name="id">Menu Id.</param>
        /// <returns>Menu List.</returns>
        public List<MenuViewModel> GetMenuList(long id)
        {
            List<MenuViewModel> menuLinksList = new List<MenuViewModel>();
            try
            {
                object[] param = new object[1];
                param[0] = 0;
                List<MenuListResult> menuListResultResult = _unitOfWork.MenuPermissionRepository.GetMenuList(param);
                if (menuListResultResult.Count > 0)
                {
                    menuLinksList = MenuListRecursive(menuListResultResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return menuLinksList;
        }

        /// <summary>
        /// Menu Recursive Function.
        /// </summary>
        /// <param name="menuListResultResult">Menu List.</param>
        /// <returns>Resursive List.</returns>
        private List<MenuViewModel> MenuListRecursive(List<MenuListResult> menuListResultResult)
        {
            MenuViewModel menu;
            List<MenuViewModel> menuLinksList = new List<MenuViewModel>();
            foreach (var outeritems in menuListResultResult.Where(x => x.ParentId == 0).OrderBy(x => x.DisplayOrder))
            {
                menu = new MenuViewModel();
                menu.Id = outeritems.Id;
                menu.ParentId = outeritems.ParentId;
                menu.MenuName = outeritems.MenuName;
                menu.DisplayName = outeritems.DisplayName;
                menu.ActionName = outeritems.Action;
                menu.ControllerName = outeritems.Controller;
                menu.Url = outeritems.Url;
                menu.Icon = outeritems.Icon;
                menu.IsActive = outeritems.IsActive;
                menu.IsDefault = outeritems.IsDefault;
                menu.DisplayOrder = outeritems.DisplayOrder;
                if (menuListResultResult.Where(x => x.ParentId == menu.Id).Count() > 0)
                {
                    menu.ChildMenuList = MenuChildList(menu.Id, menuListResultResult);
                }

                menuLinksList.Add(menu);
            }

            return menuLinksList;
        }

        /// <summary>
        /// Get Menu Chile List.
        /// </summary>
        /// <param name="parentId">Parent Id.</param>
        /// <param name="menuListResultResult">Menu List.</param>
        /// <returns>Child Menu List.</returns>
        private List<MenuViewModel> MenuChildList(long parentId, List<MenuListResult> menuListResultResult)
        {
            var menuLinksList = new List<MenuViewModel>();
            try
            {
                MenuViewModel menu;
                foreach (var inneritems in menuListResultResult.Where(x => x.ParentId == parentId).OrderBy(x => x.DisplayOrder))
                {
                    menu = new MenuViewModel();
                    menu.Id = inneritems.Id;
                    menu.ParentId = inneritems.ParentId;
                    menu.MenuName = inneritems.MenuName;
                    menu.DisplayName = inneritems.DisplayName;
                    menu.ActionName = inneritems.Action;
                    menu.ControllerName = inneritems.Controller;
                    menu.Url = inneritems.Url;
                    menu.Icon = inneritems.Icon;
                    menu.IsActive = inneritems.IsActive;
                    menu.IsDefault = inneritems.IsDefault;
                    menu.DisplayOrder = inneritems.DisplayOrder;
                    menuLinksList.Add(menu);
                }
            }
            catch
            {
                throw;
            }

            return menuLinksList;
        }

        /// <summary>
        /// GetMenuResult Method.
        /// </summary>
        /// <param name="requestParam">Request Input Parameters.</param>
        /// <returns>Menu List.</returns>
        public ServiceResponse<List<MenuListViewModel>> GetMenuResult(JQueryDataTableModel requestParam)
        {
            ServiceResponse<List<MenuListViewModel>> objReturn = new ServiceResponse<List<MenuListViewModel>>();
            List<MenuListViewModel> menuListModel = new List<MenuListViewModel>();
            try
            {
                var propertyInfo = typeof(MenuListViewModel).GetProperty(requestParam.ordercolumn);
                var menuListResult = this._unitOfWork.GenericRepository<GetMenus>().Get(x => (x.MenuName.Contains(requestParam.search) ||
                x.DisplayName.Contains(requestParam.search) || x.ParentMenu.Contains(requestParam.search))).ToList();
                menuListModel = _mapper.Map<List<MenuListViewModel>>(menuListResult);
                if (menuListModel.Count > 0)
                {
                    objReturn = this.SetResultStatus<List<MenuListViewModel>>(menuListModel, MessageStatus.Success, true);
                    //(objReturn.Data, objReturn.recordsTotal, objReturn.recordsFiltered) = DataTableShorting<MenuListViewModel>(menuListModel, requestParam, propertyInfo);
                }
                else
                {
                    objReturn = this.SetResultStatus<List<MenuListViewModel>>(menuListModel, MessageStatus.Success, true);
                }
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<List<MenuListViewModel>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        /// <summary>
        /// GetMenuById Method.
        /// </summary>
        /// <param name="menuId">Menu Id.</param>
        /// <returns>Menu Model.</returns>
        public async Task<ServiceResponse<MenuViewModel>> GetMenuById(long menuId)
        {
            ServiceResponse<MenuViewModel> objReturn = new ServiceResponse<MenuViewModel>();
            MenuViewModel menuModel = new MenuViewModel();
            try
            {
                var menuResult = _unitOfWork.GenericRepository<MenuMaster>().Get(x => x.Id == menuId).FirstOrDefault();
                menuModel = _mapper.Map<MenuViewModel>(menuResult);
                if (menuModel.Id > 0)
                {
                    objReturn = this.SetResultStatus<MenuViewModel>(menuModel, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<MenuViewModel>(null, MessageStatus.Fail, false);
                }
            }
            catch
            {
                objReturn = this.SetResultStatus<MenuViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        /// <summary>
        /// Save Edit Menu Method.
        /// </summary>
        /// <param name="menuViewModel">Menu View Model.</param>
        /// <returns>Updated Menu View.</returns>
        public async Task<ServiceResponse<MenuViewModel>> Edit(MenuViewModel menuViewModel)
        {
            ServiceResponse<MenuViewModel> objReturn = new ServiceResponse<MenuViewModel>();
            try
            {
                var menuResult = _unitOfWork.GenericRepository<MenuMaster>().Get(x => x.Id == menuViewModel.Id).FirstOrDefault();
                menuResult.MenuName = menuViewModel.MenuName;
                menuResult.DisplayName = menuViewModel.DisplayName;
                menuResult.Url = menuViewModel.Url;
                menuResult.DisplayOrder = menuViewModel.DisplayOrder ?? 0;
                menuResult.ParentId = menuViewModel.ParentId ?? 0;
                menuResult.ModifiedDate = menuViewModel.ModifiedDate;
                menuResult.Icon = menuViewModel.Icon;
                this._unitOfWork.GenericRepository<MenuMaster>().UpdateEntity(menuResult);
                await this._unitOfWork.SaveChangesAsync();
                if (menuResult.Id > 0)
                {
                    objReturn = this.SetResultStatus<MenuViewModel>(menuViewModel, MessageStatus.Update, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<MenuViewModel>(null, MessageStatus.Fail, false);
                }
            }
            catch
            {
                objReturn = this.SetResultStatus<MenuViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        /// <summary>
        /// GetMenuDetailById Mehtod.
        /// </summary>
        /// <param name="menuId">Menu Id.</param>
        /// <returns>Menu Model.</returns>
        public ServiceResponse<MenuListViewModel> GetMenuDetailById(long menuId)
        {
            ServiceResponse<MenuListViewModel> objReturn = new ServiceResponse<MenuListViewModel>();
            MenuListViewModel menuModel = new MenuListViewModel();
            try
            {
                var menuResult = _unitOfWork.GenericRepository<GetMenus>().Get(x => x.Id == menuId).FirstOrDefault();
                menuModel = _mapper.Map<MenuListViewModel>(menuResult);
                if (menuModel.Id > 0)
                {
                    objReturn = this.SetResultStatus<MenuListViewModel>(menuModel, MessageStatus.Success, true);
                }
                else
                {
                    objReturn = this.SetResultStatus<MenuListViewModel>(null, MessageStatus.Fail, false);
                }
            }
            catch
            {
                objReturn = this.SetResultStatus<MenuListViewModel>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }
    }
}
