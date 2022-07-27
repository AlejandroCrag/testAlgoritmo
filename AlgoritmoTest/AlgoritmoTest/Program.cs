using System;

namespace AlgoritmoTest
{
    class Program
    {
        static void Main(string[] args)
        {

            
            ProcesoComparacion proceso = new ProcesoComparacion();
            proceso.getCuFaltantes();

            if (proceso.TotalElementos > 0) {
                init(proceso);
            }
             

            ProcesoComparacion proceso2 = new ProcesoComparacion();
            proceso2.CrearSubClusters = true;
            init(proceso2);
            Console.WriteLine("END...");
        }
         
        private static void init(ProcesoComparacion proceso)
        {
            var resultado = proceso.Start();
            if(resultado)
            {
                init(proceso);
            }
        }       
    }
}
