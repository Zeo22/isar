using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISAR.Models
{
    [Table("Periodos")]
    public class Periodo
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; }
        [Display(Name = "Activo / Inactivo")]
        public bool Activo { get; set; }
        public virtual List<Estrategia> Estrategias { get; set; }
        public virtual List<Objetivo> Objetivos { get; set; }

        public bool PuedeEliminar()
        {
            return !(this.Estrategias.Count() > 0 || this.Objetivos.Count() > 0);
        }
    }
}