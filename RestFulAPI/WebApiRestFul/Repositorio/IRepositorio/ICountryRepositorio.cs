using WebApiRestFul.Modelos;

namespace WebApiRestFul.Repositorio.IRepositorio
{
    public interface ICountryRepositorio:IRepositorio<Country>
    {
        Task<Country> Actualizar(Country Entidad);
    }
}
