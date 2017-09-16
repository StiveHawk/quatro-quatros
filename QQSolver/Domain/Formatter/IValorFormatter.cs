using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain.Formatter
{
    public interface IValorFormatter
    {
        bool ResultadoNegativo { get; }
        string Flush();
        int Negativacoes();
    }
}
