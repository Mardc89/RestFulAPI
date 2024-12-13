using WebApiRestFul.Datos;
using WebApiRestFul.Modelos;
using WebApiRestFul.Repositorio.IRepositorio;

namespace WebApiRestFul.Repositorio
{
    public class CountryRepositorio : Repositorio<Country>, ICountryRepositorio
    {
        private readonly ApplicationDbContext _db;
        public CountryRepositorio(ApplicationDbContext db):base(db) 
        {
            _db=db;
        }
        public async Task<Country> Actualizar(Country entidad)
        {
            entidad.FechaActualizacion=DateTime.Now;
            _db.Countries.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
