// Course: Programming paradigms (C#)
// Lab 7. DateTime.
// Student: Trofimov V.A. Group: 2511
// Teacher: Povyshev V.V.
// Created 21.09.2013 Modified: 21.09.2013
// Description: Program entry point.

using System;
using System.Globalization;

namespace _7
{
    class Program
    {
        static DateTime ReadDateTime()
        {
            bool error; DateTime arg;
            do { error = !DateTime.TryParse(Console.ReadLine(), out arg); } while (error);
            return arg;
        }
        static DateTime ReadDateTime(string warningMessage)
        {
            bool error; DateTime arg;
            do
            {
                error = !DateTime.TryParse(Console.ReadLine(), out arg);
                if (error) Console.Write(warningMessage + " ");
            } while (error);
            return arg;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Please, enter a date with time like this: 2009-05-01 14:57:32");
            DateTime date1 = ReadDateTime("This is not a date or incorrect date!");
            Console.WriteLine("Please, enter a date with time like this: 2009-05-01 14:57:32");
            DateTime date2 = ReadDateTime("This is not a date or incorrect date!");
            TimeSpan diff = (date2 > date1) ? date2.Subtract(date1) : date1.Subtract(date2);
            DateTime diffTime = new DateTime(diff.Ticks);
            Console.WriteLine("Years between dates: " + (diffTime.Year - 1));
            Console.WriteLine("Months between dates: " + (diffTime.Year - 1) * 12);
            Console.WriteLine("Days between dates: " + (uint)diff.TotalDays);
            Console.WriteLine("Minutes between dates: " + (uint)diff.TotalMinutes);
            Console.WriteLine("Seconds between dates: " + (uint)diff.TotalSeconds);
        }
    }
}