using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ClpQrColoring.Validations
{
    // https://stackoverflow.com/questions/18415206/dataannotation-regular-expression-always-returns-false-for-file-input
    [AttributeUsage(AttributeTargets.Property |
        AttributeTargets.Field, AllowMultiple = false)]
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public MaxFileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            HttpPostedFileBase postedFile = value as HttpPostedFileBase;

            return _maxSize >= postedFile.ContentLength;
        }

        public override string FormatErrorMessage(string name)
        {
            string errMsgFormat = String.IsNullOrEmpty(ErrorMessage) ?
                "The file size should not exceed {0} bytes" : ErrorMessage;

            return String.Format(errMsgFormat, _maxSize);
        }
    }
}