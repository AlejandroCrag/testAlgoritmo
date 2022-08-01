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
                proceso.StatusProceso = proceso.StatusProceso+1;
                //Cargar Nuevo Documento
                Console.WriteLine("Comparacion Finalizada, Leer Nuevo Archivo");

            }
        }
    }
}