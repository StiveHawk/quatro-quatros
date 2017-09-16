using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQSolver.Domain
{
    public sealed class Dicionarios
    {
        private Dicionarios() { }

        public static Dictionary<long, long> FatorialHashSet = CriarFatorialHashSet();
        private static Dictionary<long, long> CriarFatorialHashSet()
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
            catch (OverflowException e) { }

            return dic;
        }

        public static Dictionary<int, int> TermialHashSet = CriarTermialHashSet();
        private static Dictionary<int, int> CriarTermialHashSet()
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

            return dic;
        }
    }
}
