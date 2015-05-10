using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
    }
}