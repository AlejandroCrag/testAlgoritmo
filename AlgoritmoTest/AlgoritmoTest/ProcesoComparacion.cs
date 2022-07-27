using AlgoritmoTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoritmoTest
{
    class ProcesoComparacion
    {        
        public Boolean CrearSubClusters { get; internal set; }
        public List<DatosRegistro> ElementosFaltantesList { get; set; }
        public List<SubCluster> SubClusterActuales { get; set; }
        
        public int TotalElementos { get; set; }
        public int Clouster_Id { get; private set; }

        public int SubClusterIdActual;

        internal void getCuFaltantes()
        {
            ElementosFaltantesList  = new List<DatosRegistro>();
            ElementosFaltantesList.Add(new DatosRegistro(1, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(2, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(3, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(4, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(5, "x", "y"));
            ElementosFaltantesList.Add(new DatosRegistro(6, "x", "y"));
            
            TotalElementos = ElementosFaltantesList.Count();
        }


        private void RemoverDeFaltantes(DatosRegistro Cu)
        {
            var remove  =   ElementosFaltantesList.Remove(ElementosFaltantesList.Find(x => x.Id == Cu.Id));
            if (!remove)
            {
                Console.WriteLine("ImposibleRemover de la Lista");
            }
        }

        private void AgregarAlSubCluster(DatosRegistro Cu, int SubClusId)
        {
            if (CrearSubClusters)
            {
                var data = new SubCluster(SubClusterActuales.Count + 1, Clouster_Id ,  new List<DatosRegistro>());
                data.GrupoRegistros.Add(Cu);
            }
        }

        public Boolean Start()
        {
            for (int a = 0; a < TotalElementos; a++) {
                Boolean resultado = CUvsSubClusters(ElementosFaltantesList[a]);

                if (resultado)
                {
                    return true;
                }
            }
            return false;
        }

        private Boolean CUvsSubClusters(DatosRegistro Cu)
        {

            SubClusterActuales =  getSubClustersActuales();
            SubClusterIdActual = 1;
            if (SubClusterActuales.Count() == 0 || CrearSubClusters ) {
                SubClusterIdActual = 0;
                ConvertElementosFaltantesToSubClusters();
            }

            foreach (var SCI in SubClusterActuales)
            {
                if(SubClusterIdActual != 0)
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

        private Boolean ProcesoPermutacion(DatosRegistro cu, SubCluster sCI, int subClusterIdActual)
        { 
            var elementosEnSubCluster = sCI.GrupoRegistros.Count();
            var newSubClus = new List<DatosRegistro>();
            int coincidencias = 0;
            foreach(var CuSub in sCI.GrupoRegistros)
            {
                if (CuSub.Id != cu.Id) {
                    if (Compara(CuSub, cu) || Compara(CuSub, cu)) {
                        newSubClus.Add(cu);
                        newSubClus.Add(CuSub);
                        coincidencias++;
                    }
                }
            }

            if (coincidencias > (float)(elementosEnSubCluster / 2)) {
                if (CrearSubClusters){
                    foreach (var newElement in  newSubClus) {
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

        private List<SubCluster> getSubClustersActuales()
        {
            return new List<SubCluster>();
        }

        private void ConvertElementosFaltantesToSubClusters()
        {
            SubClusterActuales = new List<SubCluster>();
            int id = 1;
            foreach (var CU in ElementosFaltantesList) {
                var data = new SubCluster(id, Clouster_Id, new List<DatosRegistro>());
                data.GrupoRegistros.Add(CU);
                SubClusterActuales.Add(data);
                id++;
            }
        }
    }
}