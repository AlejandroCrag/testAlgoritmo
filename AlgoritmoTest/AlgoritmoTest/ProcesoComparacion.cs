using AlgoritmoTest.Models;
using AlgoritmoTest.Models.Entity;
using AlgoritmoTest.Models.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoritmoTest
{
    class ProcesoComparacion
    {
        private static HuellasDB huellasDB;
        //sabremos en que parte del proceso se encuentra 
        //0 => Sin Iniciar -> se busca archivo a cargar
        //1 => Proceso AB FaltantesVsSubClusters
        //2 => Proceso BB FaltantesVsFaltantes
        //3 => Proceso Finalizado Escribir Resultados -  Buscar nuevo archivo a cargar
        public int StatusProceso = 0; 
        public List<Clientes> ElementosFaltantesList { get; set; }
        public List<SubClusters> SubClusterActuales { get; set; }

        public List<SubClusters> SubClusterGenerados = new List<SubClusters>();

        private Configuracion datosConfiguracion;
        

        public bool Start()
        { 
            GetSubClustersActuales();
            for (int a = 0; a < ElementosFaltantesList.Count(); a++)
            {
                bool resultado = ElemFaltantesvsSubClusters(ElementosFaltantesList[a]);
                if (resultado)
                {
                    return resultado;
                }
            }
            StatusProceso++;
            return false;
        }

        private bool ElemFaltantesvsSubClusters(Clientes ClienteUnico)
        {
            foreach (var SCI in SubClusterActuales)
            { 
                var resultadoPermutacion = ProcesoPermutacion(ClienteUnico, SCI);
                if (resultadoPermutacion)
                {
                    return resultadoPermutacion;
                }
            }
            return false;
        }

        private void RemoverDeFaltantes(Clientes Cu)
        {
            var remove = ElementosFaltantesList.Remove(ElementosFaltantesList.Find(x => x.Folio == Cu.Folio));

            if (!remove)
            {
                Console.WriteLine("ImposibleRemover de la Lista");
            }
        }
         
        private bool ProcesoPermutacion(Clientes Cliente, SubClusters subClusterActual)
        {
            var elementosEnSubCluster = subClusterActual.Clientes.Count();
            var newSubClus = new List<Clientes>();
            int coincidencias = 0;
            foreach (var ClienteSub in subClusterActual.Clientes)
            {
                if (ClienteSub.Folio != Cliente.Folio)
                {
                    if (Compara(ClienteSub, Cliente) || Compara(ClienteSub, Cliente))
                    {
                        newSubClus.Add(Cliente);
                        if (StatusProceso == 2) {
                            newSubClus.Add(ClienteSub);
                        }
                        coincidencias++;
                    }
                }
                else {
                    return false;
                }
            }

            if (coincidencias > (float)(elementosEnSubCluster / 2)) {
                if (StatusProceso == 2 && subClusterActual.Contenedor != "SUB_C_N")
                {
                    int newID = SubClusterGenerados.Count() + 1;
                    List<Clientes> clienteList = new List<Clientes>();
                    SubClusters nuevoElemento = new SubClusters(newID,1, "SUB_C_N", clienteList);
                    SubClusterGenerados.Add(nuevoElemento);
                    subClusterActual.Id = newID;
                }

                foreach (var newElement in newSubClus)
                {
                    if (StatusProceso == 1) {
                        AgregarAlSubCluster(newElement, subClusterActual.Id);
                    }else{
                        AgregarAlSubClusterCreados(newElement, subClusterActual.Id);
                    }
                }
                return true;
            }
            return false;
        }

        private void AgregarAlSubClusterCreados(Clientes clienteAgregar, int IdSubCluster)
        {
            /*Comprobar si el elemento a agregar no existe dentro del arreglo*/
            var obtenerSub = SubClusterGenerados.Find(x => x.Id == IdSubCluster);
            var cliente = obtenerSub.Clientes.Count(y => y.Folio == clienteAgregar.Folio); 
            if (cliente == 0)
            {
                obtenerSub.Clientes.Add(clienteAgregar);
                RemoverDeFaltantes(clienteAgregar); 
            }
        }

        private void AgregarAlSubCluster(Clientes clienteAgregar, int IdSubCluster)
        {
            /*Comprobar si el elemento a agregar no existe dentro del arreglo*/
            var obtenerSub = SubClusterActuales.Find(x => x.Id == IdSubCluster);
            var cliente = obtenerSub.Clientes.Count(y => y.Folio == clienteAgregar.Folio);
            if (cliente == 0)
            {
                obtenerSub.Clientes.Add(clienteAgregar);
                RemoverDeFaltantes(clienteAgregar);
            }
        }
        
        private bool Compara(Clientes cuSub, Clientes cu)
        {
            Random rd = new Random();
            int rand_num = rd.Next(0, 100);

            if (rand_num < 95) {
                return false;
            }
            return true;
        }

        private void GetSubClustersActuales()
        {
            //Obtenemos los SubClusters Recibidos de la comparacion
            if (SubClusterActuales.Count() == 0 && this.StatusProceso == 1) {
                this.StatusProceso = 2;
            }
            
            if (this.StatusProceso == 2)
            {
                this.SaveConfiguracion();
                ConvertElementosFaltantesToSubClusters();
            }
        }

        private void ConvertElementosFaltantesToSubClusters()
        {
            SubClusterActuales = new List<SubClusters>();
            int id = 1;

            foreach (var newCU in SubClusterGenerados) {
                SubClusterActuales.Add(newCU);
            }

            foreach (var CU in ElementosFaltantesList)
            {
                var data = new SubClusters(id,0, "FALTANTES", new List<Clientes>());
                data.Clientes.Add(CU);
                SubClusterActuales.Add(data);
                id++;
            }
        }

        private void SaveConfiguracion()
        {
            datosConfiguracion.Nombre_Archivo = "archivo1.Json";
            datosConfiguracion.StatusProceso = this.StatusProceso;
            datosConfiguracion.SubClusterActuales = this.SubClusterActuales;
            datosConfiguracion.SubClusterGenerados = this.SubClusterGenerados is null ? new List<SubClusters>() : this.SubClusterGenerados;
            datosConfiguracion.ElementosFaltantesList = this.ElementosFaltantesList;
            datosConfiguracion.GuardarConfiguracion(); 
        }
        
        public void LoadConfiguracion()
        {

            huellasDB = new HuellasDB(); 
            //var data = huellasDB.ReadConfiguracion(huellasDB.CreateConnection);
            //Leer Archivos Faltantes o En que #de Archivo va
            datosConfiguracion = new Configuracion().LeerConfiguracion();
            if (datosConfiguracion.StatusProceso != 0) {
                this.StatusProceso = datosConfiguracion.StatusProceso;
                this.ElementosFaltantesList = datosConfiguracion.ElementosFaltantesList;
                this.SubClusterActuales = datosConfiguracion.SubClusterActuales;
                this.SubClusterGenerados = datosConfiguracion.SubClusterGenerados is null ? new List<SubClusters>() : datosConfiguracion.SubClusterGenerados;
            }
            else if (this.StatusProceso==0) {
                ConsultaSubClusters consultaSub = new ConsultaSubClusters();
                consultaSub.readJsonData();
                ElementosFaltantesList = consultaSub.ClientesFaltantes;
                SubClusterActuales = consultaSub.subClusters;
                this.StatusProceso = 1;
            }
        }
    }
}