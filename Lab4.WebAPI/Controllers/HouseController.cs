using Lab2.DataAccess;
using Lab4.WebAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab4.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private readonly HouseDbContext _db;

        public HouseController(HouseDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HouseDto>> GetHousesList()
        {
            var houses = _db.Houses
                .Include(house => house.Addresses)
                .Include(house => house.Garages)
                .Select(house => new HouseDto
                {
                    Id = house.Id,
                    YearBuilt = house.YearBuilt,
                    Owner = house.Owner,
                    Area = house.Area,
                    Floors = house.Floors,
                    Addresses = house.Addresses.Select(address => new AddressDto
                    {
                        Street = address.Street,
                        City = address.City,
                        PostalCode = address.PostalCode,
                        Country = address.Country,
                        Notes = address.Notes
                    }).ToList(),
                    Garages = house.Garages.Select(garage => new GarageDto
                    {
                        Type = garage.Type,
                        Size = garage.Size
                    }).ToList()
                })
                .ToList();

            return Ok(houses);
        }

        [HttpGet("{id}")]
        public ActionResult<HouseDto> GetHouseById(int id)
        {
            var house = _db.Houses
                .Include(house => house.Addresses)
                .Include(house => house.Garages)
                .FirstOrDefault(house => house.Id == id);

            if (house == null)
            {
                return NotFound();
            }

            var houseDto = new HouseDto
            {
                Id = house.Id,
                YearBuilt = house.YearBuilt,
                Owner = house.Owner,
                Area = house.Area,
                Floors = house.Floors,
                Addresses = house.Addresses.Select(address => new AddressDto
                {
                    Street = address.Street,
                    City = address.City,
                    PostalCode = address.PostalCode,
                    Country = address.Country,
                    Notes = address.Notes
                }).ToList(),
                Garages = house.Garages.Select(garage => new GarageDto
                {
                    Type = garage.Type,
                    Size = garage.Size
                }).ToList()
            };

            return Ok(houseDto);
        }

        [HttpPost]
        public IActionResult CreateHouse([FromBody] HouseDto houseDto)
        {
            if (houseDto == null)
            {
                return BadRequest("House data is null.");
            }

            var house = new House
            {
                YearBuilt = houseDto.YearBuilt,
                Owner = houseDto.Owner,
                Area = houseDto.Area,
                Floors = houseDto.Floors
            };

            if (houseDto.Addresses != null && houseDto.Addresses.Count > 0)
            {
                house.Addresses = houseDto.Addresses.Select(addressDto => new Address
                {
                    Street = addressDto.Street,
                    City = addressDto.City,
                    PostalCode = addressDto.PostalCode,
                    Country = addressDto.Country,
                    Notes = addressDto.Notes
                }).ToList();
            }

            if (houseDto.Garages != null && houseDto.Garages.Count > 0)
            {
                house.Garages = houseDto.Garages.Select(garageDto => new Garage
                {
                    Type = garageDto.Type,
                    Size = garageDto.Size
                }).ToList();
            }

            _db.Houses.Add(house);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetHouseById), new { id = house.Id }, houseDto);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateHouse(int id, [FromBody] HouseDto houseDto)
        {
            var existingHouse = _db.Houses
                .Include(house => house.Addresses)
                .Include(house => house.Garages)
                .FirstOrDefault(house => house.Id == id);

            if (existingHouse == null)
            {
                return NotFound($"House with ID {id} not found.");
            }

            existingHouse.Owner = houseDto.Owner;
            existingHouse.YearBuilt = houseDto.YearBuilt;
            existingHouse.Area = houseDto.Area;
            existingHouse.Floors = houseDto.Floors;

            if (houseDto.Addresses != null)
            {
                _db.Addresses.RemoveRange(existingHouse.Addresses);

                existingHouse.Addresses = houseDto.Addresses.Select(addressDto => new Address
                {
                    Street = addressDto.Street,
                    City = addressDto.City,
                    PostalCode = addressDto.PostalCode,
                    Country = addressDto.Country,
                    Notes = addressDto.Notes
                }).ToList();
            }

            if (houseDto.Garages != null)
            {
                _db.Garages.RemoveRange(existingHouse.Garages);

                existingHouse.Garages = houseDto.Garages.Select(garageDto => new Garage
                {
                    Type = garageDto.Type,
                    Size = garageDto.Size
                }).ToList();
            }

            _db.SaveChanges();

            var updatedHouseDto = new HouseDto
            {
                Id = existingHouse.Id,
                YearBuilt = existingHouse.YearBuilt,
                Owner = existingHouse.Owner,
                Area = existingHouse.Area,
                Floors = existingHouse.Floors,
                Addresses = existingHouse.Addresses.Select(address => new AddressDto
                {
                    Street = address.Street,
                    City = address.City,
                    PostalCode = address.PostalCode,
                    Country = address.Country,
                    Notes = address.Notes
                }).ToList(),
                Garages = existingHouse.Garages.Select(garage => new GarageDto
                {
                    Type = garage.Type,
                    Size = garage.Size
                }).ToList()
            };

            return Ok(updatedHouseDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHouse(int id)
        {
            var house = _db.Houses.FirstOrDefault(house => house.Id == id);
            if (house == null)
            {
                return NotFound();
            }

            _db.Houses.Remove(house);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
