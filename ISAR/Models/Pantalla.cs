using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISAR.Models
{
    [Table("GrupoPantalla")]
    public class GrupoPantalla
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public virtual List<Pantalla> Pantallas { get; set; }
    }

    [Table("Pantallas")]
    public class Pantalla
    {
        public int ID { get; set; }
        public virtual GrupoPantalla Grupo { get; set; }
        public virtual List<Permiso> Permisos { get; set; }
        public string URL { get; set; }
        public string Nombre { get; set; }
    }
}