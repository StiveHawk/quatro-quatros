using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain
{
    public sealed class Simulador
    {
        private Simulador() { }

        public static readonly SimuladorOperacoes Operacoes = new SimuladorOperacoes();

        private static IEnumerable<Valor> ListarUnariosBase()
        {
            var original = new Valor(1, 4, 0).FormatterPuro(); ;

            if (Operacoes.Raiz) yield return new Valor(1, 2, 1).FormatterRaiz(original);
            yield return original;
            if (Operacoes.Terminal) yield return new Valor(1, 10, 1).FormatterTermial(original);
            if (Operacoes.Fatorial) yield return new Valor(1, 24, 1).FormatterFatorial(original);
        }

        private static IEnumerable<Valor> ListarBinarioBase()
        {
            var original = new Valor(2, 44, 0).FormatterPuro();

            if (Operacoes.Raiz) yield return new Valor(2, Math.Sqrt(44), 1).FormatterRaiz(original);
            yield return original;
            if (Operacoes.Terminal) yield return new Valor(2, Dicionarios.TermialHashSet[44], 1).FormatterTermial(original);
        }

        private static IEnumerable<Valor> ListarTernarioBase()
        {
            var original = new Valor(3, 444, 0).FormatterPuro();

            if (Operacoes.Raiz) yield return new Valor(3, Math.Sqrt(444), 1).FormatterRaiz(original);
            yield return original;
            if (Operacoes.Terminal) yield return new Valor(3, Dicionarios.TermialHashSet[444], 1).FormatterTermial(original);
        }

        private static IEnumerable<Valor> ListarQuaternarioBase()
        {
            var original = new Valor(4, 4444, 0).FormatterPuro();

            if (Operacoes.Raiz) yield return new Valor(4, Math.Sqrt(4444), 1).FormatterRaiz(original);
            yield return original;
            if (Operacoes.Terminal) yield return new Valor(4, Dicionarios.TermialHashSet[4444], 1).FormatterTermial(original);
        }

        private static IEnumerable<Valor> ListarOperacao(Valor v1, Valor v2)
        {
            int peso = v1.Peso + v2.Peso;
            int nivelOperacoes = v1.NivelOperacoes + v2.NivelOperacoes + 1;
            double aux;

            if (Operacoes.Subtracao && Subtrair(v1.Resultado, v2.Resultado, out aux)) yield return new Valor(peso, aux, nivelOperacoes).FormatterSubtracao(v1, v2);
            if (Operacoes.Subtracao && Subtrair(v2.Resultado, v1.Resultado, out aux)) yield return new Valor(peso, aux, nivelOperacoes).FormatterSubtracao(v2, v1);

            if (Operacoes.Soma && Somar(v1.Resultado, v2.Resultado, out aux)) yield return new Valor(peso, aux, nivelOperacoes).FormatterSoma(v1, v2);

            if (Operacoes.Multiplicacao && Multiplicar(v1.Resultado, v2.Resultado, out aux)) yield return new Valor(peso, aux, nivelOperacoes).FormatterMultiplicacao(v1, v2);

            if (Operacoes.Divisao)
            {
                if (v2.Resultado != 0 && Dividir(v1.Resultado, v2.Resultado, out aux))
                    yield return new Valor(peso, aux, nivelOperacoes).FormatterDivisao(v1, v2);

                if (v1.Resultado != 0 && Dividir(v2.Resultado, v1.Resultado, out aux))
                    yield return new Valor(peso, aux, nivelOperacoes).FormatterDivisao(v2, v1);
            }

            if (Operacoes.Potenciacao && v1.Resultado != 0 && v2.Resultado != 0)
            {
                if(Exponenciar(v1.Resultado, v2.Resultado, out aux))
                    yield return new Valor(peso, aux, nivelOperacoes).FormatterPotenciacao(v1, v2);

                if (Exponenciar(v2.Resultado, v1.Resultado, out aux))
                    yield return new Valor(peso, aux, nivelOperacoes).FormatterPotenciacao(v2, v1); ;
            }
        }

        private static bool Subtrair(double v1, double v2, out double resultado)
        {
            resultado = 0;
            try { resultado = checked(v1 - v2); return true; }
            catch (OverflowException) { return false; }
        }

        private static bool Somar(double v1, double v2, out double resultado)
        {
            resultado = 0;
            try { resultado = checked(v1 + v2); return true; }
            catch (OverflowException) { return false; }
        }

        private static bool Multiplicar(double v1, double v2, out double resultado)
        {
            resultado = 0;
            try { resultado = checked(v1 * v2); return true; }
            catch (OverflowException) { return false; }
        }

        private static bool Dividir(double v1, double v2, out double resultado)
        {
            resultado = 0;
            try { resultado = checked(v1 / v2); return true; }
            catch (OverflowException) { return false; }
        }

        private static bool Exponenciar(double v1, double v2, out double resultado)
        {
            resultado = 0;
            try { resultado = checked(Math.Pow(v1, v2)); return true; }
            catch (OverflowException) { return false; }
        }

        public static IEnumerable<Valor> ListarTodos()
        {
            var unarios = ListarUnariosBase().SelectMany(x => x.TodasOperacoes()).DistinctOperacao().ToList();

            var variacoesBinario =
                from v1 in unarios
                from v2 in unarios
                select ListarOperacao(v1, v2);

            var binarios = variacoesBinario.SelectMany(x => x).Concat(ListarBinarioBase()).SelectMany(x => x.TodasOperacoes()).DistinctOperacao().ToList();

            var variacoesTernarios =
                from v1 in unarios
                from v2 in binarios
                select ListarOperacao(v1, v2);

            var ternarios = variacoesTernarios.SelectMany(x => x).Concat(ListarTernarioBase()).SelectMany(x => x.TodasOperacoes()).DistinctOperacao().ToList();

            var variacoesQuaternarios1 =
                from v1 in unarios
                from v2 in ternarios
                select ListarOperacao(v1, v2);

            var variacoesQuaternarios2 =
                from v1 in binarios
                from v2 in binarios
                select ListarOperacao(v1, v2);

            var quaternariosTodos = variacoesQuaternarios1.Concat(variacoesQuaternarios2).SelectMany(x => x).Concat(ListarQuaternarioBase())
                .SelectMany(x => x.TodasOperacoes());

            return quaternariosTodos;
        }
    }
}
