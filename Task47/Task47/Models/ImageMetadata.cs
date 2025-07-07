using System;
using System.Text.Json.Serialization;

namespace Task47.Models
{
    public class ImageMetadata
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string FileName { get; set; } = string.Empty;

        public string Owner { get; set; } = string.Empty;

        public DateTime UploadedTime { get; set; } = DateTime.UtcNow;

        public DateTime LastModifiedTime { get; set; } = DateTime.UtcNow;
    }
}
