using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ClpQrColoring.Validations
{
    // https://stackoverflow.com/questions/18415206/dataannotation-regular-expression-always-returns-false-for-file-input
    [AttributeUsage(AttributeTargets.Property |
        AttributeTargets.Field, AllowMultiple = false)]
    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<string> _types;

        public FileTypesAttribute(string types)
        {
            _types = types.Split(',').ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            HttpPostedFileBase postedFile = value as HttpPostedFileBase;

            string fileExt = Path.GetExtension(postedFile.FileName).Substring(1);
            return _types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
        }

        public override string FormatErrorMessage(string name)
        {
            string errMsgFormat = String.IsNullOrEmpty(ErrorMessage) ?
                "Invalid file type. Only the following types {0} are supported." : ErrorMessage;

            return String.Format(errMsgFormat, String.Join(", ", _types));
        }
    }
}