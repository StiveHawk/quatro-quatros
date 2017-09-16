using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain
{
    public class HistoricoOperacao
    {
        public readonly int Aditivo;
        public readonly int Multiplicativo;
        public readonly int Exponencial;
        public readonly int Unario;

        public readonly int QuantidadeOperacoes;

        public HistoricoOperacao(int aditivo, int multiplicativo, int exponencial, int unario)
        {
            Aditivo = aditivo;
            Multiplicativo = multiplicativo;
            Exponencial = exponencial;
            Unario = unario;

            QuantidadeOperacoes = Aditivo + Multiplicativo + Exponencial + Unario;
        }

        public HistoricoOperacao Somar(HistoricoOperacao op)
        {
            return new HistoricoOperacao
            (
                this.Aditivo + op.Aditivo,
                this.Multiplicativo + op.Multiplicativo,
                this.Exponencial + op.Exponencial,
                this.Unario + op.Unario
            );
        }
        
        public HistoricoOperacao RegistrarAdicao() { return new HistoricoOperacao(Aditivo + 1, Multiplicativo, Exponencial, Unario); }
        public HistoricoOperacao RegistrarMultiplicacao() { return new HistoricoOperacao(Aditivo, Multiplicativo + 1, Exponencial, Unario); }
        public HistoricoOperacao RegistrarExponencial() { return new HistoricoOperacao(Aditivo, Multiplicativo, Exponencial + 1, Unario); }
        public HistoricoOperacao RegistrarUnario() { return new HistoricoOperacao(Aditivo, Multiplicativo, Exponencial, Unario + 1); }

        private static HistoricoOperacao _vazio;
        public static HistoricoOperacao Vazio() { return _vazio ?? (_vazio = new HistoricoOperacao(0, 0, 0, 0)); }

        private static HistoricoOperacao _unario;
        public static HistoricoOperacao OperacaoUnaria() { return _unario ?? (_unario = new HistoricoOperacao(0, 0, 0, 1)); }
    }
}
