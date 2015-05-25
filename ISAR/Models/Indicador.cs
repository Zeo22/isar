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
        public string Name { get; set; }
    }

    [Table("TipoIndicador")]
    public class TipoIndicador
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    [Table("FrecuenciaMedicion")]
    public class FrecuenciaMedicion
    {
        public int ID { get; set; }
        public string Name { get; set; }
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
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public virtual UnidadDeMedida UnidadDeMedida { get; set; }
        public virtual Comportamiento Comportamiento { get; set; }
        public virtual TipoIndicador Tipo { get; set; }
        public virtual FrecuenciaMedicion Frecuencia { get; set; }
        public string Formula { get; set; }
        public string FuenteInforacion { get; set; }
        public virtual Area Area { get; set; }
        public virtual ApplicationUser Responsable { get; set; }
        public virtual Umbral UmbralVerde { get; set; }
        public virtual Umbral UmbralAmarillo { get; set; }
        public virtual Umbral UmbralRojo { get; set; }
        public virtual List<MetaIndicador> Metas { get; set; }
    }
}