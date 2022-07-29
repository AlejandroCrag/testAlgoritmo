using AlgoritmoTest.Models;
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

        public List<DatosRegistro> ElementosFaltantesList { get; set; }
        public List<SubCluster> SubClusterActuales { get; set; }
        public int TotalElementosFaltantes { get; set; }
        public int Clouster_Id { get; private set; } 

        private Configuracion datosConfiguracion;
        public bool Start()
        {
            GetSubClustersActuales();
            for (int a = 0; a < TotalElementosFaltantes; a++)
            {
                //saveConfiguracion("", "", "");
                bool resultado = ElemFaltantesvsSubClusters(ElementosFaltantesList[a]);

                if (resultado)
                {
                    //saveConfiguracion("", "", "");
                    return true;
                }
            }
            //Status Incrementa en 1=> 
            //Siguiente Paso es comparar FaltantesVsFaltantes
            //O Escribir los Resultados
            StatusProceso++;
            return false;
        }

        private bool ElemFaltantesvsSubClusters(DatosRegistro ClienteUnico)
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
            ElementosFaltantesList = new List<DatosRegistro>();
            ElementosFaltantesList.Add(new DatosRegistro(1, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(2, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(3, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(4, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(5, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(6, "x", "y"));
            TotalElementosFaltantes = ElementosFaltantesList.Count();
        }


        private void RemoverDeFaltantes(DatosRegistro Cu)
        {
            var remove = ElementosFaltantesList.Remove(ElementosFaltantesList.Find(x => x.Id == Cu.Id));
            if (!remove)
            {
                Console.WriteLine("ImposibleRemover de la Lista");
            }
        }

        private void AgregarAlSubCluster(DatosRegistro clienteAgregar, int IdSubCluster)
        {
            /*Comprobar si el elemento a agregar no existe dentro del arreglo*/
            var obtenerSub = SubClusterActuales.Find(x => x.Id == IdSubCluster);
            var cliente = obtenerSub.GrupoRegistros.Count(y=> y.Id == clienteAgregar.Id);
            var agregado = false;
            if (cliente==0) {
                //Posiblemente en el proceso de FaltanteVsFaltante los SubCluster No existan

                if (StatusProceso == 2)
                {
                    //Revisar si el clouster existe
                    // getIdIfExist(SubClusterActuales);
                }
                //AgregarAlSubCluster
                //var data = new SubCluster(SubClusterActuales.Count + 1, Clouster_Id, new List<DatosRegistro>());
                //data.GrupoRegistros.Add(clienteAgregar); 
                if (agregado) {
                    RemoverDeFaltantes(clienteAgregar);
                }
            }
        }
        
        private bool ProcesoPermutacion(DatosRegistro Cliente, SubCluster subClusterActual)
        {
            var elementosEnSubCluster = subClusterActual.GrupoRegistros.Count();
            var newSubClus = new List<DatosRegistro>();
            int coincidencias = 0;
            foreach (var ClienteSub in subClusterActual.GrupoRegistros)
            {
                if (ClienteSub.Id != Cliente.Id)
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
                    AgregarAlSubCluster(newElement, subClusterActual.Clouster_Id);
                }
                return true;
            }
            return false;
        }

        private bool Compara(DatosRegistro cuSub, DatosRegistro cu)
        {
            return true;
        }

        private void GetSubClustersActuales()
        {
            //Obtenemos los SubClusters Recibidos de la comparacion
            if (this.StatusProceso == 1) {
                SubClusterActuales = new ConsultaSubClusters().ListadoSubClusters();
                //En Caso de no contar con ningun SubCluster tendremos que avanzar al siguiente status del proceso
                if (SubClusterActuales.Count() == 0)
                {
                    this.StatusProceso = 2;
                }
            }
            //Si se finalizo la comparacion O No Existen elemento con que comparar
            //Creamos Elementos a Comparar apartir de los mismo elementos faltantes
            if (this.StatusProceso == 2) {
                ConvertElementosFaltantesToSubClusters();
            }
        }

        private void ConvertElementosFaltantesToSubClusters()
        {
            SubClusterActuales = new List<SubCluster>();
            int id = 1;
            foreach (var CU in ElementosFaltantesList)
            {
                var data = new SubCluster(id, Clouster_Id, new List<DatosRegistro>());
                data.GrupoRegistros.Add(CU);
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
            datosConfiguracion = new Configuracion();
            datosConfiguracion.Ultimo_clouster = 1;
            datosConfiguracion.Ultimo_sub_clouster = 1;
            datosConfiguracion.UltimoRegistro = new DatosRegistro(1,"","");
            if (this.StatusProceso==0) {
                new ConsultaSubClusters().readJsonData();
                this.StatusProceso = 1;
            }
        }
    }
}