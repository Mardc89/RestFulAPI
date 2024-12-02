using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiRestFul.Datos;
using WebApiRestFul.Modelos.DTO;

namespace WebApiRestFul.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CountryDTO>> GetCountrys()
        {
            return Ok (CountryStore.countryList);

        }

        [HttpGet("id:int")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CountryDTO> GetCountry(int id)
        {
            if (id == 0)
            {
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CountryDTO> CrearCountry([FromBody] CountryDTO countryDTO)
        {
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

            return Ok(countryDTO);

        }




    }
}
