using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GoodboxDesign.Utils.Attributes
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string testedPropertyName;

        public DateGreaterThanAttribute(string testedPropertyName)
        {
            this.testedPropertyName = testedPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var testValue = DateTime.Parse(value.ToString());

            DateTime propertyTestedValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if(!string.IsNullOrEmpty(testedPropertyName))
            {
                var propertyTestedInfo = validationContext.ObjectType.GetProperty(testedPropertyName);

                if (propertyTestedInfo == null)
                {
                    return new ValidationResult(string.Format("Unknown property {0}", testedPropertyName));
                }

                propertyTestedValue = DateTime.Parse(propertyTestedInfo.GetValue(validationContext.ObjectInstance, null).ToString());
            }


            if(testValue >= propertyTestedValue)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

    }
}