using System;
using System.Collections.Generic;
using System.Linq;

namespace program
{
    public class List
    {
        public char key;
        public int index;
        public List pNext;

        public static void pop(List list, int index) // для работы со списком (удаление)
        {
            List current = list;
            List previous = current;
            while (current.index != index)
            {
                previous = current;
                current = current.pNext;
            }
            previous.pNext = current.pNext;
        }
    }

    class Program
    {
        private static List<char> permittedSymbolsForFunction = new List<char>() { '(', ')', '_', '/', 'x', 'y' };
        private static HashSet<string> forbiddenCombs = new HashSet<string>() { "", "_)", "xy", "yx", "xx", "yy", "//", "__" };

        public static int n; // кол во значимых переменных
        public static int k;
        public static string inputFormula; // основная формула( не изменяется)
        public static string formula; // формула которую мы постепенно упрощаем для получения результата
        public static int[] valueXColumn;
        public static int[] valueYColumn;
        public static int[] resultsColumn;
        public static int resultsCount;
        public static int[] setArray;
        public static int countNumbers = 0;
        public static List bracketsList; // список для скобочек вместе с их индексами
        public static int countLeftBrackets = 0; // число (
        public static int countRightBrackets = 0; // число )
        public static int countBrackets = 0; // общее число )(

        static void Main(string[] args)
        {
            ShowMenu();
            bool run = true;
            while (run)
            {

                while (k < 1 || k > 7)
                {
                    Console.WriteLine("Введите  1 < k < 8");
                    k = InputInteger();
                }

                while (n < 1 || n > 2)
                {
                    Console.WriteLine("Введите n (1 или 2)");
                    n = InputInteger();
                }

                valueXColumn = new int[k];
                valueYColumn = new int[k];
                resultsCount = 0;
                resultsColumn = new int[k * k];
                for (int i = 0; i < k; i++)
                {
                    valueXColumn[i] = i;
                    valueYColumn[i] = i;
                }
                Console.Write("Введите основную формулу в соответствии с заданным вводом функции: ");
                bool stop = false;
                while (!stop)
                {
                    inputFormula = Console.ReadLine();
                    for (int i = 0; i < inputFormula.Length - 1; i++)
                    {
                        if (forbiddenCombs.Contains(inputFormula[i].ToString() + inputFormula[i + 1].ToString()))
                        {
                            Console.WriteLine("Неверный ввод.");
                        }
                    }
                    if (!(inputFormula.Contains("x") || inputFormula.Contains("y") || inputFormula.Contains("/") || inputFormula.Contains("_")))
                    {
                        Console.WriteLine("Неверный ввод.");
                        continue;
                    }
                    if (n == 1 && (inputFormula.Contains('x')) && (inputFormula.Contains('y')))
                    {
                        Console.WriteLine("Вы должны ввести функцю одной переменной!");
                    }
                    else
                        stop = true;
                }
                
                Calculate();
                CheckSet();

                Console.WriteLine("Продолжить работу? 0 - да, 1 - нет."); int answer = InputInteger();
                if (answer == 1)
                {
                    run = false;
                }
            }



        }
        public static void ShowMenu()
        {
            Console.WriteLine("Работу выполнил студент группы 4211 Гайфуллин Дамир Равильевич\n"
                            + "\ng = 11, n = 3 -> (g+n-1)mod6 + 1 = 2; (g+n-1)mod7 + 1 = 7; (g+n-1)mod3 + 1 = 2\n"
                            + "\nФункция одного аргумента: _x - отрицание Поста"
                            + "\nФункция двух аргументов: x/y - усеченная разность "
                            + "\nСтандартная форма представления функции: вторая форма\n"
                            + "\nВвод функций: _x - отрицание Поста"
                            + "\nx/y - усечённая разность.\n");
        }
        public static int InputInteger()
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

