using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace NovibetIPStackAPI.Tests.Kernel
{
    /// <summary>
    /// A helper class that can assert two objects of the same type have equal property or field values
    /// </summary>
    public class AreEqualHelper
    {
        /// <summary>
        /// A method that asserts that two objects of the same type have equal property values.
        /// </summary>
        /// <typeparam name="T">The type of the objects</typeparam>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="propertiesToExclude">Properties which we want to exclude from the equality check. Useful for particular scenarios such as unique identifiers, timestamps etc.</param>
        /// <returns>A tuple with a boolean that asserts if the properties were equal or not and a list of strings describing which properties were not equal, if any.</returns>
        public static (bool,List<string>) HasEqualPropertyValues<T>(T expected, T actual, List<PropertyInfo> propertiesToExclude)
        {
            List<string> failures = new List<string>();

            List<PropertyInfo> properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            if (propertiesToExclude != null)
            {
                properties.RemoveAll(allPr => propertiesToExclude.Where(exclPr => exclPr.Name == allPr.Name).Any());
            }

            foreach (PropertyInfo property in properties)
            {

                var expectedValue = property.GetValue(expected);

                var actualValue = property.GetValue(actual);

                if (expectedValue == null && actualValue == null) continue;

                if (!expectedValue.Equals(actualValue)) failures.Add($"{property.Name}: Expected:<{expectedValue}> Actual:<{actualValue}>");
            }

            if (failures.Any())
            {
               return (false, failures);
            }

            return (true, null);
        }

        /// <summary>
        /// A method that asserts that two objects of the same type have equal field values.
        /// </summary>
        /// <typeparam name="T">The type of the objects</typeparam>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="fieldsToExclude">Fields which we want to exclude from the equality check. Useful for particular scenarios such as unique identifiers, timestamps etc.</param>
        /// <returns>A tuple with a boolean that asserts if the fields were equal or not and a list of strings describing which fields were not equal, if any.</returns>
        public static (bool, List<string>) HasEqualFieldValues<T>(T expected, T actual, List<FieldInfo> fieldsToExclude)
        {
            List<string> failures = new List<string>();

            List<FieldInfo> Fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance).ToList();

            if (fieldsToExclude != null)
            {
                Fields.RemoveAll(allPr => fieldsToExclude.Where(exclPr => exclPr.Name == allPr.Name).Any());
            }

            foreach (FieldInfo Field in Fields)
            {

                var expectedValue = Field.GetValue(expected);

                var actualValue = Field.GetValue(actual);

                if (expectedValue == null && actualValue == null) continue;

                if (!expectedValue.Equals(actualValue)) failures.Add($"{Field.Name}: Expected:<{expectedValue}> Actual:<{actualValue}>");
            }

            if (failures.Any())
            {
                return (false, failures);
            }

            return (true, null);
        }
        
    }
}
