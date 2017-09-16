using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain
{
    public class OutputNaturais
    {
        public Dictionary<int, Valor> MelhoresValores;
        public int QuantidadeProcessada;

        public OutputNaturais()
        {
            MelhoresValores = new Dictionary<int, Valor>();
            QuantidadeProcessada = 0;
        }

        private void Registrar(Valor valor)
        {
            if (valor.Inteiro)
            {
                int resultado = (int)valor.Resultado;
                
                Valor antigo = null;

                if(!MelhoresValores.TryGetValue(resultado, out antigo) || antigo.NivelOperacoes > valor.NivelOperacoes)
                    MelhoresValores[resultado] = valor;
            }
        }

        public IEnumerable<Valor> OrdemCrescente()
        {
            return MelhoresValores.OrderBy(x => x.Key).Select(x => x.Value);
        }

        private string FormatarValor(Valor valor)
        {
            return $"{valor.Resultado.ToString("#0")}: {valor.Operacao}";
        }

        public void EscreverEmArquivo(string caminhoArquivo)
        {
            using (StreamWriter writer = new StreamWriter(caminhoArquivo))
            {
                foreach (var valor in OrdemCrescente())
                    writer.WriteLine(FormatarValor(valor));

                writer.Flush();
            }
        }

        public void EscreverEmConsole()
        {
            foreach (var valor in OrdemCrescente())
                Console.WriteLine(FormatarValor(valor));
        }

        public static OutputNaturais Processar(IEnumerable<Valor> valores)
        {
            OutputNaturais output = new OutputNaturais();

            foreach (var valor in valores)
            {
                output.Registrar(valor);
                output.QuantidadeProcessada++;
            }

            return output;
        }
    }
}
