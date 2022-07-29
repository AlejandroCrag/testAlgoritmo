using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoTest.Models
{
    class SubCluster
    {
        private int id;

        public SubCluster(int id, int Clouster_Id, List<DatosRegistro> cU)
        {
            this.id = id;
            this.GrupoRegistros = cU;
            this.Clouster_Id = Clouster_Id;
        }

        public int Id { get; set; }
        public int Clouster_Id { get; set; }
        public List<DatosRegistro> GrupoRegistros { get; set; }

    }
}
