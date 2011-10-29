using System;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WCEmergency.Extentions
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName(this string foo, Expression<Func<object>> property)
        {

            var propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
            }

            var propertyName = propertyInfo.Name;
            return propertyName;
        }
    }
}
