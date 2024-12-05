using Lab2.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarageController : ControllerBase
    {
        private readonly HouseDbContext _db;

        public GarageController(HouseDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Garage>> GetGaragesList()
        {
            var garages = _db.Garages.ToList();
            return Ok(garages);
        }

        [HttpGet("{id}")]
        public ActionResult<Garage> GetGarageById(int id)
        {
            var garage = _db.Garages.FirstOrDefault(garage => garage.Id == id);
            if (garage == null)
            {
                return NotFound();
            }
            return Ok(garage);
        }

        [HttpGet("HouseId/{houseId}")]
        public ActionResult<IEnumerable<Garage>> GetGaragesByHouseId(int houseId)
        {
            var garages = _db.Garages.Where(garage => garage.HouseId == houseId).ToList();
            if (garages == null || !garages.Any())
            {
                return NotFound();
            }
            return Ok(garages);
        }

        [HttpPost]
        public IActionResult CreateGarage([FromBody] Garage garage)
        {
            if (garage == null)
            {
                return BadRequest("Garage data is null");
            }

            _db.Garages.Add(garage);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetGarageById), new { id = garage.Id }, garage);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGarage(int id, [FromBody] Garage updatedGarage)
        {
            if (updatedGarage == null)
            {
                return BadRequest("Garage data is null");
            }

            var existingGarage = _db.Garages.FirstOrDefault(garage => garage.Id == id);
            if (existingGarage == null)
            {
                return NotFound();
            }

            existingGarage.Type = updatedGarage.Type;
            existingGarage.Size = updatedGarage.Size;
            existingGarage.HouseId = updatedGarage.HouseId;

            _db.SaveChanges();

            return Ok(existingGarage);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGarage(int id)
        {
            var garage = _db.Garages.FirstOrDefault(garage => garage.Id == id);
            if (garage == null)
            {
                return NotFound();
            }

            _db.Garages.Remove(garage);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
