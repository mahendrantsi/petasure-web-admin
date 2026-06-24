namespace Project.Core.ActionFilter
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RequireIFAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object _compareValue;

        //public RequireIFAttribute(string propertyName, object compareValue)
        //{
        //    _propertyName = propertyName;
        //    _compareValue = compareValue;
        //}

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var property = validationContext.ObjectType.GetProperty(_propertyName);
        //    if (property == null)
        //    {
        //        throw new ArgumentException("Property with this name not found");
        //    }

        //    var propertyValue = property.GetValue(validationContext.ObjectInstance);
        //    if (Equals(propertyValue, _compareValue) && string.IsNullOrWhiteSpace((string)value))
        //    {
        //        return new ValidationResult(ErrorMessage);
        //    }

        //    return ValidationResult.Success;
        //}


        private readonly List<string> _propertyNames = new List<string>();
        private readonly List<object> _compareValues = new List<object>();

        public RequireIFAttribute(string propertyName, object compareValue)
        {
            _propertyNames.Add(propertyName);
            _compareValues.Add(compareValue);
        }

        public RequireIFAttribute(string[] propertyNames, object[] compareValues)
        {
            if (propertyNames.Length != compareValues.Length)
            {
                throw new ArgumentException("Number of property names and compare values must match");
            }

            _propertyNames.AddRange(propertyNames);
            _compareValues.AddRange(compareValues);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            //var isError = false;
            List<bool> isErrors = new List<bool>();
            for (int i = 0; i < _propertyNames.Count; i++)
            {
                var propertyName = _propertyNames[i];
                var property = validationContext.ObjectType.GetProperty(propertyName);
                if (property == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found");
                }

                var memberProperty = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                var propertyValue = property.GetValue(instance);
                if (Equals(propertyValue, _compareValues[i]) &&
                    ((memberProperty.PropertyType.IsEnum && (value == null || (int)value ==0 )) ||
                    (memberProperty.PropertyType == typeof(string) && string.IsNullOrWhiteSpace((string)value)) ||
                      value == null
                    ))
                {
                    isErrors.Add(true);
                }
            }

            if (isErrors.Where(x => x == true).Count() == _propertyNames.Count())
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
