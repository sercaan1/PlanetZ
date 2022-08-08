using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class IsImageValidAttribute : ValidationAttribute
    {
        public int MaxFileSize { get; set; } = 1024;
        public override bool IsValid(object value)
        {
            IFormFile formFile = value as IFormFile;

            if (formFile == null)
                return true;

            if (!formFile.ContentType.StartsWith("image/"))
            {
                ErrorMessage = "Invalid picture file";
                return false;
            }
            else if (formFile.Length > MaxFileSize * 1024)
            {
                ErrorMessage = "Maximum file size: " + MaxFileSize + "kb";
                return false;
            }
            return true;
        }
    }
}
