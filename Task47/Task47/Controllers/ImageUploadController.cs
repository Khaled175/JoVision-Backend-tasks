using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task47.Models;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace Task47.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public ImageUploadController()
        {
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = request.Image;
            var owner = request.Owner;

            if (image == null || image.Length == 0)
            {
                return BadRequest("No image file uploaded.");
            }

            if (image.ContentType != "image/jpeg")
            {
                return BadRequest("Only JPEG image files are allowed.");
            }

            var fileName = Path.GetFileName(image.FileName);
            var imageFilePath = Path.Combine(_uploadPath, fileName);
            var metadataFilePath = Path.Combine(_uploadPath, Path.GetFileNameWithoutExtension(fileName) + ".json");

            if (System.IO.File.Exists(imageFilePath) || System.IO.File.Exists(metadataFilePath))
            {
                return BadRequest("A file with the same name already exists. Overrite is not allowed");
            }

            try
            {
                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var metadata = new ImageMetadata
                {
                    Id = Guid.NewGuid().ToString(),
                    FileName = fileName,
                    Owner = owner ?? string.Empty,
                    UploadedTime = DateTime.UtcNow,
                    LastModifiedTime = DateTime.UtcNow
                };

                var metadataJson = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(metadataFilePath, metadataJson);
                return CreatedAtAction(nameof(UploadImage), new { id = metadata.Id }, metadata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
