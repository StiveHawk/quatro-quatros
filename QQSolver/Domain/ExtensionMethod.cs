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

        public static Dictionary<double, Valor> ApenasMelhores(this IEnumerable<Valor> valores)
        {
            var melhores = new Dictionary<double, Valor>();

            foreach (var valor in valores)
            {
                Valor antigo = null;
                if (!melhores.TryGetValue(valor.Resultado, out antigo) || antigo.NivelOperacoes > valor.NivelOperacoes)
                    melhores[valor.Resultado] = valor;
            }

            return melhores;
        }
    }
}