        public static void Calculate()
        {
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    int x = valueXColumn[i];
                    int y = valueYColumn[j];
                    int result = -1;
                    formula = inputFormula;

                    while (formula.Length != 1) // начинаем вычисление для x = i, y = j
                    {
                        AddBrackets(ref formula);
                        BracketsCount(formula); // всегда обновляем кол-во скобочек
                        List current = bracketsList;
                        while (current.pNext.key != ')')
                        {
                            current = current.pNext;
                        }
                        string MicroFormula = "";
                        for (int t = current.index + 1; t < current.pNext.index; t++)
                        {
                            MicroFormula += formula[t]; // определяем текущее действие(по индексам скобочек)
                        }

                        if (MicroFormula.Contains("(") || MicroFormula.Contains(")"))
                        {
                            Console.WriteLine("Ошибка ввода.");
                            return;
                        }
                        if (MicroFormula.Length == 1) // если действие состоит из одного элемента, определяем значение
                        {
                            if (MicroFormula == "x")
                            {
                                result = x;
                            }
                            else if (MicroFormula == "y")
                            {
                                result = y;
                            }
                        }
                        else if (MicroFormula[0] == '_') // если действие J, вызываем соотв. функцию
                        {
                            if (MicroFormula.Length != 2)
                            {
                                Console.WriteLine("Ошибка ввода.");
                                return;
                            }
                            if (MicroFormula[1] == 'x')
                            {
                                result = DenialOfPost(x, k);
                            }
                            else if (MicroFormula[1] == 'y')
                            {
                                result = DenialOfPost(y, k);
                            }
                            else if (Char.IsDigit(MicroFormula[1]))
                            {
                                result = DenialOfPost(MicroFormula[1] - '0', k);
                            }
                        }

                        else // иначе вызываем x=>y
                        {
                            if (MicroFormula.Length != 3)
                            {
                                Console.WriteLine("Ошибка ввода.");
                                return;
                            }
                            result = TrunDifference(x, y, MicroFormula, k);

                        }
                        formula = formula.Remove(current.index, current.pNext.index - current.index + 1); // удаляем выполненное действие
                        formula = formula.Insert(current.index, result.ToString()); // вставляем полученное в действии значение
                        List.pop(bracketsList, current.index); // удаляем скобочки
                        List.pop(bracketsList, current.pNext.index);
                        countBrackets -= 2;
                    }
                    resultsColumn[resultsCount] = result; // заносим в массив результатов
                    resultsCount += 1;
                }
            }
            if (n == 2)
            {
                PrintTwoVariable(valueXColumn, valueYColumn, resultsColumn);
                SecondFormTwoVariables(valueXColumn, valueYColumn, resultsColumn);
            }
            else
            {
                PrintOneVariable(valueXColumn, resultsColumn);
                SecondFormOneVariable(valueXColumn, resultsColumn);
            }
        }
        #region Calculate methods
        public static int TrunDifference(int x, int y, string formula, int k)
        {
            if (formula[0] == 'x' && formula[2] == 'x')
            {
                return 0;
            }
            else if (formula[0] == 'x' && formula[2] == 'y')
            {
                if (x >= y)
                {
                    return x - y;
                }
                else
                {
                    return 0;
                }
            }
            else if (formula[0] == 'x' && char.IsDigit(formula[2]))
            {
                if (x >= formula[2] - '0')
                {
                    return x - (formula[2] - '0');
                }
                else
                {
                    return 0;
                }
            }
            else if (formula[0] == 'y' && formula[2] == 'x')
            {
                if (y >= x)
                {
                    return y - x;
                }
                else
                {
                    return 0;
                }
            }
            else if (formula[0] == 'y' && formula[2] == 'y')
            {
                return 0;
            }
            else if (formula[0] == 'y' && char.IsDigit(formula[2]))
            {
                if (y >= formula[2] - '0')
                {
                    return y - (formula[2] - '0');
                }
                else
                {
                    return 0;
                }
            }
            else if (char.IsDigit(formula[0]) && formula[2] == 'x')
            {
                if (formula[0] - '0' >= x)
                {
                    return (formula[0] - '0') - x;
                }
                else
                {
                    return 0;
                }
            }
            else if (char.IsDigit(formula[0]) && formula[2] == 'y')
            {
                if (formula[0] - '0' >= y)
                {
                    return formula[0] - '0' - y;
                }
                else
                {
                    return 0;
                }
            }
            else if (char.IsDigit(formula[0]) && char.IsDigit(formula[2]))
            {
                if (formula[0] - '0' >= formula[2] - '0')
                {
                    return (formula[0] - '0') - (formula[2] - '0');
                }
                else
                {
                    return 0;
                }
            }
            return -1;
        } // нахождение x/y
        public static int DenialOfPost(int x, int k)
        {
            return (x + 1) % k;
        } // _x

        #endregion
        public static void AddBrackets(ref string formula)
        {

            for (int l = 0; l < formula.Length - 1; l++)
            {
                if (formula[l] == '_' && formula[l + 1] != '(')
                {
                    if ((!formula.Contains("(_x)")) && (!formula.Contains("(_y)")) && (!formula.Contains($"(_{Char.IsDigit(formula[l + 1])})")))
                    {
                        formula = formula.Insert(l, "(");
                        formula = formula.Insert(l + 3, ")");
                        if (!formula.Contains("/x/") && !formula.Contains("/y/"))
                        {
                            return;

                        }
                    }
                }
            }

            if (formula.Length > 4)
            {
                for (int l = 0; l < formula.Length - 2; l++)
                {
                    if (formula[l] == '/' && formula[l + 2] == '/')
                    {
                        formula = formula.Insert(l - 1, "(");
                        formula = formula.Insert(l + 3, ")");
                        l = formula.Length - 2;
                    }
                }
            }
            else if (formula[0] != '(')
            {
                formula = formula.Insert(formula.Length, ")");
                formula = formula.Insert(0, "(");
            }
        }

        public static void BracketsCount(string formula) // подсчет скобок
        {
            bracketsList = new List();
            bracketsList.pNext = null;
            List current = bracketsList;
            for (int i = 0; i < formula.Length; i++)
            {
                if (formula[i] == '(')
                {
                    while (current.pNext != null)
                    {
                        current = current.pNext;
                    }
                    List pTemp = new List();
                    current.pNext = pTemp;
                    pTemp.key = '(';
                    pTemp.pNext = null;
                    pTemp.index = i;
                    countLeftBrackets += 1;
                    countBrackets++;
                }
                else if (formula[i] == ')')
                {
                    while (current.pNext != null)
                    {
                        current = current.pNext;
                    }
                    List pTemp = new List();
                    current.pNext = pTemp;
                    pTemp.key = ')';
                    pTemp.pNext = null;
                    pTemp.index = i;
                    countRightBrackets += 1;
                    countBrackets++;
                }
            }
        }

        public static void PrintTwoVariable(int[] valuex, int[] valuey, int[] results) // вывод для двух перменных
        {
            resultsCount = 0;
            Console.WriteLine($"| X | Y | {inputFormula} |");
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    Console.Write($"| {valuex[i]} | {valuey[j]} | {String.Concat(Enumerable.Repeat(" ", inputFormula.Length / 2))}" +
                        $"{results[resultsCount]}{String.Concat(Enumerable.Repeat(" ", inputFormula.Length / 2 - 1))} |");
                    resultsCount += 1;
                    Console.WriteLine();
                }
            }
        }
        public static void PrintOneVariable(int[] valuex, int[] results) // вывод для одной переменной
        {
            resultsCount = 0;
            Console.WriteLine($"| X | {inputFormula} |");
            for (int i = 0; i < k; i++)
            {
                Console.Write($"| {valuex[i]} | {String.Concat(Enumerable.Repeat(" ", inputFormula.Length / 2))}{results[resultsCount]}{String.Concat(Enumerable.Repeat(" ", inputFormula.Length / 2 - 1))} |");
                resultsCount += k;
                Console.WriteLine();
            }
        }

        public static void SecondFormTwoVariables(int[] valuex, int[] valuey, int[] results)
        {
            resultsCount = 0;
            int current;
            string form = "";
            Console.WriteLine("\n2я форма (один из аналогов полинома Жегалкина):");
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    current = results[resultsCount];
                    if (current != 0)
                    {
                        if (Math.Abs(current - k) < current)
                        {
                            form += " - ";
                            if (Math.Abs(current - k) == 1)
                            {
                                form += $"j{i}(x)*j{j}(y)";
                            }
                            else
                            {
                                form += $"{Math.Abs(current - k)}*j{i}(x)*j{j}(y)";
                            }
                        }
                        else
                        {
                            form += " + ";
                            if (current == 1)
                            {
                                form += $"j{i}(x)*j{j}(y)";
                            }
                            else
                            {
                                form += $"{current}*j{i}(x)*j{j}(y)";
                            }
                        }
                    }
                    resultsCount++;
                }
            }
            Console.WriteLine(form + "\n");
        }

        public static void SecondFormOneVariable(int[] valuex, int[] results)
        {
            resultsCount = 0;
            int current;
            string form = "";
            Console.WriteLine("\n2-я форма:");
            for (int i = 0; i < k; i++)
            {
                current = results[resultsCount];
                if (current != 0)
                {
                    if (Math.Abs(current - k) < current)
                    {
                        form += " - ";
                        if (Math.Abs(current - k) == 1)
                        {
                            form += $"j{i}(x)";
                        }
                        else
                        {
                            form += $"{Math.Abs(current - k)}*j{i}(x)";
                        }
                    }
                    else
                    {
                        form += " + ";
                        if (current == 1)
                        {
                            form += $"j{i}(x)";
                        }
                        else
                        {
                            form += $"{current}*j{i}(x)";
                        }
                    }
                }
                resultsCount++;
            }
            Console.WriteLine(form + "\n");
        }

        public static void CheckSet()
        {
            Console.WriteLine($"\nВведите множество для проверки через пробел (0 2 5): ");
            var setArray = Console.ReadLine()
                .Split(new char[] {' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x=> Convert.ToInt32(x))
                .Distinct();

            resultsCount = 0;
            int saveCount = 0;
            Console.WriteLine($"Строки, сохраняющие множество:\n| X | Y | f(x,y)");
            if (n == 2)
            {
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < k; j++)
                    {
                        if (setArray.Contains(valueXColumn[i]) && setArray.Contains(valueYColumn[j])/* && setArray.Contains(resultsColumn[resultsCount])*/)
                        {
                            if (setArray.Contains(resultsColumn[resultsCount]))
                            {
                                Console.WriteLine($"| {valueXColumn[i]} | {valueYColumn[j]} | {resultsColumn[resultsCount]}");
                                saveCount++;
                            }
                            else
                            {
                                Console.WriteLine("Множество не созраняется.");
                                return;
                            }
                        }
                        resultsCount += 1;
                    }
                }
                if (resultsCount == k*k)
                {
                    Console.WriteLine("Множество сохраняется");
                }
            }
            else
            {
                for (int j = 0; j < k; j++)
                {
                    if (setArray.Contains(valueXColumn[j]))
                    {
                        if(setArray.Contains(resultsColumn[resultsCount]))
                        {
                            Console.WriteLine($"| {valueXColumn[j]} | {resultsColumn[resultsCount]}");
                            saveCount++;
                        }
                        else
                        {
                            Console.WriteLine("Множество не созраняется.");
                            return;
                        }
                    }
                    resultsCount += k;
                }
                if (resultsCount == k )
                {
                    Console.WriteLine("Множество сохраняется");
                }
            }
            //Console.ReadKey();
        }
    }
}
