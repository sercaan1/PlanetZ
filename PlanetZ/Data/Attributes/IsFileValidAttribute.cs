using System.ComponentModel.DataAnnotations;

namespace PlanetZ.Data.Attributes
{
    public class IsFileValidAttribute : ValidationAttribute
    {
        public int MaxFileSize { get; set; } = 1024;
        public override bool IsValid(object value)
        {
            IFormFile formFile = value as IFormFile;

            if (formFile == null)
                return true;

            //if (!formFile.ContentType.EndsWith(".pdf") || !formFile.ContentType.EndsWith(".jpg") || !formFile.ContentType.EndsWith(".png") || !formFile.ContentType.EndsWith(".jpeg"))
            //{
            //    ErrorMessage = "File extension must be .pdf, .jpg, .png or .jpeg";
            //    return false;
            //}
            else if (formFile.Length > MaxFileSize * 1024)
            {
                ErrorMessage = "Maximum file size: " + MaxFileSize + "kb";
                return false;
            }
            else if (formFile.Length == 0)
            {
                ErrorMessage = "file size cannot be 0 kb";
                return false;
            }
            return true;
        }
    }
}
