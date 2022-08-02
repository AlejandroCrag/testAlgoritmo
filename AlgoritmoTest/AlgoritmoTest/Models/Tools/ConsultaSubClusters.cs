using AlgoritmoTest.Models.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlgoritmoTest.Models.Tools
{
    class ConsultaSubClusters
    {
        internal List<SubClusters> subClusters;
        public List<Clientes> ClientesFaltantes { get; internal set; }

        public List<SubClusters> ListadoSubClusters() {
            var resultadoConsulta = new List<SubClusters>();
            //var data = new SubCluster(0, 1, new List<DatosRegistro>());
           // resultadoConsulta.Add(data);
            return resultadoConsulta;
        }

        public void readJsonData() {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\Models\Entity\data.json");
            string sFilePath = Path.GetFullPath(sFile);

            using (StreamReader r = new StreamReader(sFilePath))
            {
                string json = r.ReadToEnd();
                var data  = JsonSerializer.Deserialize<Entity.Master>(json);
                var subClusters = new List<SubClusters>();
                List<Clientes> clientesPendientes = new List<Clientes>();
                foreach (var subcluster in data.SubClusters) {
                    if (subcluster.Contenedor == "NOMATCH")
                    {
                        clientesPendientes = subcluster.Clientes;
                    }
                    else {
                        subClusters.Add(subcluster);
                    }
                }

                this.subClusters = subClusters;
                this.ClientesFaltantes = clientesPendientes; 
            }
        }
    }
}