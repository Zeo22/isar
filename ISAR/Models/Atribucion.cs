using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace ISAR.Models
{
    [Table("Atribuciones")]
    public class Atribucion
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Atribución")]
        public string Descripcion { get; set; }
        [ScriptIgnore]
        public virtual Area Area { get; set; }
    }
}