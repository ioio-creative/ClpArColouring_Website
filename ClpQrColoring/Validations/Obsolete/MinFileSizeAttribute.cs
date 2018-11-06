using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ClpQrColoring.Validations
{
    // https://stackoverflow.com/questions/18415206/dataannotation-regular-expression-always-returns-false-for-file-input
    [AttributeUsage(AttributeTargets.Property |
        AttributeTargets.Field, AllowMultiple = false)]
    public class MinFileSizeAttribute : ValidationAttribute
    {
        private readonly int _minSize;

        public MinFileSizeAttribute(int minSize)
        {
            _minSize = minSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            HttpPostedFileBase postedFile = value as HttpPostedFileBase;

            return _minSize < postedFile.ContentLength;
        }

        public override string FormatErrorMessage(string name)
        {
            string errMsgFormat = String.IsNullOrEmpty(ErrorMessage) ?
                "The file size should be greater than {0} bytes" : ErrorMessage;
            return String.Format(errMsgFormat, _minSize);
        }
    }
}