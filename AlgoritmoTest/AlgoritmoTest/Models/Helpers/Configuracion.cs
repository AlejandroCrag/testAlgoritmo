using AlgoritmoTest.Models.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlgoritmoTest.Models
{
    class Configuracion
    {
        public string Nombre_Archivo { get; set; }
        public int StatusProceso { get; set; }
        public List<Clientes> ElementosFaltantesList { get; set; }
        public List<SubClusters> SubClusterActuales { get; set; }
        public List<SubClusters> SubClusterGenerados { get; internal set; }

        public void GuardarConfiguracion()
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\Models\Tools\configuracion.json");
            string fileName = Path.GetFullPath(sFile); 
            string jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(fileName, jsonString);
        }

        public Configuracion LeerConfiguracion()
        {
            try
            {
                string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string sFile = System.IO.Path.Combine(sCurrentDirectory, @"..\..\..\Models\Tools\configuracion.json");
                string sFilePath = Path.GetFullPath(sFile);

                using (StreamReader r = new StreamReader(sFilePath))
                {
                    string json = r.ReadToEnd();
                    var data = JsonSerializer.Deserialize<Configuracion>(json);
                    return data;
                }
            }
            catch (Exception) {
                Configuracion exception = new Configuracion();
                exception.StatusProceso = 0;
                return exception;
            }
             
        }

        public void GuardarResultados(Master masterData) {
            string fileName = $"resultados_{this.Nombre_Archivo}.json";
            string jsonString = JsonSerializer.Serialize(masterData);
            File.WriteAllText(fileName, jsonString); 
        }
    }
}