using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace ISAR.Models
{
    public class Area
    {
        public int ID { get; set; }
        [Display(Name = "Área")]
        public string Nombre { get; set; }
        [Display(Name = "Área Padre")]
        public virtual Area AreaPadre { get; set; }
        [Display(Name="Nivel Organizacional")]
        public virtual NivelOrganizacional Nivel { get; set; }
        public virtual List<Area> AreasHijas { get; set; }
        public virtual List<Atribucion> Atribuciones { get; set; }
        public virtual List<ApplicationUser> Usuarios { get; set; }

        public bool PuedeEliminar()
        {
            return !(this.AreasHijas.Count() > 0 || this.Atribuciones.Count() > 0 || this.Usuarios.Count() > 0);
        }
    }
}