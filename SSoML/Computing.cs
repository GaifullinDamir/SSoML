using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    public class Computing
    {
        public int K { get; set; }
        public int N { get; set; }

        private const string TD = "_"; //отрицание Поста
        private const string NP = "-."; //усечённая разность

        private static List<char> permittedSymbolsForFunction = new List<char>() { '(', ')', '_', '/', 'x', 'y' };
        private static HashSet<string> forbiddenCombs = new HashSet<string>() { "", "_)", "(_", "/_", "xy", "yx" };

        private int[] firstColumn;
        private int[] secondColumn;
        public Computing()
        {

        }
        public static int PostsNegation(int x, int k)
        {
            return (x + 1) % k;
        }

        public static int TrunDifference(int x, int y)
        {
            if (x >= y)
            {
                return x - y;
            }
            else
                return 0;
        }

        public static void CreateColumns(int k, ref int[] firstColumn)
        {
            firstColumn = new int[k];
            for (int i = 0; i < k; i++)
            {
                firstColumn[i] = i;
            }
        }

        public static void CreateColumns(int k, ref int[] firstColumn, ref int[] secondColumn)
        {
            firstColumn = new int[k*k];
            secondColumn = new int[k*k];
            int step = 0;
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    firstColumn[step] = i;
                    secondColumn[step] = j;
                    step++;
                }
            }
        }
    }
}
