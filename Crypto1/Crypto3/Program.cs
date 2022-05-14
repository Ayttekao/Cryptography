using System;
using Crypto3.Stuff;

namespace Crypto3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(GaloisField.Add(5, 2));
            Console.WriteLine(GaloisField.Multiply(5, 2));
            Console.WriteLine(GaloisField.Inverse(25, 283));
            Console.WriteLine(GaloisField.FindIrreduciblePolynomials());
        }
    }
}