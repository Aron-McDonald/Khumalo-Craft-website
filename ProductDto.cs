using System.ComponentModel.DataAnnotations;

namespace CLDV_POE_PART_2.Models
{
    public class ProductDto
    {
        [Required] 
        public string Name { get; set; } = "";

        [Required] 
        public string Price { get; set; } = "";

        [Required] 
        public string Category { get; set; } = "";

        public IFormFile? ImageFile { get; set; }

        [Required] 
        public string Availability { get; set; } = "";
    }
}
