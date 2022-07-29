using AlgoritmoTest.Models;
using AlgoritmoTest.Models.Entity;
using AlgoritmoTest.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoritmoTest
{
    class ProcesoComparacion
    { 
        //sabremos en que parte del proceso se encuentra 
        //0 => Sin Iniciar -> se busca archivo a cargar
        //1 => Proceso AB FaltantesVsSubClusters
        //2 => Proceso BB FaltantesVsFaltantes
        //3 => Proceso Finalizado Escribir Resultados
        //4 => Buscar nuevo archivo a cargar
        public int StatusProceso = 0;

        public List<Clientes> ElementosFaltantesList { get; set; }
        public List<SubClusters> SubClusterActuales { get; set; }
        public int TotalElementosFaltantes { get; set; }
        public int Clouster_Id { get; private set; } 

        private Configuracion datosConfiguracion;
        public bool Start()
        {
            GetSubClustersActuales();
            for (int a = 0; a < ElementosFaltantesList.Count(); a++)
            {
                bool resultado = ElemFaltantesvsSubClusters(ElementosFaltantesList[a]);
                if (resultado)
                {
                    return true;
                }
            }

            //Status Incrementa en 1=> 
            //Siguiente Paso es comparar FaltantesVsFaltantes
            //O Escribir los Resultados
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
                    return true;
                }
            }
            return false;
        }

        internal void GetElementosFaltantes()
        {
            ElementosFaltantesList = new List<Clientes>();
            ElementosFaltantesList.Add(new Clientes(11, 2, 2, 2 , new List<Fingerprint>()));
            ElementosFaltantesList.Add(new Clientes(12, 2, 2, 21, new List<Fingerprint>()));
            ElementosFaltantesList.Add(new Clientes(13, 2, 2, 22, new List<Fingerprint>()));
            ElementosFaltantesList.Add(new Clientes(14, 2, 2, 23, new List<Fingerprint>()));
            ElementosFaltantesList.Add(new Clientes(15, 2, 2, 24, new List<Fingerprint>()));

            TotalElementosFaltantes = ElementosFaltantesList.Count();
        }


        private void RemoverDeFaltantes(Clientes Cu)
        {
            var remove = ElementosFaltantesList.Remove(ElementosFaltantesList.Find(x => x.Folio == Cu.Folio));
            if (!remove)
            {
                Console.WriteLine("ImposibleRemover de la Lista");
            }
        }

        private void AgregarAlSubCluster(Clientes clienteAgregar, int IdSubCluster)
        {
            /*Comprobar si el elemento a agregar no existe dentro del arreglo*/
            var obtenerSub = SubClusterActuales.Find(x => x.Id == IdSubCluster);
            var cliente = obtenerSub.Clientes.Count(y=> y.Folio == clienteAgregar.Folio);
            var agregado = false;
            if (cliente==0) {
                //Posiblemente en el proceso de FaltanteVsFaltante los SubCluster No existan
                
                agregado = true;
                if (StatusProceso == 2)
                {
                    //Revisar si el clouster existe
                    // getIdIfExist(SubClusterActuales);
                }
                else {
                    obtenerSub.Clientes.Add(clienteAgregar);
                }
                //AgregarAlSubCluster
                //var data = new SubCluster(SubClusterActuales.Count + 1, Clouster_Id, new List<DatosRegistro>());
                //data.GrupoRegistros.Add(clienteAgregar); 
                if (agregado) {
                    RemoverDeFaltantes(clienteAgregar);
                }
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
                foreach (var newElement in newSubClus)
                {
                    AgregarAlSubCluster(newElement, subClusterActual.Id);
                }
                return true;
            }
            return false;
        }

        private bool Compara(Clientes cuSub, Clientes cu)
        {
            Random rd = new Random();
            int rand_num = rd.Next(0, 100);


            if (rand_num < 93) {
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
                ConvertElementosFaltantesToSubClusters();
            }
        }

        private void ConvertElementosFaltantesToSubClusters()
        {
            SubClusterActuales = new List<SubClusters>();
            int id = 1;
            foreach (var CU in ElementosFaltantesList)
            {
                var data = new SubClusters(id, "FALTANTES", new List<Clientes>());
                data.Clientes.Add(CU);
                SubClusterActuales.Add(data);
                id++;
            } 
        }

        private void SaveConfiguracion(String clouster,String subCluster,String DatosRegistro)
        { 
            Console.WriteLine("YYYYYYYYYYYY");
        }

        public void LoadConfiguracion()
        {
            //Leer Archivos Faltantes o En que #de Archivo va
            datosConfiguracion = new Configuracion();
            datosConfiguracion.Ultimo_clouster = 1;
            datosConfiguracion.Ultimo_sub_clouster = 1;
            //datosConfiguracion.UltimoRegistro = new Clientes();
            if (this.StatusProceso==0) {
                ConsultaSubClusters consultaSub = new ConsultaSubClusters();
                consultaSub.readJsonData();
                ElementosFaltantesList = consultaSub.ClientesFaltantes;
                SubClusterActuales = consultaSub.subClusters;
                this.StatusProceso = 1;
            }
        }
    }
}