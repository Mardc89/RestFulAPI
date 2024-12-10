﻿using System.ComponentModel.DataAnnotations;

namespace WebApiRestFul.Modelos.DTO
{
    public class CountryUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }
        [Required]
        public int Habitantes { get; set; }
        public int Area { get; set; }
        [Required]
        public string ImagenUrl { get; set; }

    }
}
