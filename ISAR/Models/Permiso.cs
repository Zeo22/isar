using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISAR.Models
{
    [Table("Permisos")]
    public class Permiso
    {
        public int ID { get; set; }
        public virtual ApplicationRole Rol { get; set; }
        public virtual Pantalla Pantalla { get; set; }
        public bool Lectura { get; set; }
        public bool Escritura { get; set; }
    }
}