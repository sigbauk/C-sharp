using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// remove:
using DynamixelControl;

namespace DynamixelControl
{
    class TestProgram
    {
        public static void Main()
        {

            DynamixelControl.Initialize();

            //Console.WriteLine(DynamixelControl.GetModelNumber(2));


            Console.WriteLine(SensorControl.GetBuzzerData0(100));

           //DynamixelControl.ToggleWheelMode(4);

            /*
           Console.WriteLine("CW Angle Limit: " + DynamixelControl.GetCWAngleLimit(4));
           Console.WriteLine("CCW Angle Limit: " + DynamixelControl.GetCCWAngleLimit(4));
            */

            string a = Console.ReadLine();




            DynamixelControl.Terminate();

 
            




        }


    }
}
