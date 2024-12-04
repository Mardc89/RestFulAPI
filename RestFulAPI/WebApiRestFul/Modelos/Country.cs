using System.ComponentModel.DataAnnotations;

namespace WebApiRestFul.Modelos
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
