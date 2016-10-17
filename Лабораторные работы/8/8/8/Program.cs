// Course: Programming paradigms (C#)
// Lab 8. Strings.
// Student: Trofimov V.A. Group: 2511
// Teacher: Povyshev V.V.
// Created 21.09.2013 Modified: 21.09.2013
// Description: Program entry point.

using System;
using System.Text;

namespace _8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please, write a line: ");
            string s = Console.ReadLine();
            s = s.TrimStart(' ').TrimEnd(' ');
            Console.WriteLine("\nThis string without lead and finish spaces:\n" + s);
            string[] sArray = s.Split(' ');
            Console.WriteLine("\nThis string splitted into words:");
            foreach (string i in sArray) { Console.WriteLine(i); }
            StringBuilder sb = new StringBuilder();
            foreach (string i in sArray) { sb.Append(i); sb.Append(" "); }
            Console.WriteLine("\nThis string turned back undivided:\n" + sb.ToString());
            Console.WriteLine("\nThis string in upper register:\n" + s.ToUpper());
            Console.WriteLine("\nThis string in lower register:\n" + s.ToLower());
            char[] temp = s.ToCharArray(); Array.Reverse(temp);
            Console.WriteLine("\nThis string in reverse:\n" + new string(temp));
        }
    }
}