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

            DynamixelControl.Terminate();

            DynamixelControl.Initialize();

            DynamixelControl.SetMovingSpeed(2, 123);

            bool a = true;

            while (a)
            {
                Console.WriteLine(DynamixelControl.GetPresentLoad(2));
            }

            DynamixelControl.Terminate();

            Console.ReadLine();

            return 1;

        }


    }
}
