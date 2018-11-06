namespace ClpQrColoring.Validations
{
    public class FileValidationCommon
    {
        public static double ConvertBytesToMegabytes(
            int bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}