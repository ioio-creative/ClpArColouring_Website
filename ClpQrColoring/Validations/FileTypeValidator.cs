using ClpQrColoring.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClpQrColoring.Validations
{
    [AttributeUsage(AttributeTargets.Property |
        AttributeTargets.Field, AllowMultiple = false)]
    public class FileTypeValidator
        : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrMsgFormat = 
            "{0} must be one of the following file types: {1}";
        private string _commaDelimitedValidFileTypes;

        /// <summary> 
        /// Valid file extentions 
        /// </summary> 
        public string[] ValidFileTypes { get; private set; }

        /// <param name="validFileTypes">Valid file extentions(without the dot) comma delimited list</param> 
        //public FileTypeValidator(params string[] validFileTypes)
        public FileTypeValidator(params string[] validFileTypes)
        {
            ValidFileTypes = validFileTypes;
            _commaDelimitedValidFileTypes = ValidFileTypes.ToConcatenatedString(",");
        }

        public override bool IsValid(object value)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;

            if (value == null || String.IsNullOrEmpty(file.FileName))
            {
                return true;
            }

            if (ValidFileTypes != null)
            {
                bool validFileTypeFound = false;

                // remove the dot in extension
                string fileExt = Path.GetExtension(file.FileName).Substring(1);                
                validFileTypeFound = ValidFileTypes
                    .Contains(fileExt, StringComparer.OrdinalIgnoreCase);
                
                if (!validFileTypeFound)
                {
                    // I think ErrorMessage is not supposed to be set like this.
                    //ErrorMessage = FormatErrorMessage("{0}");
                    return false;
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            string errMsgFormat = ErrorMessage ?? _defaultErrMsgFormat; 
            return String.Format(errMsgFormat, name, _commaDelimitedValidFileTypes);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
              ModelMetadata metadata
            , ControllerContext context)
        {
            ModelClientValidationRule clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "validfiletype"
            };

            clientValidationRule.ValidationParameters.Add("filetypes", _commaDelimitedValidFileTypes);

            yield return clientValidationRule;
        }
    }
}