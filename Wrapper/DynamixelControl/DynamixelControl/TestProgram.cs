using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamixelControl
{
    class TestProgram
    {
        public static int Main()
        {

            Communication.Terminate();

            Communication.Initialize();

            Communication.SetGoalPosition(2, 12);

            Communication.Terminate();

            Console.ReadLine();

            return 1;

        }


    }
}
