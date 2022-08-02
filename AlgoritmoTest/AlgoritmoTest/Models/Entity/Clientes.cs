using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoTest.Models.Entity
{
    class Clientes
    {
        public Clientes(int pais, int canal, int sucursal, int folio, List<Fingerprint> fingerprints)
        {
            this.Pais = pais;
            this.Canal = canal;
            this.Sucursal = sucursal;
            this.Folio = folio;
            this.Fingerprints = fingerprints;
        }

        public int Pais {get; set;}
        public int Canal { get; set; }
        public int Sucursal { get; set; }
        public int Folio { get; set; }
        public List<Fingerprint> Fingerprints { get; set; }
    }
}