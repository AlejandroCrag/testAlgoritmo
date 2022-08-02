using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoTest.Models.Entity
{
    class SubClusters
    {
        public SubClusters(int id, int estatus, string contenedor, List<Clientes> clientes)
        {
            Id = id;
            Estatus = estatus;
            Contenedor = contenedor;
            Clientes = clientes;
        }
        public int Id { get; set; }
        public int Estatus { get; set; }
        public String Contenedor{ get; set; }
        public List<Clientes> Clientes { get; set; }

    }
}
