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

            ActuatorControl.Initialize();

            //Console.WriteLine(DynamixelControl.GetModelNumber(2));

            SensorControl.PlayBuzzerNote(100, 51);

           //DynamixelControl.ToggleWheelMode(4);

            /*
           Console.WriteLine("CW Angle Limit: " + DynamixelControl.GetCWAngleLimit(4));
           Console.WriteLine("CCW Angle Limit: " + DynamixelControl.GetCCWAngleLimit(4));
            */

            string a = Console.ReadLine();




            ActuatorControl.Terminate();

 
            




        }


    }
}
