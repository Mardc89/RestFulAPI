using AutoMapper;
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
        private readonly IMapper _mapper;

        public CountryController(ILogger<CountryController> logger, ApplicationDbContext db,IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CountryDTO>>> GetCountrys()
        {
            _logger.LogInformation("Obtener los paises");
            IEnumerable<Country> countryList = await _db.Countries.ToListAsync();
            return Ok (_mapper.Map<IEnumerable<CountryDTO>>(countryList));

        }

        [HttpGet("id:int",Name="GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al obtener Country con Id " + id);
                return BadRequest();
            }
            var country = await _db.Countries.FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();

            }

            return Ok(_mapper.Map<CountryDTO>(country));

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CountryDTO>> CrearCountry([FromBody] CountryCreateDTO countryDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _db.Countries.FirstOrDefaultAsync(m => m.Nombre.ToLower() == countryDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("Existe","Ya Existe ese Nombre");
                return BadRequest(ModelState);
            }

            if (countryDTO == null)
            {
                return BadRequest(countryDTO);
            }


           Country modelo=_mapper.Map<Country>(countryDTO);

            await _db.AddAsync(modelo);
            await _db.SaveChangesAsync();


            return CreatedAtRoute("GetCountry",new { id=modelo.Id},modelo);

        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarCountry(int id)
        {
            if (id==0)
            {
                return BadRequest();
            }
            var country = await _db.Countries.FirstOrDefaultAsync(s=>s.Id==id);

            if (country == null)
            {

                return NotFound();
            }

            _db.Countries.Remove(country);
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCountry(int id,[FromBody]CountryUpdateDTO countryDTO)
        {
            if (countryDTO==null || id!=countryDTO.Id)
            {
                return BadRequest();
            }

            Country modelo = _mapper.Map<Country>(countryDTO);


             _db.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialCountry(int id, JsonPatchDocument<CountryUpdateDTO> PatchCountryDTO)
        {
            if (PatchCountryDTO == null || id==0)
            {
                return BadRequest();
            }
            var country = await _db.Countries.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            CountryUpdateDTO countryDTO =_mapper.Map<CountryUpdateDTO>(country);

            if (country == null) return BadRequest();

            PatchCountryDTO.ApplyTo(countryDTO,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country modelo = _mapper.Map<Country>(countryDTO);

            _db.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();

        }





    }
}
