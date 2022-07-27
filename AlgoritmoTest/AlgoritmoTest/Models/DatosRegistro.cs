using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoTest.Models
{
    public class DatosRegistro
    {
        public int Id { get; set; }

        public string Plantilla { get; set; }

        public string Posicion { get; set; }

        public DatosRegistro(int v1, string v2, string v3)
        {
            this.Id = v1;
            this.Plantilla = v2;
            this.Posicion = v3;
        }
         
    }
}
