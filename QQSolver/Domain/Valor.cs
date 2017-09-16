using QQSolver.Domain.Formatter;
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

            this.Inteiro = (Resultado > int.MinValue && Resultado < int.MaxValue && Resultado % 1 == 0);
            this.Positivo = (Resultado > 0);
            this.Puro = (NivelOperacoes == 0);
        }

        private IEnumerable<Valor> ListarOperacoesUnarias()
        {
            if (!Puro)
            {
                if (Positivo && Inteiro)
                {
                    if (Simulador.Operacoes.Fatorial && Resultado < long.MaxValue && Resultado > 2)
                    {
                        long lAux;
                        var resultadoLong = (long)Resultado;

                        if (Dicionarios.FatorialHashSet.TryGetValue(resultadoLong, out lAux))
                            yield return new Valor(Peso, lAux, NivelOperacoes + 1).FormatterFatorial(this);
                    }

                    if (Simulador.Operacoes.Terminal && Resultado < int.MaxValue && Resultado > 1)
                    {
                        int iAux;
                        var resultadoInt = (int)Resultado;

                        if (Dicionarios.TermialHashSet.TryGetValue(resultadoInt, out iAux) && iAux != Resultado)
                            yield return new Valor(Peso, iAux, NivelOperacoes + 1).FormatterTermial(this);
                    }
                }
            }
        }

        public Valor FormatterPuro() { this.Formatter = new UnarioFormatter.PuroFormatter(this); return this; }
        public Valor FormatterFatorial(Valor original) { this.Formatter = new UnarioFormatter.FatorialFormatter(original); return this; }
        public Valor FormatterTermial(Valor original) { this.Formatter = new UnarioFormatter.TermialFormatter(original); return this; }
        public Valor FormatterRaiz(Valor original) { this.Formatter = new UnarioFormatter.RaizFormatter(original); return this; }

        public Valor FormatterSoma(Valor v1, Valor v2) { this.Formatter = BinarioFormatter.Soma(v1, v2); return this; }
        public Valor FormatterDivisao(Valor v1, Valor v2) { this.Formatter = BinarioFormatter.Divisao(v1, v2); return this; }
        public Valor FormatterMultiplicacao(Valor v1, Valor v2) { this.Formatter = BinarioFormatter.Multiplicacao(v1, v2); return this; }
        public Valor FormatterPotenciacao(Valor v1, Valor v2) { this.Formatter = BinarioFormatter.Potenciacao(v1, v2); return this; }
        public Valor FormatterSubtracao(Valor v1, Valor v2) { this.Formatter = BinarioFormatter.Subtracao(v1, v2); return this; }


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
    }
}