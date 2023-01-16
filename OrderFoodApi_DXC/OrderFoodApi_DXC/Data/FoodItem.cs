using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderFoodApi_DXC.Api.Data
{
    [Table("FoodItem")]
    public class FoodItem
    {
        [Key]
        public int FoodId { get; set; } 

        [Column(TypeName = "nvarchar(50)")]
        public string? Title { get; set; }

        public string? Desc { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; } = null!;

        [NotMapped]
        public string? ImageSrc { get; set; }
    }
}
