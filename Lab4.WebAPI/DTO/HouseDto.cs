using System.Collections.Generic;

namespace Lab4.WebAPI.DTO
{
    public class HouseDto
    {
        public int Id { get; set; }
        public int YearBuilt { get; set; }
        public string Owner { get; set; }
        public double Area { get; set; }
        public int Floors { get; set; }
        public List<AddressDto> Addresses { get; set; }
        public List<GarageDto> Garages { get; set; }
    }
}
