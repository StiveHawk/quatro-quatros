using QQSolver.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            QQExtendido();
        }

        static void TestarTodosUnarios()
        {
            foreach (var unario in TodosUnarios(Valor.ListarUnariosBase()))
                Console.WriteLine($"{unario.Resultado.ToString("#0.##")}: {unario.Operacao}");

            Console.ReadLine();
        }

        static IEnumerable<Valor> TodosUnarios(IEnumerable<Valor> valores)
        {
            foreach(var valor in valores)
                yield return valor;

            foreach (var valor in valores)
                foreach (var filho in TodosUnarios(valor.TodasOperacoes()))
                    yield return filho;
        }
        
        static void QQExtendido()
        {
            //Valor.OperacoesPermitidas.Fatorial = false;
            Valor.OperacoesPermitidas.Raiz = false;
            //Valor.OperacoesPermitidas.Potenciacao = false;
            //Valor.OperacoesPermitidas.Terminal = false;

            var unarios = Valor.ListarUnariosBase().SelectMany(x => x.TodasOperacoes()).DistinctOperacao().ToList();

            var baseBinarios = from v1 in unarios
                               from v2 in unarios
                               select Valor.ListarOperacao(v1, v2);

            var binarios = baseBinarios.SelectMany(x => x).Concat(Valor.ListarBinarioBase()).SelectMany(x => x.TodasOperacoes()).DistinctOperacao().ToList();

            var baseTernarios = from v1 in unarios
                                from v2 in binarios
                                select Valor.ListarOperacao(v1, v2);

            var ternarios = baseTernarios.SelectMany(x => x).Concat(Valor.ListarTernarioBase()).SelectMany(x => x.TodasOperacoes()).DistinctOperacao().ToList();

            var baseQuaternarios1 = from v1 in unarios
                                    from v2 in ternarios
                                    select Valor.ListarOperacao(v1, v2);

            var baseQuaternarios2 = from v1 in binarios
                                    from v2 in binarios
                                    select Valor.ListarOperacao(v1, v2);

            var quaternariosTodos = baseQuaternarios1.Concat(baseQuaternarios2).SelectMany(x => x).Concat(Valor.ListarQuaternarioBase())
                .SelectMany(x => x.TodasOperacoes());

            var quaternariosRelevantes = quaternariosTodos
                .Where(x => x.Resultado > 0 && x.Resultado <= 100 && x.Inteiro);
            //.OrderBy(x => x.Resultado)
            //.ToList();
            

            int total = 0;
            int count = 0;
            foreach (var resultado in quaternariosTodos)
            {
                //count++;
                //if (count == 100000)
                //{
                //    total += count;
                //    count = 0;
                //    Console.WriteLine("Lidos " + total);
                //}

                //Console.WriteLine($"{resultado.Resultado.ToString("#0.##")}: {resultado.Operacao}");
                Registrar(resultado);
            }

            foreach (var melhor in MelhoresValores.OrderBy(x => x.Key).Select(x => x.Value))
                Console.WriteLine($"{melhor.Resultado.ToString("#0.##")}: {melhor.Operacao}");

            Console.WriteLine("Fim");
            Console.ReadLine();
        }

        static Dictionary<int, Valor> MelhoresValores = new Dictionary<int, Valor>();
        static void Registrar(Valor valor)
        {
            if(valor.Inteiro)
            {
                int resultado = (int)valor.Resultado;

                //if(resultado > 0 && resultado < 100)
                //{
                    Valor antigo = null;
                    MelhoresValores.TryGetValue(resultado, out antigo);
                    if(antigo == null || antigo.NivelOperacoes > valor.NivelOperacoes)
                    {
                        MelhoresValores[resultado] = valor;
                        //Console.WriteLine($"{valor.Resultado.ToString("#0.##")}: {valor.Operacao}");
                    }
                //}
            }
        }

        static void QQPadrao()
        {
            var unarios = Valor.ListarUnariosBase().ToList();

            var baseBinarios = from v1 in unarios
                               from v2 in unarios
                               select Valor.ListarOperacao(v1, v2);

            var binarios = baseBinarios.SelectMany(x => x).Concat(Valor.ListarBinarioBase()).ToList();

            var baseTernarios = from v1 in unarios
                                from v2 in binarios
                                select Valor.ListarOperacao(v1, v2);

            var ternarios = baseTernarios.SelectMany(x => x).Concat(Valor.ListarTernarioBase());

            var baseQuaternarios1 = from v1 in unarios
                                    from v2 in ternarios
                                    select Valor.ListarOperacao(v1, v2);

            var baseQuaternarios2 = from v1 in binarios
                                    from v2 in binarios
                                    select Valor.ListarOperacao(v1, v2);

            var quaternarios = baseQuaternarios1.Concat(baseQuaternarios2).SelectMany(x => x).Concat(Valor.ListarQuaternarioBase())
                .Where(x => x.Resultado > 0 && x.Resultado <= 100 && x.Inteiro)
                .OrderBy(x => x.Resultado)
                .ToList();

            foreach (var resultado in quaternarios)
                Console.WriteLine($"{resultado.Resultado.ToString("#0.##")}: {resultado.Operacao}");

            Console.ReadLine();
        }
    }
}
