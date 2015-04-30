using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISAR.Models
{
    [Table("NivelOrganizacional")]
    public class NivelOrganizacional
    {
        public int ID { get; set; }
        [Display(Name = "Nivel Organizacional")]
        public string Nombre { get; set; }
    }
}