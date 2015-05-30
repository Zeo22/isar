using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISAR.Models
{
    [Table("MetasIndicadores")]
    public class MetaIndicador
    {
        public int ID { get; set; }
        public virtual Indicador Indicador { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; }
        public float Meta { get; set; }
        public float Resultado { get; set; }
        public float Cumplimiento { get; set; }
        public bool MetaCerrada { get; set; }
        public bool ResultadoCerrado { get; set; }
    }
}