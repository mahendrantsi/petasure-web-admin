using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Project.Web.Common
{
    public class ChangeLog<T> where T : class
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<Change> ColumnsChange { get; set; }

        public ChangeLog(string createdBy, DateTime createdOn)
        {
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ColumnsChange = new List<Change>();
        }

        public static List<ChangeLog<T>> GenerateChangeLogs<T>(List<T> list, string[] excludedProperty = null) where T : class
        {
            List<ChangeLog<T>> changeLogs = new List<ChangeLog<T>>();

            if (excludedProperty is null)
                excludedProperty = new string[] { "CreatedOn", "CreatedBy" };

            try
            {
                for (int i = 1; i < list.Count; i++)
                {
                    T previousData = list[i - 1];
                    T currentData = list[i];

                    Type type = typeof(T);
                    PropertyInfo[] properties = type.GetProperties();

                    var changes = properties
                        .Where(prop => !object.Equals(prop.GetValue(previousData), prop.GetValue(currentData))
                                && !excludedProperty.Contains(prop.Name)
                        )

                        .Select(prop => new Change
                        {
                            Column = (prop.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single()).DisplayName,
                            OldValue = prop.GetValue(previousData)?.ToString(),
                            NewValue = prop.GetValue(currentData)?.ToString()
                        }).ToList();


                    if (changes.Any())
                    {
                        var changeLog = new ChangeLog<T>(currentData.GetType().GetProperty("CreatedBy").GetValue(currentData).ToString(), Convert.ToDateTime(currentData.GetType().GetProperty("CreatedOn").GetValue(currentData)));
                        changeLog.ColumnsChange.AddRange(changes);
                        changeLogs.Add(changeLog);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return changeLogs;
        }


        public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException( "No property reference expression was found.", "propertyExpression");
            }
           // var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);

            var attribute = memberInfo.GetCustomAttributes(typeof(DisplayNameAttribute), false).SingleOrDefault();

            if (attribute == null)
            {
                return memberInfo.Name;
            }
            else
            {
                 return    ((DisplayNameAttribute)attribute).DisplayName;
            }
        }
        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            Debug.Assert(propertyExpression != null, "propertyExpression != null");
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }
       

    }

    public class Change
    {
        public string Column { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }


}
