// Course: Programming paradigms (C#)
// Lab 1. Basics.
// Student: Trofimov V.A. Group: 2511
// Teacher: Povyshev V.V.
// Created 10.09.2013 Modified: 10.09.2013
// Description: Program entry point.

using System;

namespace _1 {
    class Program 
    {
        static void Main(string[] args) 
        {
            Console.Out.WriteLine("Please, enter your name:"); String studentName;
            Console.WriteLine("Hello, " + (studentName = Console.In.ReadLine()));
            Console.Title = studentName;
            Console.SetWindowSize(50, 25);
            Console.BufferWidth = 50;
            Console.BufferHeight = 25; 
            Console.Beep();
        }
    }
}
