﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain.Formatter
{
    public class BinarioFormatter : IValorFormatter
    {
        private string Resultado;

        private Valor Valor1;
        private Valor Valor2;
        private string Operador;
        private bool ResultadoNegativo;

        public BinarioFormatter(Valor valor1, Valor valor2)
        {
            Valor1 = valor1;
            Valor2 = valor2;
        }

        public string Flush()
        {
            if (Resultado == null)
            {
                if(ResultadoNegativo)
                    Resultado = $"-({Valor1.Formatter.Flush()} {Operador} {Valor2.Formatter.Flush()})";
                else
                    Resultado = $"({Valor1.Formatter.Flush()} {Operador} {Valor2.Formatter.Flush()})";
            }

            return Resultado;
        }

        public static BinarioFormatter Multiplicacao(Valor valor1, Valor valor2, bool resultadoNegativo) { return new BinarioFormatter(valor1, valor2) { Operador = "*", ResultadoNegativo = resultadoNegativo }; }
        public static BinarioFormatter Potenciacao(Valor valor1, Valor valor2, bool resultadoNegativo) { return new BinarioFormatter(valor1, valor2) { Operador = "^", ResultadoNegativo = resultadoNegativo }; }
        public static BinarioFormatter Soma(Valor valor1, Valor valor2, bool resultadoNegativo) { return new BinarioFormatter(valor1, valor2) { Operador = "+", ResultadoNegativo = resultadoNegativo }; }
        public static BinarioFormatter Divisao(Valor valor1, Valor valor2, bool resultadoNegativo) { return new BinarioFormatter(valor1, valor2) { Operador = "/", ResultadoNegativo = resultadoNegativo }; }
        //public static BinarioFormatter Subtracao(Valor valor1, Valor valor2, bool resultadoNegativo) { return new BinarioFormatter(valor1, valor2) { Operador = "-" }; }
    }
}
