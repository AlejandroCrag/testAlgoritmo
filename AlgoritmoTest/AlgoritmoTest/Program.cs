using System;

namespace AlgoritmoTest
{
    class Program
    {
        public static ProcesoComparacion proceso { get; private set; }

        static void Main(string[] args)
        {
            proceso = new ProcesoComparacion();
            proceso.LoadConfiguracion();
            Iniciar(); 
        }

        private static void Iniciar()
        { 
            proceso.GetElementosFaltantes();
             
            var resultado = proceso.Start();

            if (resultado)
            {
                Iniciar();
            }
            else if (proceso.StatusProceso < 3)
            {
                Iniciar();
            }
            else if (proceso.StatusProceso == 3) {
                //Escribir Resultados
            }
            else if (proceso.StatusProceso == 4) { 
                //Cargar Nuevo Documento
            }
        }
    }
}