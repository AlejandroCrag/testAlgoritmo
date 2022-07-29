using System;

namespace AlgoritmoTest
{
    class Program
    {
        public static ProcesoComparacion proceso { get; private set; }

        static void Main(string[] args)
        {
            proceso = new ProcesoComparacion();
            Init();
            Console.WriteLine("end");
        }

        private static void Init()
        {
            if (!proceso.Init) {
                proceso.Init = true;
            }

            proceso.GetElementosFaltantes();
            proceso.LoadConfiguracion();

            var resultado = proceso.Start();
            try
            {
                Console.WriteLine(proceso.ElementoActual);
            }
            catch (StackOverflowException e) {
                Console.WriteLine(e.Message);
            }
            
            if (resultado)
            {
                Init();
            } else if (proceso.ElementoActual < proceso.TotalElementosFaltantes-1 ) {
                Init();
            }
        }
    }
}