using WebApiRestFul.Modelos.DTO;

namespace WebApiRestFul.Datos
{
    public static class  CountryStore
    {
        public static List<CountryDTO> countryList = new List<CountryDTO>
        {
            new CountryDTO{Id=1,Nombre="Peru"},
            new CountryDTO{Id=2,Nombre="Colombia"}
        };
    }
}
