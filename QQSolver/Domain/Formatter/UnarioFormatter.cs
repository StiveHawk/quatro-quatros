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
            public FatorialFormatter(Valor valor) : base(valor) { }
            public override string Flush() { return Resultado ?? (Resultado = $"({Valor.Formatter.Flush()})!"); }
        }

        public class RaizFormatter : UnarioFormatter
        {
            public RaizFormatter(Valor valor) : base(valor) { }
            public override string Flush() { return Resultado ?? (Resultado = $"Sqrt({Valor.Formatter.Flush()})"); }
        }

        public class TermialFormatter : UnarioFormatter
        {
            public TermialFormatter(Valor valor) : base(valor) { }
            public override string Flush() { return Resultado ?? (Resultado = $"({Valor.Formatter.Flush()})?"); }
        }

        public class PuroFormatter : UnarioFormatter
        {
            public PuroFormatter(Valor valor) : base(valor) { }
            public override string Flush() { return Resultado ?? (Resultado = Valor.Resultado.ToString()); }
        }

        public static FatorialFormatter Fatorial(Valor valor) { return new FatorialFormatter(valor); }
        public static RaizFormatter Raiz(Valor valor) { return new RaizFormatter(valor); }
        public static TermialFormatter Termial(Valor valor) { return new TermialFormatter(valor); }
        public static PuroFormatter Puro(Valor valor) { return new PuroFormatter(valor); }
    }
}
