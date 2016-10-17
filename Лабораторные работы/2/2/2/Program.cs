// Course: Programming paradigms (C#)
// Lab 2. Value-Types.
// Student: Trofimov V.A. Group: 2511
// Teacher: Povyshev V.V.
// Created 10.09.2013 Modified: 10.09.2013
// Description: Program entry point.

using System;

namespace _2
{
    struct Segment
    {
        public double StartX, StartY, EndX, EndY;
        public Segment(double xStart, double yStart, double xEnd, double yEnd)
        {
            StartX = xStart;
            StartY = yStart;
            EndX = xEnd;
            EndY = yEnd;
        }
        public double Length()
        {
            double lengthX = EndX - StartX;
            double lengthY = EndY - StartY;
            return Math.Sqrt(lengthX * lengthX + lengthY * lengthY);
        }
    }
    enum Style
    {
        Classic,
        Pop,
        Rock,
        Rap,
        NewAge,
        Electronic
    }
    class Program
    {
        static int Func(int x) 
        {
            return 3 - x + 2 * x * x * x;
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("{0}", Func(10));
            for (uint i = 0; i < 6; i++)
                Console.Write("{0} ", (Style)i);
            Console.WriteLine();
            Segment any = new Segment(0.0, 0.0, 3.0, 5.0);
            any.EndY = 4.0;
            Console.WriteLine("Length of segment with coordinates ({0}, {1}), ({2}, {3}) is {4}", 
                any.StartX, any.StartY, any.EndX, any.EndY, any.Length());
        }
    }
}
