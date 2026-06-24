namespace Project.Services.ServiceEntities
{
    using Project.Models.CommonModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class BaseService
    {
        public virtual ServiceResponse<T> SetResultStatus<T>(T objData, string message, bool isSuccess)
            where T : class
        {
            ServiceResponse<T> objReturn = new ServiceResponse<T>();
            objReturn.Message = message;
            objReturn.IsSuccess = isSuccess;
            objReturn.Data = objData;
            objReturn.EndOn = DateTime.UtcNow;
            return objReturn;
        }



        public virtual ServiceResponse<T> AutoSetResult<T>(T objData, string message)
        where T : class
        {
            return new ServiceResponse<T>()
            {
                Message = (objData != null) ? message : "Data not found",
                IsSuccess = (objData != null),
                Data = objData,
                EndOn = DateTime.UtcNow,
            };
        }
         

        public virtual (List<T>, int, int) DataTableShorting<T>(List<T> objData, JQueryDataTableModel param, PropertyInfo propertyInfo) where T : class
        {
            int recordsTotal = objData.Count, recordsFiltered = objData.Count;

            if (param.sortorder == "asc")
            {
                objData = objData.OrderBy(x => propertyInfo.GetValue(x, null))
                                           .Skip(param.start).Take(param.length)
                                           .ToList();
            }
            else
            {
                if (param.length == 0)
                {
                    objData = objData.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                }
                else
                {

                    objData = objData.OrderByDescending(x => propertyInfo.GetValue(x, null))
                                     .Skip(param.start).Take(param.length)
                                     .ToList();
                }
            }

            return (objData, recordsTotal, recordsFiltered);
        }


        public virtual (List<T>, int, int) DataTableOnlyShorting<T>(List<T> objData, JQueryDataTableModel dataModal, PropertyInfo propertyInfo) where T : class
        {
            int recordsTotal = objData.Count, recordsFiltered = objData.Count;

            if (dataModal.sortorder == "asc")
            {
                objData = objData.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
            }
            else
            {
                objData = objData.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
            }

            return (objData, recordsTotal, recordsFiltered);
        }
         

        private static Func<T, object> GetOrderByExpression<T>(string sortColumn)
        {
            Func<T, object> orderByExpr = null;
            if (!String.IsNullOrEmpty(sortColumn))
            {
                Type sponsorResultType = typeof(T);

                if (sponsorResultType.GetProperties().Any(prop => prop.Name == sortColumn))
                {
                    System.Reflection.PropertyInfo pinfo = sponsorResultType.GetProperty(sortColumn);
                    orderByExpr = (data => pinfo.GetValue(data, null));
                }
            }
            return orderByExpr;
        }
    }
}
