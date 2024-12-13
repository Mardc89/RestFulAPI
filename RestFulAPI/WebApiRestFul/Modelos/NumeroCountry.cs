using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiRestFul.Modelos
{
    public class NumeroCountry
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CountryNo { get; set; }

        [Required]
        public string CountryId { get; set; }

        [ForeignKey("CountryId")]

        public Country Country { get; set; }

        public string DetalleEspecial { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

    }
}
