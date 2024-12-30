using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class A
    {
        public virtual void printf()
        {

        }
    }

    class B :A
    {
        public override void printf()
        {
            Console.WriteLine("class b");
        }
    }

    class C: A
    {
        public override void printf()
        {
            Console.WriteLine("class c");
        }
    }

    class D : B
    {
    }


}
