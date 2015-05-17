using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ISAR.Models;

namespace ISAR.Models
{
    [Table(("Puestos"))]
    public class Puesto
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Puesto")]
        public string Nombre { get; set; }
        public virtual List<ApplicationUser> Usuarios { get; set; }

        public bool PuedeEliminar()
        {
            return !(this.Usuarios.Count() > 0);
        }
    }
}