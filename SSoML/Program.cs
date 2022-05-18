using System;

namespace program
{
    public class UI
    {
        public static int IntInput()
        {
            string strInput; bool stop = false;
            int number = -1;
            while (!stop)
            {
                try
                {
                    strInput = Console.ReadLine();
                    number = int.Parse(strInput); stop = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Не верный ввод.");
                    stop = false;
                    break;
                }
            }
            return number;
        }
    }
    class Program
    {
        int k;
        int n;
        public static int[] firstColumn;
        public static int[] secondColumn;
        public static int[] resultColumn;

        public static string formula = "2x_y";
        static void Main(string[] args)
        {
            bool check = true;
            Computing.FormulaEnter(ref check);
            if(!check)
            {
                Console.WriteLine("Неправильный ввод.");
            }
            Computing.CreateColumns(3, ref firstColumn, ref secondColumn);
            Computing.ResultColumn(3, ref resultColumn, firstColumn, secondColumn);
            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine($" {firstColumn[i]}   {secondColumn[i]}   {resultColumn[i]}");
            }
        }

        
    }
}
