using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderFoodApi_DXC.Data
{
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        public int Id { set; get; }
        [Required]
        [MaxLength(50)]
        public string? UserName { set; get; }
        [Required]
        [MaxLength(50)]
        public string? Password { set; get; }
        [MaxLength(50)]
        public string? Email { set; get; }
        [MaxLength(50)]
        public string? HoTen { set; get; }
    }
}
