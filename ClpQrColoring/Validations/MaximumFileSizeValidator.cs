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
    public class MaximumFileSizeValidator
        : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrMsgFormat 
            = "{0} cannot be larger than {1} MB";

        /// <summary> 
        /// Maximum file size in MB 
        /// </summary> 
        public double MaximumFileSize { get; private set; }

        /// <param name="maximumFileSize">Maximum file size in MB</param> 
        public MaximumFileSizeValidator(double maximumFileSize)
        {
            MaximumFileSize = maximumFileSize;           
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!IsValidMaximumFileSize((value as HttpPostedFileBase).ContentLength))
            {
                // I think ErrorMessage is not supposed to be set like this.
                //ErrorMessage = String.Format(_defaultErrMsgFormat, "{0}", MaximumFileSize);
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            string errMsgFormat = ErrorMessage ?? _defaultErrMsgFormat;
            return String.Format(ErrorMessage, name, MaximumFileSize);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
              ModelMetadata metadata
            , ControllerContext context)
        {
            ModelClientValidationRule clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "maximumfilesize"
            };

            clientValidationRule.ValidationParameters.Add("size", MaximumFileSize);

            yield return clientValidationRule;
        }

        private bool IsValidMaximumFileSize(int fileSize)
        {
            return (FileValidationCommon.ConvertBytesToMegabytes(fileSize) <= MaximumFileSize);
        }
    }
}