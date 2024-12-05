using System.ComponentModel.DataAnnotations;

namespace Lab2.DataAccess
{
    public class House
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int YearBuilt { get; set; }

        [Required]
        [StringLength(100)]
        public string Owner { get; set; }

        [Required]
        public double Area { get; set; }

        [Required]
        public int Floors { get; set; }

        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<Garage> Garages { get; set; } = new List<Garage>();
    }
}
