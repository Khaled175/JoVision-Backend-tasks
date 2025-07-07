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
    public class ImageDeleteController : ControllerBase
    {
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public ImageDeleteController()
        {
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(
            [FromQuery] string fileName,
            [FromQuery] string fileOwner)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest("File name is required.");
            }

            if (string.IsNullOrWhiteSpace(fileOwner))
            {
                return BadRequest("File owner is required for deletion verification.");
            }

            var imageFilePath = Path.Combine(_uploadPath, Path.GetFileName(fileName));
            var metadataFilePath = Path.Combine(_uploadPath, Path.GetFileNameWithoutExtension(fileName) + ".json");

            if (!System.IO.File.Exists(imageFilePath) || !System.IO.File.Exists(metadataFilePath))
            {
                return NotFound($"File '{fileName}' or its metadata not found.");
            }

            try
            {
                var JsonMetadata = await System.IO.File.ReadAllTextAsync(metadataFilePath);
                var metadata = JsonSerializer.Deserialize<ImageMetadata>(JsonMetadata);

                if (metadata == null || !string.Equals(metadata.Owner, fileOwner, StringComparison.OrdinalIgnoreCase))
                {
                    return Forbid ("You are not authorized to delete this file.");
                }

                System.IO.File.Delete(imageFilePath);
                System.IO.File.Delete(metadataFilePath);

                return NoContent(); // 204 No Content
            }
            catch (FileNotFoundException)
            {
                return NotFound($"File '{fileName}' or its metadata not found during deletion attempt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
