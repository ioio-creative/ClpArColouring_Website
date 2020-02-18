using ClpQrColoring.DataAccess;
using ClpQrColoring.Validations;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ClpQrColoring.Models.ArCharacters
{
    public class User
    {
        [StringLength(ColorAppDataAccess.RequiredPrimaryKeyLength)]        
        public string ID { get; set; }

        //[Display(Name = "Name")]
        //[Required(ErrorMessage = "Please enter name.")]
        //[MaxLength(50)]
        //public string Name { get; set; }

        [Display(Name = "電郵")]
        [Required(ErrorMessage = "請填寫")]
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        //[RegularExpression(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        //    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        //    ErrorMessage = "格式錯誤，請重新輸入")]
        // https://stackoverflow.com/questions/25994423/emailaddress-or-datatype-email-attribute
        //[DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "格式錯誤，請重新輸入")]               
        [StringLength(100, ErrorMessage = "{0}長度不能超過{1}字")]
        public string Email { get; set; }

        [Display(Name = "確認電郵")]
        [Required(ErrorMessage = "請填寫")]
        [Compare("Email", ErrorMessage = "與上址不吻合，請重新輸入")]
        [StringLength(100, ErrorMessage = "{0}長度不能超過{1}字")]
        [EmailAddress(ErrorMessage = "格式錯誤，請重新輸入")]  // keep this since iOS browser will show different keyboards for different html5 input types
        public string ConfirmEmail { get; set; }

        [Display(Name = "上載作品")]
        [Required(ErrorMessage = "請選取相片")]
        [DataType(DataType.Upload)]
        /* Obsolete */
        /*
        https://stackoverflow.com/questions/18415206/dataannotation-regular-expression-always-returns-false-for-file-input 
        [MaxFileSize(5242880, ErrorMessage = "The file size should not exceed {0} bytes")]  // 5MB
        [MinFileSize(0, ErrorMessage = "The file size should be greater than {0} bytes")]  // 5MB
        [FileTypes("jpg,jpeg,png", ErrorMessage = "Invalid file type. Only the following types {0} are supported.")]
        */
        // https://code.msdn.microsoft.com/MVC-File-upload-unobtrusive-d0099c30
        [FileTypeValidator("jpg", "jpeg", "png", ErrorMessage = "檔案須為.jpg/.png格式")]
        [MaximumFileSizeValidator(10, ErrorMessage = "檔案上限：10MB")]
        [MinimumFileSizeValidator(0, ErrorMessage = "檔案下限：0MB")]                
        //[FileUploadValidator(0.5, 2.4, "png")]
        public HttpPostedFileBase PostedFile { get; set; }

        [StringLength(70)]
        public string AwsS3OrigImgKey { get; set; }

        [StringLength(70)]
        public string AwsS3WarpedImgKey { get; set; }

        [StringLength(70)]
        public string AwsS3VideoKey { get; set; }

        [StringLength(70)]
        public string AwsS3VideoThumbnailKey { get; set; }

        [StringLength(150)]
        public string AwsS3BucketDomain { get; set; }

        public string SignalrHubConnectionId { get; set; }
    }
}