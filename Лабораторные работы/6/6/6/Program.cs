// Course: Programming paradigms (C#)
// Lab 6. Arrays.
// Student: Trofimov V.A. Group: 2511
// Teacher: Povyshev V.V.
// Created 21.09.2013 Modified: 21.09.2013
// Description: Program entry point.

using System;
using System.Collections.Generic;
using System.Collections;

namespace _6
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 99, 77, 66, 22, 11, 88, 55, 44, 33 };
            Console.Write("Source array: ");
            foreach (int i in arr) { Console.Write(i + " "); }
            Console.Write("\nSorted array: "); Array.Sort(arr);
            foreach (int i in arr) { Console.Write(i + " "); }
            Console.WriteLine("\nPosition of 22 is " + Array.IndexOf(arr, 22));
            List<byte> list = new List<byte> { 99, 77, 66, 22, 11, 88, 55, 44, 33 };
            list.Remove(22);
            Console.Write("Array without item 22: ");
            foreach (int i in list) { Console.Write(i + " "); }
            Console.WriteLine("\n{ 2 5 } { 2 2 } X { 1 2 } { 1 0 } is ");
            int[,] a = { { 2, 5 }, { 2, 2 } };
            int[,] b = { { 1, 2 }, { 1, 0 } };
            for (int i = 0; i < 2; i++) { 
                for (int j = 0; j < 2; j++)  
                    Console.Write(a[i,0] * b[0,j] + a[i,1] * b[1,j] + " ");
                Console.WriteLine();
            }
            Hashtable h = new Hashtable();
            foreach (int i in new int[] { 99, 77, 66, 22, 11, 88, 55, 44, 33 })
                h.Add(i, "Элемент " + i.ToString());
            Console.WriteLine("Hashtable has " + (h.ContainsKey(22) ? "" : "not ") + "22 element");
        }
    }
}