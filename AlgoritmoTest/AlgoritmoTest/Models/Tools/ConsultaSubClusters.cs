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
        public List<SubCluster> ListadoSubClusters() {
            var resultadoConsulta = new List<SubCluster>();
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

                foreach (var subcluster in data.Clusters) {
                    if (subcluster.Contenedor == "NOMATCH")
                    {
                        //add to no matches
                        Console.WriteLine("ARREGLO DE FALTANTES");
                    }
                    else {
                        //add to clusters actuales
                        Console.WriteLine("ARREGLO DE CLUSTERS");
                    }
                }
                Console.WriteLine("data read");
            }
        }
    }
}