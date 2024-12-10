using System.ComponentModel.DataAnnotations;

namespace WebApiRestFul.Modelos.DTO
{
    public class CountryCreateDTO
    {

        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        public int Habitantes { get; set; }
        public int Area { get; set; }

        public string ImagenUrl { get; set; }

    }
}
