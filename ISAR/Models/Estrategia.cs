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
        public string Titulo { get; set; }
        public List<Objetivo> ObjetivoAlineado { get; set; }
    }
}