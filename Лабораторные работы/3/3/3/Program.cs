// Course: Programming paradigms (C#)
// Lab 3. Cycles.
// Student: Trofimov V.A. Group: 2511
// Teacher: Povyshev V.V.
// Created 10.09.2013 Modified: 10.09.2013
// Description: Program entry point.

using System;

namespace _3
{
    class Program
    {
        static void Main(string[] args)
        {
            uint arg = 0; bool error;
            uint firstFib = 0, secondFib = 1, nextFib, count = 2;
            Console.Write("Please, enter a number: ");
            do
            {
                error = !UInt32.TryParse(Console.ReadLine(), out arg);
                if (error || arg == 0) Console.WriteLine("This is not a number or incorrect number!");
            } while (error || arg == 0);
            if (arg == 1) { Console.WriteLine("Fibonacci 1 number is 0"); return; }
            if (arg == 2) { Console.WriteLine("Fibonacci 2 number is 1"); return; }
            do 
            {
                try
                {
                    checked
                    {
                        nextFib = firstFib + secondFib; count++;
                        firstFib = secondFib; secondFib = nextFib;
                    }
                }
                catch (System.OverflowException e)
                {
                    Console.WriteLine("Sorry, but your number is too big"); return;
                }
            } while (count != arg);
            Console.WriteLine("Fibonacci {0} number is {1}", arg, secondFib);
        }
    }
}