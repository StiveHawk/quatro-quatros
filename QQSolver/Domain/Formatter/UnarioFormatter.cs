using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain.Formatter
{
    public abstract class UnarioFormatter : IValorFormatter
    {
        protected string Resultado;
        public bool ResultadoNegativo { get; protected set; }
        private Valor Valor;

        public UnarioFormatter(Valor valor, bool resultadoNegativo)
        {
            Valor = valor;
            ResultadoNegativo = resultadoNegativo;
        }

        private int? _negativacoes;
        public int Negativacoes()
        {
            if (_negativacoes == null)
            {
                var negativacoes = 0;

                if(Valor.Formatter != this) negativacoes = Valor.Formatter.Negativacoes();

                if (ResultadoNegativo)
                    _negativacoes = negativacoes + 1;
                else
                    _negativacoes = negativacoes;
            }

            return _negativacoes.Value;
        }

        public abstract string Flush();

        public class FatorialFormatter : UnarioFormatter
        {
            public FatorialFormatter(Valor valor, bool resultadoNegativo) : base(valor, resultadoNegativo) { }
            public override string Flush()
            {
                if (Resultado == null)
                {
                    if (ResultadoNegativo) Resultado = $"-(({Valor.Formatter.Flush()})!)";
                    else Resultado = $"({Valor.Formatter.Flush()})!";
                }

                return Resultado;
            }
        }

        public class RaizFormatter : UnarioFormatter
        {
            public RaizFormatter(Valor valor, bool resultadoNegativo) : base(valor, resultadoNegativo) { }
            public override string Flush()
            {
                if (Resultado == null)
                {
                    if (ResultadoNegativo) Resultado = $"(-Sqrt({Valor.Formatter.Flush()}))";
                    else Resultado = $"Sqrt({Valor.Formatter.Flush()})";
                }

                return Resultado;
            }
        }

        public class TermialFormatter : UnarioFormatter
        {
            public TermialFormatter(Valor valor, bool resultadoNegativo) : base(valor, resultadoNegativo) { }
            public override string Flush()
            {
                if (Resultado == null)
                {
                    if (ResultadoNegativo) Resultado = $"-(({Valor.Formatter.Flush()})?)";
                    else Resultado = $"({Valor.Formatter.Flush()})?";
                }

                return Resultado;
            }
        }

        public class PuroFormatter : UnarioFormatter
        {
            public PuroFormatter(Valor valor, bool resultadoNegativo) : base(valor, resultadoNegativo) { }
            public override string Flush()
            {
                if (Resultado == null)
                {
                    if (Valor.Resultado < 0) Resultado = $"({Valor.Resultado.ToString()})";
                    else Resultado = Valor.Resultado.ToString();
                }

                return Resultado;
            }
        }

        public static FatorialFormatter Fatorial(Valor valor, bool resultadoNegativo) { return new FatorialFormatter(valor, resultadoNegativo); }
        public static RaizFormatter Raiz(Valor valor, bool resultadoNegativo) { return new RaizFormatter(valor, resultadoNegativo); }
        public static TermialFormatter Termial(Valor valor, bool resultadoNegativo) { return new TermialFormatter(valor, resultadoNegativo); }
        public static PuroFormatter Puro(Valor valor, bool resultadoNegativo) { return new PuroFormatter(valor, resultadoNegativo); }
    }
}
