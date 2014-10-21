using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamixelControl
{
    /// <summary>
    /// The methods in this class are used for communication between the computer and the Dynamixel actuator(s).
    /// </summary>
    public class Communication
    {

        private const int DEFAULT_PORTNUM = 3; //com3
        private const int DEFAULT_BAUDNUM = 1; //mbps	


        // Help functions and variables:
        private static int[] singleByteAddresses = new int[] { 2, 3, 4, 5, 11, 12, 13, 16, 17, 18, 24, 25, 26, 27, 28, 29, 42, 43, 44, 46, 47 };
        private static bool IsSingleByteAddress(int address)
        {
            return singleByteAddresses.Contains(address);
        }

        /// <summary>
        /// Dictionary containing the control table. Key is parameter name, value is address. 
        /// </summary>
        public static Dictionary<string, int> controlTableDictionary = new Dictionary<string, int>();
        static Communication() // dictionary entries:
        {
            controlTableDictionary.Add("model number(l)", 0);
            controlTableDictionary.Add("model number(h)", 1);
            controlTableDictionary.Add("version of firmware", 2);
            controlTableDictionary.Add("id", 3);
            controlTableDictionary.Add("baud rate", 4);
            controlTableDictionary.Add("return delay time", 5);
            controlTableDictionary.Add("cw angle limit(l)", 6);
            controlTableDictionary.Add("cw angle limit(h)", 7);
            controlTableDictionary.Add("ccw angle limit(l)", 8);
            controlTableDictionary.Add("ccw angle limit(h)", 9);
            controlTableDictionary.Add("the highest limit temperature", 11);
            controlTableDictionary.Add("the lowest limit voltage", 12);
            controlTableDictionary.Add("the highest limit voltage", 13);
            controlTableDictionary.Add("max torque(l)", 14);
            controlTableDictionary.Add("max torque(h)", 15);
            controlTableDictionary.Add("status return level", 16);
            controlTableDictionary.Add("alarm led", 17);
            controlTableDictionary.Add("alarm shutdown", 18);
            controlTableDictionary.Add("torque enable", 24);
            controlTableDictionary.Add("led", 25);
            controlTableDictionary.Add("cw compliance margin", 26);
            controlTableDictionary.Add("ccw compliance margin", 27);
            controlTableDictionary.Add("cw compliance slope", 28);
            controlTableDictionary.Add("ccw compliance slope", 29);
            controlTableDictionary.Add("goal position(l)", 30);
            controlTableDictionary.Add("goal position(h)", 31);
            controlTableDictionary.Add("moving speed(l)", 32);
            controlTableDictionary.Add("moving speed(h)", 33);
            controlTableDictionary.Add("torque limit(l)", 34);
            controlTableDictionary.Add("torque limit(h)", 35);
            controlTableDictionary.Add("present position(l)", 36);
            controlTableDictionary.Add("present position(h)", 37);
            controlTableDictionary.Add("present speed(l)", 38);
            controlTableDictionary.Add("present speed(h)", 39);
            controlTableDictionary.Add("present load(l)", 40);
            controlTableDictionary.Add("present load(h)", 41);
            controlTableDictionary.Add("present voltage", 42);
            controlTableDictionary.Add("present temperature", 43);
            controlTableDictionary.Add("registered", 44);
            controlTableDictionary.Add("moving)", 46);
            controlTableDictionary.Add("lock", 47);
            controlTableDictionary.Add("punch(l)", 48);
            controlTableDictionary.Add("punch(h)", 49);
        }


        /// <summary>
        //  Attempts to initialize the communication devices
        /// </summary>
        /// <returns>1 if success, 0 if failure</returns>
        public static int Initialize()
        {
            return Wrapper.dxl_initialize(DEFAULT_PORTNUM, DEFAULT_BAUDNUM);
        }

        public static void Terminate()
        {
            Wrapper.dxl_terminate();
        }

        public static int GetModelNumber(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["model number(l)"]);
        }

        public static int GetVersionOfFirmware(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["version of firmware"]);
        }

        public static int GetID(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["id"]);
        }

        public static void SetID(int id, int newID)
        {
            WriteToDxl(id, controlTableDictionary["id"], newID);
        }

        public static int GetBaudrate(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["baud rate"]);
        }

        public static void SetBaudrate(int id, int newBaud)
        {
            WriteToDxl(id, controlTableDictionary["baud rate"], newBaud);
        }

        public static int GetReturnDelayTime(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["return delay time"]);
        }

        public static void SetReturnDelayTime(int id, int newReturnDelayTime)
        {
            WriteToDxl(id, controlTableDictionary["return delay time"], newReturnDelayTime);
        }

        public static int GetCWAngleLimit(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["cw angle limit(l)"]);
        }

        public static void SetCWAngleLimit(int id, int newCWAngleLimit)
        {
            WriteToDxl(id, controlTableDictionary["cw angle limit(l)"], newCWAngleLimit);
        }

        public static int GetCCWAngleLimit(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ccw angle limit(l)"]);
        }

        public static void SetCCWAngleLimit(int id, int newCCWAngleLimit)
        {
            WriteToDxl(id, controlTableDictionary["ccw angle limit(l)"], newCCWAngleLimit);
        }

        public static int GetTheHighestLimitTemperature(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["the highest limit temperature"]);
        }

        public static void SetTheHighestLimitTemperature(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["the highest limit temperature"], value);
        }

        public static int GetTheLowestLimitVoltage(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["the lowest limit voltage"]);
        }

        public static void SetTheLowestLimitVoltage(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["the lowest limit voltage"], value);
        }

        public static int GetTheHighestLimitVoltage(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["the highest limit voltage"]);
        }

        public static void SetTheHighestLimitVoltage(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["the highest limit voltage"], value);
        }

        public static int GetMaxTorque(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["max torque(l)"]);
        }

        public static void SetMaxTorque(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["max torque(l)"], value);
        }

        public static int GetStatusReturnLevel(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["status return level"]);
        }

        public static void SetStatusReturnLevel(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["status return level"], value);
        }

        public static int GetAlarmLED(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["alarm led"]);
        }

        public static void SetAlarmLED(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["alarm led"], value);
        }

        public static int GetAlarmShutdown(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["alarm shutdown"]);
        }

        public static void SetAlarmShutdown(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["alarm shutdown"], value);
        }

        public static int GetTorqueEnable(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["torque enable"]);
        }

        public static void SetTorqueEnable(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["torque enable"], value);
        }

        public static int GetLED(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["led"]);
        }

        public static void SetLED(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["led"], value);
        }

        public static int GetCWComplianceMargin(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["cw compliance margin"]);
        }

        public static void SetCWComplianceMargin(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["cw compliance margin"], value);
        }

        public static int GetCCWComplianceMargin(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ccw compliance margin"]);
        }

        public static void SetCCWComplianceMargin(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["ccw compliance margin"], value);
        }

        public static int GetCWComplianceSlope(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["cw compliance slope"]);
        }

        public static void SetCWComplianceSlope(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["cw compliance slope"], value);
        }

        public static int GetCCWComplianceSlope(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ccw compliance slope"]);
        }

        public static void SetCCWComplianceSlope(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["ccw compliance slope"], value);
        }

        public static int GetGoalPosition(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["goal position(l)"]);
        }

        public static void SetGoalPosition(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["goal position(l)"], value);
        }

        public static int GetMovingSpeed(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["moving speed(l)"]);
        }

        public static void SetMovingSpeed(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["moving speed(l)"], value);
        }

        public static int GetTorqueLimit(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["torque limit(l)"]);
        }

        public static void SetTorqueLimit(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["torque limit(l)"], value);
        }

        public static int GetPresentPosition(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present position(l)"]);
        }

        public static int GetPresentSpeed(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present speed(l)"]);
        }

        public static int GetPresentLoad(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present load(l)"]);
        }

        public static int GetPresentVoltage(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present voltage"]);
        }

        public static int GetPresentTemperature(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present temperature"]);
        }

        public static int GetRegistered(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["registered"]);
        }


        public static int GetMoving(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["moving"]);
        }

        public static int GetLock(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["lock"]);
        }


        public static void SetLock(int id, int value)
        {
            if (value != 0) value = 1; // make sure that the value will be either 0 or 1 
            WriteToDxl(id, controlTableDictionary["lock"], value);
        }

        public static int GetPunch(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["punch(l)"]);
        }

        public static void SetPunch(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["punch(l)"], value);
        }
	
	

        // Additional methods

        public void TorqueEnableSwitch(int id)
        {
            int status = GetTorqueEnable(id);
            if (status > 0) SetTorqueEnable(id, 0); // if on, turn off
            else SetTorqueEnable(id, 1); // if off, turn on
        }


        public void LedSwitch(int id)
        {
            int status = GetLED(id);
            if (status > 0) SetLED(id, 0); // if on, turn off
            else SetLED(id, 1);
        }

        
        public static bool IsInstructionRegistered(int id)
        {
            return (GetRegistered(id) > 0 ? true : false);
        }



        public static bool IsMoving(int id)
        {
            return (GetMoving(id) > 0 ? true : false);
        }

        public static bool IsEEPROMLocked(int id)
        {
            return (GetLock(id) > 0 ? true : false);
        }
	
	
	

        // Base functions: 

        public static int ReadFromDxl(int id, int address)
        {
            if (IsSingleByteAddress(address)) return ReadByteFromDxl(id, address);
            else return ReadWordFromDxl(id, address);
        }

        public static void WriteToDxl(int id, int address, int value)
        {
            if (IsSingleByteAddress(address)) WriteByteToDxl(id, address, value);
            else WriteWordToDxl(id, address, value);
        }

        private static void WriteByteToDxl(int id, int address, int value)
        {
            Wrapper.dxl_write_byte(id, address, value);
        }

        private static void WriteWordToDxl(int id, int address, int value)
        {
            Wrapper.dxl_write_word(id, address, value);
        }

        private static int ReadByteFromDxl(int id, int address)
        {
            return Wrapper.dxl_read_byte(id, address);
        }

        private static int ReadWordFromDxl(int id, int address)
        {
            return Wrapper.dxl_read_word(id, address);
        }


    }
}
