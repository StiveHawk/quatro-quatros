using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain
{
    public class Valor
    {
        public int Peso;
        public double Resultado;
        public int NivelOperacoes;

        public bool Inteiro;
        public bool Positivo;
        public bool Puro;

        public IValorFormatter Formatter;
        public string Operacao {  get { return Formatter.Flush(); } }

        public Valor(int representacao, double resultado, int nivelOperacoes)
        {
            this.Peso = representacao;
            this.Resultado = resultado;
            this.NivelOperacoes = nivelOperacoes;

            this.Inteiro = (Resultado < int.MaxValue && Resultado % 1 == 0);
            this.Positivo = (Resultado > 0);
            this.Puro = (NivelOperacoes == 0);
        }

        private IEnumerable<Valor> ListarOperacoesUnarias()
        {
            if (!Puro)
            {
                if (Positivo && Inteiro)
                {
                    if (OperacoesPermitidas.Fatorial && Resultado < long.MaxValue && Resultado > 2)
                    {
                        long lAux;
                        var resultadoLong = (long)Resultado;

                        if (FactorialHashSet.TryGetValue(resultadoLong, out lAux))
                            yield return new Valor(Peso, lAux, NivelOperacoes + 1).FormatterFatorial(this);
                    }

                    if (OperacoesPermitidas.Terminal && Resultado < int.MaxValue && Resultado > 1)
                    {
                        int iAux;
                        var resultadoInt = (int)Resultado;

                        if (TerminalHashSet.TryGetValue(resultadoInt, out iAux) && iAux != Resultado)
                            yield return new Valor(Peso, iAux, NivelOperacoes + 1).FormatterTerminal(this);
                    }
                }
            }
        }

        public Valor FormatterPuro() { this.Formatter = new UnaryFormatter.PuroFormatter(this); return this; }
        public Valor FormatterFatorial(Valor original) { this.Formatter = new UnaryFormatter.FatorialFormatter(original); return this; }
        public Valor FormatterTerminal(Valor original) { this.Formatter = new UnaryFormatter.TerminalFormatter(original); return this; }
        public Valor FormatterRaiz(Valor original) { this.Formatter = new UnaryFormatter.RaizFormatter(original); return this; }

        public Valor FormatterSoma(Valor v1, Valor v2) { this.Formatter = BinaryFormatter.Soma(v1, v2); return this; }
        public Valor FormatterDivisao(Valor v1, Valor v2) { this.Formatter = BinaryFormatter.Divisao(v1, v2); return this; }
        public Valor FormatterMultiplicacao(Valor v1, Valor v2) { this.Formatter = BinaryFormatter.Multiplicacao(v1, v2); return this; }
        public Valor FormatterPotenciacao(Valor v1, Valor v2) { this.Formatter = BinaryFormatter.Potenciacao(v1, v2); return this; }
        public Valor FormatterSubtracao(Valor v1, Valor v2) { this.Formatter = BinaryFormatter.Subtracao(v1, v2); return this; }


        private IEnumerable<Valor> ThisEnumerable() { yield return this; }

        public IEnumerable<Valor> TodasOperacoes()
        {
            return TodasOperacoes(ThisEnumerable());
        }

        private IEnumerable<Valor> TodasOperacoes(IEnumerable<Valor> valores)
        {
            foreach (var valor in valores)
                yield return valor;

            foreach (var valor in valores)
                foreach (var filho in TodasOperacoes(valor.ListarOperacoesUnarias()))
                    yield return filho;
        }

        public static Operacoes OperacoesPermitidas = new Operacoes();

        public class Operacoes
        {
            public bool Soma;
            public bool Subtracao;
            public bool Divisao;
            public bool Potenciacao;
            public bool Multiplicacao;

            public bool Raiz;
            public bool Fatorial;
            public bool Terminal;

            public Operacoes()
            {
                this.Soma = true;
                this.Subtracao = true;
                this.Divisao = true;
                this.Potenciacao = true;
                this.Multiplicacao = true;

                this.Raiz = true;
                this.Fatorial = true;
                this.Terminal = true;
            }
        }

        private static Dictionary<long, long> FactorialHashSet = CreateFactorialHashSet();
        private static Dictionary<long, long> CreateFactorialHashSet()
        {
            Dictionary<long, long> dic = new Dictionary<long, long>();
            long chave = 1;
            long result = 1;

            try
            {
                while (result < long.MaxValue)
                {
                    dic.Add(chave, result);

                    chave++;
                    result = checked(result * chave);
                }
            }
            catch(OverflowException e) {  }

            return dic;
        }

        private static Dictionary<int, int> TerminalHashSet = CreateTerminalHashSet();
        private static Dictionary<int, int> CreateTerminalHashSet()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            int chave = 1;
            int result = 1;

            try
            {
                while (result < int.MaxValue)
                {
                    dic.Add(chave, result);

                    chave++;
                    result = checked(result + chave);
                }
            }
            catch (OverflowException e) { }
            catch (OutOfMemoryException) { }

            return dic;
        }

        public static IEnumerable<Valor> ListarUnariosBase()
        {
            var original = new Valor(1, 4, 0).FormatterPuro(); ;

            if (OperacoesPermitidas.Raiz) yield return new Valor(1, 2, 1).FormatterRaiz(original);
            yield return original;
            if (OperacoesPermitidas.Terminal) yield return new Valor(1, 10, 1).FormatterTerminal(original);
            if (OperacoesPermitidas.Fatorial) yield return new Valor(1, 24, 1).FormatterFatorial(original);
        }

        public static IEnumerable<Valor> ListarBinarioBase()
        {
            var original = new Valor(2, 44, 0).FormatterPuro();

            if (OperacoesPermitidas.Raiz) yield return new Valor(2, Math.Sqrt(44), 1).FormatterRaiz(original);
            yield return original;
            if (OperacoesPermitidas.Terminal) yield return new Valor(2, TerminalHashSet[44], 1).FormatterTerminal(original);
        }

        public static IEnumerable<Valor> ListarTernarioBase()
        {
            var original = new Valor(3, 444, 0).FormatterPuro();

            if (OperacoesPermitidas.Raiz) yield return new Valor(3, Math.Sqrt(444), 1).FormatterRaiz(original);
            yield return original;
            if (OperacoesPermitidas.Terminal) yield return new Valor(3, TerminalHashSet[444], 1).FormatterTerminal(original);
        }

        public static IEnumerable<Valor> ListarQuaternarioBase()
        {
            var original = new Valor(4, 4444, 0).FormatterPuro();

            if (OperacoesPermitidas.Raiz) yield return new Valor(4, Math.Sqrt(4444), 1).FormatterRaiz(original);
            yield return original;
            if (OperacoesPermitidas.Terminal) yield return new Valor(4, TerminalHashSet[4444], 1).FormatterTerminal(original);
        }

        public static IEnumerable<Valor> ListarOperacao(Valor v1, Valor v2)
        {
            int peso = v1.Peso + v2.Peso;
            int nivelOperacoes = v1.NivelOperacoes + v2.NivelOperacoes + 1;

            if(OperacoesPermitidas.Subtracao) yield return new Valor(peso, v1.Resultado - v2.Resultado, nivelOperacoes).FormatterSubtracao(v1, v2);
            if (OperacoesPermitidas.Subtracao) yield return new Valor(peso, v2.Resultado - v1.Resultado, nivelOperacoes).FormatterSubtracao(v2, v1);

            if (OperacoesPermitidas.Soma) yield return new Valor(peso, v1.Resultado + v2.Resultado, nivelOperacoes).FormatterSoma(v1, v2);

            if (OperacoesPermitidas.Multiplicacao) yield return new Valor(peso, v1.Resultado * v2.Resultado, nivelOperacoes).FormatterMultiplicacao(v1, v2);

            if (OperacoesPermitidas.Multiplicacao)
            {
                if (v1.Resultado != 0)
                    yield return new Valor(peso, v2.Resultado / v1.Resultado, nivelOperacoes).FormatterDivisao(v2, v1);

                if (v2.Resultado != 0)
                    yield return new Valor(peso, v1.Resultado / v2.Resultado, nivelOperacoes).FormatterDivisao(v1, v2);
            }

            if(OperacoesPermitidas.Potenciacao && v1.Resultado != 0 && v2.Resultado != 0)
            {
                yield return new Valor(peso, Math.Pow(v1.Resultado, v2.Resultado), nivelOperacoes).FormatterPotenciacao(v1, v2);
                yield return new Valor(peso, Math.Pow(v2.Resultado, v1.Resultado), nivelOperacoes).FormatterPotenciacao(v2, v1); ;
            }
        }

        public interface IValorFormatter
        {
            string Flush();
        }

        public abstract class UnaryFormatter : IValorFormatter
        {
            protected string Resultado;
            private Valor Valor;

            public UnaryFormatter(Valor valor)
            {
                Valor = valor;
            }

            public abstract string Flush();

            public class FatorialFormatter : UnaryFormatter
            {
                public FatorialFormatter(Valor valor) : base(valor) { }
                public override string Flush() { return Resultado ?? (Resultado = $"({Valor.Formatter.Flush()})!"); }
            }

            public class RaizFormatter : UnaryFormatter
            {
                public RaizFormatter(Valor valor) : base(valor) { }
                public override string Flush() { return Resultado ?? (Resultado = $"Sqrt({Valor.Formatter.Flush()})"); }
            }

            public class TerminalFormatter : UnaryFormatter
            {
                public TerminalFormatter(Valor valor) : base(valor) { }
                public override string Flush() { return Resultado ?? (Resultado = $"({Valor.Formatter.Flush()})?"); }
            }

            public class PuroFormatter : UnaryFormatter
            {
                public PuroFormatter(Valor valor) : base(valor) { }
                public override string Flush() { return Resultado ?? (Resultado = Valor.Resultado.ToString()); }
            }

            public static FatorialFormatter Fatorial(Valor valor){ return new FatorialFormatter(valor); }
            public static RaizFormatter Raiz(Valor valor) { return new RaizFormatter(valor); }
            public static TerminalFormatter Terminal(Valor valor) { return new TerminalFormatter(valor); }
            public static PuroFormatter Puro(Valor valor) { return new PuroFormatter(valor); }
        }

        public class BinaryFormatter : IValorFormatter
        {
            private string Resultado;

            private Valor Valor1;
            private Valor Valor2;
            private string Operador;

            public BinaryFormatter(Valor valor1, Valor valor2)
            {
                Valor1 = valor1;
                Valor2 = valor2;
            }

            public string Flush()
            {
                if(Resultado == null)
                    Resultado = $"({Valor1.Formatter.Flush()} {Operador} {Valor2.Formatter.Flush()})";

                return Resultado;
            }

            public static BinaryFormatter Multiplicacao(Valor valor1, Valor valor2) { return new BinaryFormatter(valor1, valor2) { Operador = "*" }; }
            public static BinaryFormatter Potenciacao(Valor valor1, Valor valor2) { return new BinaryFormatter(valor1, valor2) { Operador = "^" }; }
            public static BinaryFormatter Soma(Valor valor1, Valor valor2) { return new BinaryFormatter(valor1, valor2) { Operador = "+" }; }
            public static BinaryFormatter Subtracao(Valor valor1, Valor valor2) { return new BinaryFormatter(valor1, valor2) { Operador = "-" }; }
            public static BinaryFormatter Divisao(Valor valor1, Valor valor2) { return new BinaryFormatter(valor1, valor2) { Operador = "/" }; }
        }
    }
}