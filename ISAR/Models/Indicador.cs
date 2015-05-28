using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISAR.Models
{
    [Table("IndicadoresComportamiento")]
    public class Comportamiento
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }

    [Table("TipoIndicador")]
    public class TipoIndicador
    {
        public int ID { get; set; }
        public virtual NivelOrganizacional Nivel { get; set; }
        public string Nombre { get; set; }

        public virtual List<Indicador> Indicadores { get; set; }
    }

    [Table("FrecuenciaMedicion")]
    public class FrecuenciaMedicion
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }

    [Table("Umbrales")]
    public class Umbral
    {
        public int ID { get; set; }
        public float Desde { get; set; }
        public float Hasta { get; set; }
    }

    [Table("Indicadores")]
    public class Indicador
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Required]
        [Display(Name = "Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }
        [Required]
        [Display(Name = "Fecha Final")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinal { get; set; }
        [Display(Name = "Unidad de Medida")]
        public virtual UnidadDeMedida UnidadDeMedida { get; set; }
        public virtual Comportamiento Comportamiento { get; set; }
        [Display(Name = "Tipo Indicador")]
        public virtual TipoIndicador Tipo { get; set; }
        [Display(Name = "Frecuencia de Medición")]
        public virtual FrecuenciaMedicion Frecuencia { get; set; }
        [Display(Name = "Fórmula")]
        public string Formula { get; set; }
        [Display(Name = "Fuente de Información")]
        public string FuenteInformacion { get; set; }
        public virtual Area Area { get; set; }
        public virtual ApplicationUser Responsable { get; set; }
        public virtual Umbral UmbralVerde { get; set; }
        public virtual Umbral UmbralAmarillo { get; set; }
        public virtual Umbral UmbralRojo { get; set; }
        public virtual List<MetaIndicador> Metas { get; set; }
        public virtual List<Periodo> Periodos { get; set; }

        public bool PuedeEliminar()
        {
            return !(this.Metas.Count() > 0);
        }
    }
}