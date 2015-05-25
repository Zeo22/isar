using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISAR.Models
{
    [Table("Estrategias")]
    public class Estrategia
    {
        public int ID { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Display(Name = "Objetivo")]
        public virtual List<Objetivo> ObjetivoAlineado { get; set; }
        public virtual List<Periodo> Periodos { get; set; }

        public bool PuedeEliminar()
        {
            return true;
        }
    }
}