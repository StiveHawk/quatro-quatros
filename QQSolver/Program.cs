using QQSolver.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var naturais = OutputNaturais.Processar(Simulador.ListarApenasMelhores());
            sw.Stop();

            naturais.EscreverEmArquivo("Naturais.txt");
            naturais.EscreverEmConsole();
            
            Console.WriteLine($"Processado em {sw.Elapsed.ToString()}");
            Console.ReadLine();
        }
    }
}
