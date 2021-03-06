﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISAR.Models
{
    [Table("UnidadesDeMedida")]
    public class UnidadDeMedida
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Unidad")]
        public string Nombre { get; set; }
        [Required]
        public string Abreviatura { get; set; }

        public bool PuedeEliminar()
        {
            return true;
        }
    }
}