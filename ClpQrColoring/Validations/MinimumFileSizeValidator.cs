using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace ClpQrColoring.Validations
{
    // https://code.msdn.microsoft.com/MVC-File-upload-unobtrusive-d0099c30
    [AttributeUsage(AttributeTargets.Property |
        AttributeTargets.Field, AllowMultiple = false)]
    public class MinimumFileSizeValidator
        : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrMsgFormat = 
            "{0} cannot be smaller than {1} MB";

        /// <summary> 
        /// Minimum file size in MB 
        /// </summary> 
        public double MinimumFileSize { get; private set; }

        /// <param name="minimumFileSize">MinimumFileSize file size in MB</param> 
        public MinimumFileSizeValidator(double minimumFileSize)
        {
            MinimumFileSize = minimumFileSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!IsValidMinimumFileSize((value as HttpPostedFileBase).ContentLength))
            {
                // I think ErrorMessage is not supposed to be set like this.
                //ErrorMessage = FormatErrorMessage("{0}");
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            string errMsgFormat = ErrorMessage ?? _defaultErrMsgFormat;
            return String.Format(errMsgFormat, name, MinimumFileSize);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
              ModelMetadata metadata
            , ControllerContext context)
        {
            ModelClientValidationRule clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "minimumfilesize"
            };

            clientValidationRule.ValidationParameters.Add("size", MinimumFileSize);

            yield return clientValidationRule;
        }

        private bool IsValidMinimumFileSize(int fileSize)
        {
            // use < rather than <= as 0 file size is not allowed
            return FileValidationCommon.ConvertBytesToMegabytes(fileSize) > MinimumFileSize;
        }
    }
}