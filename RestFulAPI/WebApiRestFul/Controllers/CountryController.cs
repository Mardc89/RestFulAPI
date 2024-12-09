using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRestFul.Datos;
using WebApiRestFul.Modelos;
using WebApiRestFul.Modelos.DTO;

namespace WebApiRestFul.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ApplicationDbContext _db;

        public CountryController(ILogger<CountryController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CountryDTO>> GetCountrys()
        {
            _logger.LogInformation("Obtener los paises");
            return Ok (_db.Countries.ToList());

        }

        [HttpGet("id:int",Name="GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CountryDTO> GetCountry(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al obtener Country con Id " + id);
                return BadRequest();
            }
            var country = _db.Countries.FirstOrDefault(m => m.Id == id);
            if (country == null)
            {
                return NotFound();

            }

            return Ok(country);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CountryDTO> CrearCountry([FromBody] CountryDTO countryDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_db.Countries.FirstOrDefault(m => m.Nombre.ToLower() == countryDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("Existe","Ya Existe ese Nombre");
                return BadRequest(ModelState);
            }

            if (countryDTO == null)
            {
                return BadRequest(countryDTO);
            }
            if (countryDTO.Id>0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }

            Country modelo = new()
            {
                Nombre = countryDTO.Nombre,
                Detalle = countryDTO.Detalle,
                ImagenUrl = countryDTO.ImagenUrl,
                Habitantes = countryDTO.Habitantes,
                Tarifa = countryDTO.Tarifa,
                Area = countryDTO.Area
            };

            _db.Add(modelo);
            _db.SaveChanges();


            return CreatedAtRoute("GetCountry",new { id=countryDTO.Id},countryDTO);

        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EliminarCountry(int id)
        {
            if (id==0)
            {
                return BadRequest();
            }
            var country = _db.Countries.FirstOrDefault(s=>s.Id==id);

            if (country == null)
            {

                return NotFound();
            }

            _db.Countries.Remove(country);
            _db.SaveChanges();

            return NoContent();

        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateCountry(int id,[FromBody]CountryDTO countryDTO)
        {
            if (countryDTO==null || id!=countryDTO.Id)
            {
                return BadRequest();
            }
            //var country = _db.Countries.FirstOrDefault(d => d.Id == id);
            //country.Nombre=countryDTO.Nombre;
            //country.Habitantes = countryDTO.Habitantes;
            //country.Area = countryDTO.Area;
            Country modelo = new()
            {
                Id = countryDTO.Id,
                Nombre = countryDTO.Nombre,
                Detalle = countryDTO.Detalle,
                ImagenUrl = countryDTO.ImagenUrl,
                Habitantes = countryDTO.Habitantes,
                Tarifa = countryDTO.Tarifa,
                Area = countryDTO.Area
            };

            _db.Update(modelo);
            _db.SaveChanges();

            return NoContent();

        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialCountry(int id, JsonPatchDocument<CountryDTO> PatchCountryDTO)
        {
            if (PatchCountryDTO == null || id==0)
            {
                return BadRequest();
            }
            var country = _db.Countries.AsNoTracking().FirstOrDefault(d => d.Id == id);

            CountryDTO countryDTO = new()
            {
                Id = country.Id,
                Nombre=country.Nombre,
                Detalle=country.Detalle,
                ImagenUrl=country.ImagenUrl,
                Habitantes=country.Habitantes,
                Tarifa=country.Tarifa,
                Area = country.Area
            };

            if (country == null) return BadRequest();

            PatchCountryDTO.ApplyTo(countryDTO,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country modelo = new()
            {
                Id = countryDTO.Id,
                Nombre = countryDTO.Nombre,
                Detalle = countryDTO.Detalle,
                ImagenUrl = countryDTO.ImagenUrl,
                Habitantes = countryDTO.Habitantes,
                Tarifa = countryDTO.Tarifa,
                Area = countryDTO.Area
            };

            _db.Update(modelo);
            _db.SaveChanges();
            return NoContent();

        }





    }
}
