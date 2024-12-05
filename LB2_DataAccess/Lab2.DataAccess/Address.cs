using System.ComponentModel.DataAnnotations;

namespace Lab2.DataAccess
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Street { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        public int HouseId { get; set; }

    }
}
