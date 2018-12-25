using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace first_.NET_Core_app
{
    class Calculator
    {
        //private string expression;

        //public Calculator(string expression) => this.expression = expression;

        public double? GetOperationRes(List<string> subEx)
        {
            int indexSign = GetIndexSign(subEx)[0];

            double a = double.Parse(string.Join("", subEx.GetRange(0, indexSign)));

            int elemAfterSign = subEx.Count - (indexSign + 1);

            double b = double.Parse(string.Join("", subEx.GetRange(indexSign + 1, elemAfterSign)));

            double result = 0;

            switch (subEx[indexSign])
            {
                case "+": { result = a + b; break; }
                case "-": { result = a - b; break; }
                case "/": { result = a / b; break; }
                case "*": { result = a * b; break; }

                default:
                    break;
            }

            return result;
        }

        public int[] GetIndexSign(List<string> expression)
        {
            int[] signIndexes = new int[3];
            int counter = 0;

            for (int i = 0; i < expression.Count; i++)
            {
                if (i != 0 && double.TryParse(expression[i - 1], out double res) && counter < 3)
                {
                    signIndexes[counter] = i;
                    counter++;
                }
            }
            return signIndexes;
        }

        public double GetResultExpression(string ex)
        {
            string pattern = @"([+\-*/])";
            ex = ex.Replace(" ", string.Empty);

            var tokens = Regex.Split(ex, pattern).Where(i => !string.IsNullOrEmpty(i)).ToList();

            int endIndex = 0;

            while (tokens.Count != 1)
            {
                int[] signIndexes = GetIndexSign(tokens);

                string firstSign = tokens[signIndexes[0]];
                string secondSign = tokens[signIndexes[1]];
                List<string> subEx;

                if ((secondSign == "/" || secondSign == "*") && (firstSign != "/" && firstSign != "*"))
                {
                    endIndex = signIndexes[2] == 0 ? tokens.Count - signIndexes[0] - 1 : signIndexes[2] - signIndexes[0] - 1;

                    subEx = tokens.GetRange(signIndexes[0] + 1, endIndex);

                    tokens.RemoveRange(signIndexes[0] + 2, endIndex - 1);

                    tokens[signIndexes[0] + 1] = GetOperationRes(subEx).ToString();
                    Console.WriteLine(string.Join(" ", tokens));
                }
                else
                {
                    endIndex = signIndexes[1] == 0 ? tokens.Count : signIndexes[1];

                    subEx = tokens.GetRange(0, endIndex);

                    tokens.RemoveRange(1, endIndex - 1);

                    tokens[0] = GetOperationRes(subEx).ToString();
                    Console.WriteLine(string.Join(" ", tokens));
                }
            }
            return Convert.ToDouble(tokens[0]);
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            Console.WriteLine("Expample: 2+5*4");
            Console.WriteLine("Also can work with long expression like: -5-5+5*2/-2,5+5-3/3+1/2*4-33*3/23+4*3+3*2/4*2*2/5*100*12/123*12/234*123*123");

            Console.Write("\nEnter your expression: ");

            string expression = Console.ReadLine();
            
            Calculator calculator = new Calculator();

           Console.WriteLine($"Result: {calculator.GetResultExpression(expression)}");

            Console.ReadLine();
        }
    }
}
