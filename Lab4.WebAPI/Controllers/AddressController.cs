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
                return NotFound();

            return Ok(address);
        }

        [HttpGet("HouseId/{houseId}")]
        public ActionResult<IEnumerable<Address>> GetAddressesByHouseId(int houseId)
        {
            var addresses = _db.Addresses.Where(address => address.HouseId == houseId).ToList();

            if (!addresses.Any())
                return NotFound();

            return Ok(addresses);
        }

        [HttpPost]
        public IActionResult CreateAddress([FromBody] Address address)
        {
            if (address == null)
                return BadRequest("Address data is null");

            _db.Addresses.Add(address);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, [FromBody] Address updatedAddress)
        {
            if (updatedAddress == null)
                return BadRequest("Address data is null");

            var existingAddress = _db.Addresses.FirstOrDefault(address => address.Id == id);

            if (existingAddress == null)
                return NotFound();

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
                return NotFound();

            _db.Addresses.Remove(address);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpGet("Search")]
        public ActionResult<IEnumerable<Address>> SearchAddresses(string? street, string? city, string? postalCode, string? country, string? notes)
        {
            var query = _db.Addresses.AsQueryable();

            var filters = new Dictionary<string, Func<string?, IQueryable<Address>, IQueryable<Address>>>
                {
                    { "street", (value, query) => query.Where(address => address.Street.Contains(value)) },
                    { "city", (value, query) => query.Where(address => address.City.Contains(value)) },
                    { "postalCode", (value, query) => query.Where(address => address.PostalCode.Contains(value)) },
                    { "country", (value, query) => query.Where(address => address.Country.Contains(value)) },
                    { "notes", (value, query) => query.Where(address => address.Notes.Contains(value)) }
                };

            var parameters = new Dictionary<string, string?>
                {
                    { "street", street },
                    { "city", city },
                    { "postalCode", postalCode },
                    { "country", country },
                    { "notes", notes }
                };

            foreach (var param in parameters)
            {
                if (!string.IsNullOrEmpty(param.Value) && filters.ContainsKey(param.Key))
                    query = filters[param.Key](param.Value, query);
            }

            var addresses = query.ToList();

            if (!addresses.Any())
                return NotFound("No addresses found.");

            return Ok(addresses);
        }
    }
}
