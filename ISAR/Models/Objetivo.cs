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
    }

    [Table("CategoriaObjetivo")]
    public class CategoriaObjetivo
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }

    [Table("TipoAlineacion")]
    public class TipoAlineacion
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }

    [Table("Objetivos")]
    public class Objetivo
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public TipoObjetivo Tipo { get; set; }
        public CategoriaObjetivo Categoria { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public Area Area { get; set; }
        public ApplicationUser Responsable { get; set; }
        public TipoAlineacion Alineacion { get; set; }
        public List<Objetivo> ObjetivosAlineados { get; set; }
    }
}