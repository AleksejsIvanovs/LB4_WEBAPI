using System.ComponentModel.DataAnnotations;

namespace Lab2.DataAccess
{
    public class Garage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; } 

        [Required]
        public double Size { get; set; }
        public int HouseId { get; set; }
    }
}
