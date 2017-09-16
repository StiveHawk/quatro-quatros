using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain
{
    public static class ExtensionMethod
    {
        public static IEnumerable<Valor> DistinctOperacao(this IEnumerable<Valor> valores)
        {
            HashSet<string> hash = new HashSet<string>();

            foreach (var valor in valores)
            {
                string text = valor.Operacao;
                if (!hash.Contains(text))
                {
                    hash.Add(text);
                    yield return valor;
                }
            }
        }
    }
}
