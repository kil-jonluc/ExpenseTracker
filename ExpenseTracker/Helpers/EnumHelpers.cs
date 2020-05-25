using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExpenseTracker.Helpers
{
    public static class EnumHelpers<T>
    {
        //public static string GetDisplayValue(T value)
        //{
        //    var fieldInfo = value.GetType().GetField(value.ToString());

        //    var descriptionAttributes = fieldInfo.GetCustomAttributes(
        //        typeof(DisplayAttribute), false) as DisplayAttribute[];

        //    if (descriptionAttributes[0].ResourceType != null)
        //        return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

        //    if (descriptionAttributes == null) return string.Empty;
        //    return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        //}

        public static string GetDescription(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].Description != null)
            {
                return descriptionAttributes[0].Description;
            }
            else
            {
                // fallback on key name
                if (descriptionAttributes == null) return string.Empty;
                return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
            }
        }

        //private static string LookupResource(Type resourceManagerProvider, string resourceKey)
        //{
        //    foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
        //    {
        //        if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
        //        {
        //            System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
        //            return resourceManager.GetString(resourceKey);
        //        }
        //    }

        //    return resourceKey; // Fallback with the key name
        //}
    }
}
