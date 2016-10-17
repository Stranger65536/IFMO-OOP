// Course: Programming paradigms (C#)
// Lab 4. Exceptions.
// Student: Trofimov V.A. Group: 2511
// Teacher: Povyshev V.V.
// Created 10.09.2013 Modified: 10.09.2013
// Description: Program entry point.

using System;

namespace _4
{
    class Program
    {
        static double Func(double arg)
        {
            return 3 - 1 / Math.Sqrt(arg) + 2 / arg;
        }
        static void Main(string[] args)
        {
            bool error; double arg, res;
            Console.WriteLine("Please enter an argument:");
            do
            {
                if (error = !Double.TryParse(Console.ReadLine(), out arg))
                    Console.WriteLine("This is not a number or incorrect number. Try harder.");
            } while (error);
            try
            {
                res = Func(arg);
                if (Double.IsNaN(res)) throw new ArithmeticException();
            }
            catch (ArithmeticException aE) { Console.WriteLine("Invalid operation"); return; } 
                Console.WriteLine(res);
        }
    }
}