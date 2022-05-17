using System;

namespace program
{
    public class Interface
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

        static void Main(string[] args)
        {


            Computing.CreateColumns(7, ref firstColumn, ref secondColumn);
            for (int i = 0; i < 49; i++)
            {
                Console.WriteLine($" {firstColumn[i]}   {secondColumn[i]}");
            }
        }

        
    }
}
