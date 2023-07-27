using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            float test_f = 1.234f;
            object holy_shit = test_f;
            float unbox_one = (float)holy_shit;
            Console.WriteLine("Hello World!" + holy_shit.ToString());
            Console.ReadLine();
        }
    }
}
