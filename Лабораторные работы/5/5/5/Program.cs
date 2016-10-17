// Course: Programming paradigms (C#)
// Lab 5. Methods and arguments.
// Student: Trofimov V.A. Group: 2511
// Teacher: Povyshev V.V.
// Created 21.09.2013 Modified: 21.09.2013
// Description: Program entry point.

using System;

namespace _5
{
    class Utils
    {
        private Utils() { }
        public static double ReadDouble() 
        {
            bool error; double arg;
            do { error = !Double.TryParse(Console.ReadLine(), out arg); } while (error || arg == 0);
            return arg;
        }
        public static double ReadDouble(string warningMessage)
        {
            bool error; double arg;
            do
            {
                error = !Double.TryParse(Console.ReadLine(), out arg);
                if (error || arg == 0) Console.Write(warningMessage + " ");
            } while (error || arg == 0);
            return arg;
        }
    }
    struct Circle
    {
        public struct Point
        {
            public double X, Y;
            public Point(double X, double Y)
            {
                this.X = X; this.Y = Y;
            }
        }
        public Point Position { get; set; }
        private double radius;
        public double Radius
        {
            get { return radius; }
            set { radius = ((value > 0) ? value : 0d); }
        }
        public double Length() { return 2 * Math.PI * radius; }
        public double Square() { return Math.PI * Math.Pow(radius, 2); }
    }
    struct Additional
    {
        public static decimal Factorial(uint arg)
        {
            if (arg == 0) return 1;
            decimal res = arg;
            checked { for (uint i = arg - 1; i > 0; i--) { res *= i; } }
            return res;
        }
        public static decimal RecursionFactorial(uint arg)
        {
            if (arg == 0) return 1;
            return arg * RecursionFactorial(arg - 1);
        }
        public static long Sum(long first, long second, params long[] values)
        {
            unchecked
            {
                long res = first + second;
                for (int i = 0; i < values.Length; i++) res += values[i];
                return res;
            }
        }
        public static void Sum(out long res, long first, long second, params long[] values)
        {
            unchecked
            {
                res = first + second;
                for (int i = 0; i < values.Length; i++) res += values[i];
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Circle c = new Circle(); 
            Console.Write("Please, enter an X-coordinate of circle position: ");
            double x = Utils.ReadDouble("This is not a number or incorrect number!");
            Console.Write("Please, enter an Y-coordinate of circle position: ");
            double y = Utils.ReadDouble("This is not a number or incorrect number!");
            Console.Write("Please, enter a radius of circle: ");
            double radius = Utils.ReadDouble("This is not a number or incorrect number!");
            c.Position = new Circle.Point(x, y); c.Radius = radius;
            Console.Write("Circle with coordinates ({0}, {1}) ", c.Position.X, c.Position.Y);
            Console.Write("has a length of {0} and a square of {1}\n", c.Length(), c.Square());
            try { Console.Write("27! is "); Console.WriteLine(Additional.Factorial(27)); }
            catch (OverflowException e) { Console.WriteLine(e.Message); }
            try { Console.Write("28! is "); Console.WriteLine(Additional.Factorial(28)); }
            catch (OverflowException e) { Console.WriteLine(e.Message); }
            try { Console.Write("27! is "); Console.WriteLine(Additional.RecursionFactorial(27)); }
            catch (OverflowException e) { Console.WriteLine(e.Message); }
            catch (StackOverflowException e) { Console.WriteLine(e.Message); }
            try { Console.Write("28! is "); Console.WriteLine(Additional.RecursionFactorial(28)); }
            catch (OverflowException e) { Console.WriteLine(e.Message); }
            catch (StackOverflowException e) { Console.WriteLine(e.Message); }
            Console.WriteLine("512 + 256 is " + Additional.Sum(512, 256));
            Console.Write("1024 + 512 + 256 + 128 + 64 + 32 + 16 + 8 + 4 + 2 + 1 is "); 
            Console.WriteLine(Additional.Sum(1024, 512, 256, 128, 64, 32, 16, 8, 4, 2, 1));
            long res = 0; Additional.Sum(out res, 1, 2);
            Console.WriteLine("1 + 2 is " + res);
        }
    }
}