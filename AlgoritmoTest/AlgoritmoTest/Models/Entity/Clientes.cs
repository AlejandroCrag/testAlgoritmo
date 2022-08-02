using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoTest.Models.Entity
{
    public class Clientes
    {
        Clientes(int pais, int canal, int sucursal, int folio, List<Huellas> huellas)
        {
            this.Pais = pais;
            this.Canal = canal;
            this.Sucursal = sucursal;
            this.Folio = folio;
            this.Huellas = huellas;
        }

        public int Pais {get; set;}
        public int Canal { get; set; }
        public int Sucursal { get; set; }
        public int Folio { get; set; }
        public List<Huellas> Huellas { get; set; }
    }
}