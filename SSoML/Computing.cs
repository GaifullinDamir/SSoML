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
        private static HashSet<string> forbiddenCombs = new HashSet<string>() { "", "_)", "(_", "/_", "xy", "yx", "xx", "yy" };

        public Computing()
        {

        }

        public static int Multiple(int x, int factor, int k)
        {
            return (factor * x) % k;
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

        public static void ResultColumn(int k, ref int[] resultColumn, int[] firstColumn, int[] secondColumn)
        {

            resultColumn = new int[k * k];
            for (int i = 0; i < k * k; i++)
            {
                resultColumn[i] = TrunDifference(Multiple(firstColumn[i], 2, k), secondColumn[i]);
            }
        }

        public static void FormulaEnter(ref bool check)
        {
            Console.Write("Введите число переменных (1/2): "); int vars = UI.IntInput();
            Console.Write("Введите формулу: "); string strFormula = Console.ReadLine();
            int leftBracket = 0;
            int rightBracket = 0;
            List<char> formula = new List<char>();
            for (int i = 0; i < strFormula.Length; i++)
            {
                formula.Add(strFormula[i]);
            }
            for (int j = 0; j < formula.Count; j++)
            {
                Console.WriteLine(formula[j]);
            }
            for (int z = 0; z < formula.Count - 1; z++)
            {
                if (forbiddenCombs.Contains(formula[z].ToString() + formula[z + 1].ToString()))
                {
                    check = false;
                    return;
                }
            }
            if (formula.Count == 0)
            {
                check = false;
                return;
            }

            for (int i = 0; i < formula.Count; i++)
            {
                if (formula[i] == '(')
                {
                    leftBracket++;
                }
                else if (formula[i] == ')')
                {
                    rightBracket++;
                }
            }
            if (leftBracket != rightBracket)
            {
                check = false;
                return;
            }
            if(vars == 1 && formula.Contains('x') && formula.Contains('y'))
            {
                check = false;
                return;
            }
            for (int i = 0; i < formula.Count; i++)
            {
                if (!(Char.IsDigit(formula[i])) && !(permittedSymbolsForFunction.Contains(formula[i])))
                {
                    check = false;
                    return;
                }
            }

            SortedDictionary<int, int> bracketsDict = new SortedDictionary<int, int>();
            var keysList = bracketsDict.Keys.ToList();
            int current = 0;
            for (int i = 0; i < formula.Count; i++)
            {
                if(formula[i] == '(')
                {
                    current = i;
                    bracketsDict.Add(current, -1);
                }
                else if (formula[i] == ')')
                {
                    bracketsDict[current] = i;
                    keysList = bracketsDict.Keys.ToList();
                    if(current != keysList[0])
                    {
                        current = keysList[keysList.IndexOf(current) - 1];
                    }
                }
                
            }
            if (bracketsDict.Values.ToList().Contains(-1))
            {
                check = false;
                return;
            }
        }   
            

    }
}
