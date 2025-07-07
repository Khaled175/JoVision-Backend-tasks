using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Task47.Models
{
    public class UploadImageRequest
    {
        [Required(ErrorMessage = "Image file is required.")]
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Owner name is required.")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters.")]
        public string? Owner { get; set; }
    }
}
