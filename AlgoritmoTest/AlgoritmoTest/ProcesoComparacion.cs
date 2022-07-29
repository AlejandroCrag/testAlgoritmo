using AlgoritmoTest.Models;
using AlgoritmoTest.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoritmoTest
{
    class ProcesoComparacion
    { 
        public bool CrearSubClusters { get; internal set; }

        public bool Init = false;

        public List<DatosRegistro> ElementosFaltantesList { get; set; }
        public List<SubCluster> SubClusterActuales { get; set; }

        public int TotalElementosFaltantes { get; set; }
        public int Clouster_Id { get; private set; }

        public int SubClusterIdActual;

        public int ElementoActual = 0;

        private Configuracion datosConfiguracion;

        public bool Start()
        {
            CrearSubClusters = true;
            for (int a = 0; a < TotalElementosFaltantes; a++)
            {
                ElementoActual = a;
                bool resultado = ElemFaltantesvsSubClusters(ElementosFaltantesList[a]);

                if (resultado)
                {
                    //saveConfiguracion("", "", "");
                    return true;
                }
            }
            //saveConfiguracion("", "", "");
            return false;
        }

        private bool ElemFaltantesvsSubClusters(DatosRegistro Cu)
        {
            GetSubClustersActuales();

            SubClusterIdActual = 1;
            ConvertElementosFaltantesToSubClusters();

            foreach (var SCI in SubClusterActuales)
            {
                if (SubClusterIdActual != 0)
                {
                    SubClusterIdActual = SCI.Id;
                }

                var resultadoPermutacion = ProcesoPermutacion(Cu, SCI, SubClusterIdActual);
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

        private void AgregarAlSubCluster(DatosRegistro Cu, int SubClusId)
        {
            if (CrearSubClusters)
            {
                var data = new SubCluster(SubClusterActuales.Count + 1, Clouster_Id, new List<DatosRegistro>());
                data.GrupoRegistros.Add(Cu);
            }
        }
        
        private bool ProcesoPermutacion(DatosRegistro cu, SubCluster sCI, int subClusterIdActual)
        {
            var elementosEnSubCluster = sCI.GrupoRegistros.Count();
            var newSubClus = new List<DatosRegistro>();
            int coincidencias = 0;
            foreach (var CuSub in sCI.GrupoRegistros)
            {
                if (CuSub.Id != cu.Id)
                {
                    if (Compara(CuSub, cu) || Compara(CuSub, cu))
                    {
                        newSubClus.Add(cu);
                        newSubClus.Add(CuSub);
                        coincidencias++;
                    }
                }
                else {
                    return false;
                }
            }

            //if (coincidencias > (float)(elementosEnSubCluster / 2)) {
            if (CrearSubClusters)
                {
                    if (CrearSubClusters) {
                    foreach (var newElement in newSubClus) {
                        AgregarAlSubCluster(newElement, subClusterIdActual);
                        RemoverDeFaltantes(newElement);
                    }
                }
                else {
                    AgregarAlSubCluster(cu, subClusterIdActual);
                    RemoverDeFaltantes(cu);
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
            new ConsultaSubClusters().readJsonData();
            if (!CrearSubClusters)
            {
                SubClusterActuales = new ConsultaSubClusters().ListadoSubClusters();

                if (SubClusterActuales.Count() == 0) {
                    ConvertElementosFaltantesToSubClusters();
                }
            } else if (CrearSubClusters)
            {
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
            CrearSubClusters = true;
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
        }
    }
}