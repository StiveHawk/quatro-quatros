using QQSolver.Domain.Formatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain
{
    public class Valor : IComparable<Valor>
    {
        public int Peso;
        public double Resultado;
        public HistoricoOperacao HistoricoOperacao;

        public bool Inteiro;
        public bool Positivo;
        public bool Puro;

        public IValorFormatter Formatter;
        public string OperacaoExtenso {  get { return Formatter.Flush(); } }



        public Valor(int representacao, double resultado, HistoricoOperacao historicoOperacao)
        {
            this.Peso = representacao;
            this.Resultado = resultado;
            this.HistoricoOperacao = historicoOperacao;

            this.Inteiro = (Resultado > int.MinValue && Resultado < int.MaxValue && Resultado % 1 == 0);
            this.Positivo = (Resultado > 0);
            this.Puro = (HistoricoOperacao.QuantidadeOperacoes == 0);
        }

        public int CompareTo(Valor valor)
        {
            // 1 = Mais complexo
            // -1 = Menos complexo

            if (HistoricoOperacao.QuantidadeOperacoes < valor.HistoricoOperacao.QuantidadeOperacoes) return -1;
            if (HistoricoOperacao.QuantidadeOperacoes > valor.HistoricoOperacao.QuantidadeOperacoes) return 1;

            if (HistoricoOperacao.Unario < valor.HistoricoOperacao.Unario) return -1;
            if (HistoricoOperacao.Unario > valor.HistoricoOperacao.Unario) return 1;

            if (HistoricoOperacao.Exponencial < valor.HistoricoOperacao.Exponencial) return -1;
            if (HistoricoOperacao.Exponencial > valor.HistoricoOperacao.Exponencial) return 1;

            if (HistoricoOperacao.Multiplicativo < valor.HistoricoOperacao.Multiplicativo) return -1;
            if (HistoricoOperacao.Multiplicativo > valor.HistoricoOperacao.Multiplicativo) return 1;

            if (Formatter.Negativacoes() < valor.Formatter.Negativacoes()) return -1;
            if (Formatter.Negativacoes() > valor.Formatter.Negativacoes()) return 1;

            if (HistoricoOperacao.Aditivo < valor.HistoricoOperacao.Aditivo) return -1;
            if (HistoricoOperacao.Aditivo > valor.HistoricoOperacao.Aditivo) return 1;

            return 0;
        }

        public bool MenosComplexoQue(Valor valor)
        {
            return CompareTo(valor) == -1;
        }

        private IEnumerable<Valor> ListarOperacoesUnarias()
        {
            if (!Puro)
            {
                if (Positivo && Inteiro)
                {
                    var historico = HistoricoOperacao.RegistrarUnario();

                    if (Simulador.Operacoes.Fatorial && Resultado < long.MaxValue && Resultado > 2)
                    {
                        long lAux;
                        var resultadoLong = (long)Resultado;

                        if (Dicionarios.FatorialHashSet.TryGetValue(resultadoLong, out lAux))
                        {
                            yield return new Valor(Peso, lAux, historico).FormatterFatorial(this, false);
                            yield return new Valor(Peso, -lAux, historico).FormatterFatorial(this, true);
                        }
                    }

                    if (Simulador.Operacoes.Terminal && Resultado < int.MaxValue && Resultado > 1)
                    {
                        int iAux;
                        var resultadoInt = (int)Resultado;

                        if (Dicionarios.TermialHashSet.TryGetValue(resultadoInt, out iAux) && iAux != Resultado)
                        {
                            yield return new Valor(Peso, iAux, historico).FormatterTermial(this, false);
                            yield return new Valor(Peso, -iAux, historico).FormatterTermial(this, true);
                        }
                    }
                }
            }
        }

        public Valor FormatterPuro(bool resultadoNegativo) { this.Formatter = new UnarioFormatter.PuroFormatter(this, resultadoNegativo); return this; }
        public Valor FormatterFatorial(Valor original, bool resultadoNegativo) { this.Formatter = new UnarioFormatter.FatorialFormatter(original, resultadoNegativo); return this; }
        public Valor FormatterTermial(Valor original, bool resultadoNegativo) { this.Formatter = new UnarioFormatter.TermialFormatter(original, resultadoNegativo); return this; }
        public Valor FormatterRaiz(Valor original, bool resultadoNegativo) { this.Formatter = new UnarioFormatter.RaizFormatter(original, resultadoNegativo); return this; }

        public Valor FormatterSoma(Valor v1, Valor v2, bool resultadoNegativo) { this.Formatter = BinarioFormatter.Soma(v1, v2, resultadoNegativo); return this; }
        public Valor FormatterDivisao(Valor v1, Valor v2, bool resultadoNegativo) { this.Formatter = BinarioFormatter.Divisao(v1, v2, resultadoNegativo); return this; }
        public Valor FormatterMultiplicacao(Valor v1, Valor v2, bool resultadoNegativo) { this.Formatter = BinarioFormatter.Multiplicacao(v1, v2, resultadoNegativo); return this; }
        public Valor FormatterPotenciacao(Valor v1, Valor v2, bool resultadoNegativo) { this.Formatter = BinarioFormatter.Potenciacao(v1, v2, resultadoNegativo); return this; }
        //public Valor FormatterSubtracao(Valor v1, Valor v2, bool resultadoNegativo) { this.Formatter = BinarioFormatter.Subtracao(v1, v2, resultadoNegativo); return this; }


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