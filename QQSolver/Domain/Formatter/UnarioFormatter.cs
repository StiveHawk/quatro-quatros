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
        private Valor Valor;

        public UnarioFormatter(Valor valor)
        {
            Valor = valor;
        }

        public abstract string Flush();

        public class FatorialFormatter : UnarioFormatter
        {
            private bool ResultadoNegativo;
            public FatorialFormatter(Valor valor, bool resultadoNegativo) : base(valor) { this.ResultadoNegativo = resultadoNegativo; }
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
            private bool ResultadoNegativo;
            public RaizFormatter(Valor valor, bool resultadoNegativo) : base(valor) { this.ResultadoNegativo = resultadoNegativo; }
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
            private bool ResultadoNegativo;
            public TermialFormatter(Valor valor, bool resultadoNegativo) : base(valor) { this.ResultadoNegativo = resultadoNegativo; }
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
            public PuroFormatter(Valor valor) : base(valor) { }
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
        public static PuroFormatter Puro(Valor valor) { return new PuroFormatter(valor); }
    }
}
