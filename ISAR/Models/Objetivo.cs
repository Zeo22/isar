using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISAR.Models
{
    [Table("TipoObjetivo")]
    public class TipoObjetivo
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public virtual List<Objetivo> Objetivos { get; set; }
    }

    [Table("CategoriaObjetivo")]
    public class CategoriaObjetivo
    {
        public int ID { get; set; }
        public virtual NivelOrganizacional Nivel { get; set; }
        public string Nombre { get; set; }
        public virtual List<Objetivo> Objetivos { get; set; }
    }

    [Table("TipoAlineacion")]
    public class TipoAlineacion
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public virtual List<Objetivo> Objetivos { get; set; }
    }

    [Table("Objetivos")]
    public class Objetivo
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Display(Name = "Tipo Objetivo")]
        public virtual TipoObjetivo Tipo { get; set; }
        [Display(Name = "Categoría Balanced Scorecard")]
        public virtual CategoriaObjetivo Categoria { get; set; }
        [Display(Name = "Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Fecha Final")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinal { get; set; }
        [Display(Name = "Área")]
        public virtual Area Area { get; set; }
        [Display(Name = "Responsable")]
        public virtual ApplicationUser Responsable { get; set; }
        [Display(Name = "Alineación")]
        public virtual List<Atribucion> Atribuciones { get; set; }
        public virtual TipoAlineacion Alineacion { get; set; }
        public virtual List<Objetivo> ObjetivosAlineados { get; set; } // Los hijos
        public virtual List<Estrategia> Estrategias { get; set; }
        public virtual Periodo Periodo { get; set; }
    }
}