using AutoMapper;
using WebApiRestFul.Modelos;
using WebApiRestFul.Modelos.DTO;

namespace WebApiRestFul
{
    public class MappingConfing:Profile
    {
        public MappingConfing()
        {
            CreateMap<Country, CountryDTO>();
            CreateMap<CountryDTO, Country>();

            CreateMap<Country,CountryCreateDTO>().ReverseMap();
            CreateMap<Country,CountryUpdateDTO>().ReverseMap();
        }
    }
}




