using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DetectiveSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new WitnessListController(args[0]);
            Console.ReadKey();
            controller.StartInvestigation();
        }
    }
}
