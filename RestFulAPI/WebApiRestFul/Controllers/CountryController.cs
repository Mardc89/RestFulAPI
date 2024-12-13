using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApiRestFul.Datos;
using WebApiRestFul.Modelos;
using WebApiRestFul.Modelos.DTO;
using WebApiRestFul.Repositorio.IRepositorio;

namespace WebApiRestFul.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryRepositorio _countryRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public CountryController(ILogger<CountryController> logger,ICountryRepositorio countryRepositorio,IMapper mapper)
        {
            _logger = logger;
            _countryRepositorio = countryRepositorio;
            _mapper = mapper;
            _response = new(); 
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCountry()
        {
            try
            {
                _logger.LogInformation("Obtener los paises");
                IEnumerable<Country> countryList = await _countryRepositorio.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<CountryDTO>>(countryList);
                _response.statusCode=HttpStatusCode.OK;
                return Ok (_response);
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMesages = new List<string>() { ex.ToString() };
              
            }
            return _response;

        }

        [HttpGet("id:int",Name="GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCountry(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al obtener Country con Id " + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExistoso = false;
                    return BadRequest(_response);
                }
                var country = await _countryRepositorio.Obtener(m => m.Id == id);
                if (country == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExistoso = false;
                    return NotFound(_response);

                }
                _response.Resultado = _mapper.Map<CountryDTO>(country);
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExistoso=false;
                _response.ErrorMesages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearCountry([FromBody] CountryCreateDTO countryDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _countryRepositorio.Obtener(m => m.Nombre.ToLower() == countryDTO.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("Existe","Ya Existe ese Nombre");
                    return BadRequest(ModelState);
                }

                if (countryDTO == null)
                {
                    return BadRequest(countryDTO);
                }


               Country modelo=_mapper.Map<Country>(countryDTO);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _countryRepositorio.Crear(modelo);
                _response.Resultado= modelo;
                _response.statusCode = HttpStatusCode.Created;
    
                return CreatedAtRoute("GetCountry",new { id=modelo.Id},_response);
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMesages = new List<string>() { ex.ToString() };

            }

            return _response;


        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EliminarCountry(int id)
        {
            try
            {
                if (id==0)
                {
                    _response.IsExistoso = false;
                    _response.statusCode= HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var country = await _countryRepositorio.Obtener(s=>s.Id==id);

                if (country == null)
                {
                    _response.IsExistoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _countryRepositorio.Remover(country);
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExistoso = false;
                _response.ErrorMesages = new List<string>() { ex.ToString() };

            }
            return BadRequest(_response);


        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCountry(int id,[FromBody]CountryUpdateDTO countryDTO)
        {
            if (countryDTO==null || id!=countryDTO.Id)
            {
                _response.IsExistoso = false;
                _response.statusCode=HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Country modelo = _mapper.Map<Country>(countryDTO);


            await _countryRepositorio.Actualizar(modelo);
            _response.statusCode = HttpStatusCode.NoContent;
   

            return Ok(_response);

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
            var country = await _countryRepositorio.Obtener(d => d.Id == id,tracked:false);

            CountryUpdateDTO countryDTO =_mapper.Map<CountryUpdateDTO>(country);

            if (country == null) return BadRequest();

            PatchCountryDTO.ApplyTo(countryDTO,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country modelo = _mapper.Map<Country>(countryDTO);

           await _countryRepositorio.Actualizar(modelo);
            _response.statusCode=HttpStatusCode.NoContent;
            return Ok(_response);

        }





    }
}
