using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TexodeAPI
{
    public class MobileCard
    {
        [JsonPropertyName("ID")]
        public Guid ID { get; set; }
        [JsonPropertyName("Name")]
        [Required]
        public string Name { get; set; }
        [JsonPropertyName("Image")]
        [Required]
        public string Image { get; set; }
    }
}
