using Lab2.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly HouseDbContext _db;

        public AddressController(HouseDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Address>> GetAddressesList()
        {
            var addresses = _db.Addresses.ToList();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public ActionResult<Address> GetAddressById(int id)
        {
            var address = _db.Addresses.FirstOrDefault(address => address.Id == id);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }

        [HttpGet("HouseId/{houseId}")]
        public ActionResult<IEnumerable<Address>> GetAddressesByHouseId(int houseId)
        {
            var addresses = _db.Addresses.Where(address => address.HouseId == houseId).ToList();
            if (!addresses.Any())
            {
                return NotFound();
            }
            return Ok(addresses);
        }

        [HttpPost]
        public IActionResult CreateAddress([FromBody] Address address)
        {
            if (address == null)
            {
                return BadRequest("Address data is null");
            }

            _db.Addresses.Add(address);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, [FromBody] Address updatedAddress)
        {
            if (updatedAddress == null)
            {
                return BadRequest("Address data is null");
            }

            var existingAddress = _db.Addresses.FirstOrDefault(address => address.Id == id);
            if (existingAddress == null)
            {
                return NotFound();
            }

            existingAddress.Street = updatedAddress.Street;
            existingAddress.City = updatedAddress.City;
            existingAddress.PostalCode = updatedAddress.PostalCode;
            existingAddress.Country = updatedAddress.Country;
            existingAddress.Notes = updatedAddress.Notes;

            _db.SaveChanges();

            return Ok(existingAddress);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var address = _db.Addresses.FirstOrDefault(address => address.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            _db.Addresses.Remove(address);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
