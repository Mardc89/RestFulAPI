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
        public IEnumerable<CountryDTO> GetCountrys()
        {
            return CountryStore.countryList;

        }

        [HttpGet("id:int")]
        public CountryDTO GetCountry(int id)
        {
            return CountryStore.countryList.FirstOrDefault(m=>m.Id==id);

        }




    }
}
