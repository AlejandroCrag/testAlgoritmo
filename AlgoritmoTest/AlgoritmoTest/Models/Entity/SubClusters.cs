using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoTest.Models.Entity
{
    class SubClusters
    {
        public SubClusters(int id, string contenedor, List<Clientes> clientes)
        {
            Id = id;
            Contenedor = contenedor;
            Clientes = clientes;
        }

        public int Id{ get; set; }
        public String Contenedor{ get; set; }
        public List<Clientes> Clientes { get; set; }

    }
}
