using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiRestFul.Datos;
using WebApiRestFul.Modelos.DTO;

namespace WebApiRestFul.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;

        public CountryController(ILogger<CountryController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CountryDTO>> GetCountrys()
        {
            _logger.LogInformation("Obtener los paises");
            return Ok (CountryStore.countryList);

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
            var country = CountryStore.countryList.FirstOrDefault(m => m.Id == id);
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
            if (CountryStore.countryList.FirstOrDefault(m => m.Nombre.ToLower() == countryDTO.Nombre.ToLower()) != null)
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
            countryDTO.Id = CountryStore.countryList.OrderByDescending(m => m.Id).FirstOrDefault().Id + 1 ;
            CountryStore.countryList.Add(countryDTO);

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
            var country = CountryStore.countryList.FirstOrDefault(s=>s.Id==id);

            if (country == null)
            {

                return NotFound();
            }

            CountryStore.countryList.Remove(country);

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
            var country = CountryStore.countryList.FirstOrDefault(d => d.Id == id);
            country.Nombre=countryDTO.Nombre;
            country.Habitantes = countryDTO.Habitantes;
            country.Area = countryDTO.Area;


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
            var country = CountryStore.countryList.FirstOrDefault(d => d.Id == id);

            PatchCountryDTO.ApplyTo(country,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();

        }





    }
}
