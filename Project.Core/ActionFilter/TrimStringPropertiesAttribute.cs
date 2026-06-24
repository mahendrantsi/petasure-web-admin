using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.ActionFilter
{
    public class TrimStringPropertiesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                TrimStringProperties(argument);
            }

            base.OnActionExecuting(context);
        }

        private void TrimStringProperties(object model)
        {
            if (model == null)
                return;

            var properties = model.GetType().GetProperties().Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

            foreach (var property in properties)
            {
                var value = (string)property.GetValue(model);
                if (value != null)
                {
                    var trimmedValue = value.Trim();
                    property.SetValue(model, trimmedValue);
                }
            }
        }
    }
}
