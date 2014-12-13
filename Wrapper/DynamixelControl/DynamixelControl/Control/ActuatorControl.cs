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
    public class ActuatorControl
    {

        private const int DEFAULT_PORTNUM = 3; //com3
        private const int DEFAULT_BAUDNUM = 1; //mbps	


        // Base functions: 

        /// <summary>
        //  Attempts to initialize the communication devices
        /// </summary>
        /// <returns>1 if success, 0 if failure</returns>
        public static int Initialize()
        {
            return Wrapper.dxl_initialize(DEFAULT_PORTNUM, DEFAULT_BAUDNUM);
        }

        /// <summary>
        /// Terminates the communication devices
        /// </summary>
        public static void Terminate()
        {
            Wrapper.dxl_terminate();
        }

        /// <summary>
        /// Reads a byte or word from the Dynamixel actuator
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="address">Memory address to read from (see Control Table)</param>
        /// <returns>Value at the memory address</returns>
        public static int ReadFromDxl(int id, int address)
        {
            if (IsSingleByteAddress(address)) return ReadByteFromDxl(id, address);
            else return ReadWordFromDxl(id, address);
        }

        /// <summary>
        /// Writes a byte or word to the Dynamixel actuator
        /// (checks whether the input is a byte or word before execution)
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="address">Memory address to write to (see Control Table)</param>
        /// <param name="value">Value to write</param>
        public static void WriteToDxl(int id, int address, int value)
        {
            if (IsSingleByteAddress(address)) WriteByteToDxl(id, address, value);
            else WriteWordToDxl(id, address, value);
        }

        /// <summary>
        /// Returns the model number of the Dynamixel
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Model number</returns>
        public static int GetModelNumber(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["model number(l)"]);
        }

        /// <summary>
        /// Returns firmware version
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Firmware version</returns>
        public static int GetVersionOfFirmware(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["version of firmware"]);
        }

        /// <summary>
        /// Returns the ID of the actuator
        /// 254 is the Broadcast ID
        /// </summary>
        /// <param name="id">Dynamixel actuator ID (to check)</param>
        /// <returns>Dynamixel actuator ID, range: 0-254</returns>
        public static int GetID(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["id"]);
        }

        /// <summary>
        /// Sets the ID parameter on the Dynamixel actuator
        /// 254 is the Broadcast ID
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="newID">New ID value, range: 0-254</param>
        public static void SetID(int id, int newID)
        {
            if (newID < 0) newID = 0;
            if (newID > 254) newID = 254;
            WriteToDxl(id, controlTableDictionary["id"], newID);
        }

        /// <summary>
        /// Returns the baudrate
        /// The Baud Rate represents the communication speed (0-254).
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Baudrate, range: 0-254</returns>
        public static int GetBaudrate(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["baud rate"]);
        }

        /// <summary>
        /// Sets the baudrate
        /// The Baud Rate represents the communication speed (0-254).
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="newBaud">New baudrate value, range: 0-254</param>
        public static void SetBaudrate(int id, int newBaud)
        {
            if (newBaud < 0) newBaud = 0;
            if (newBaud > 254) newBaud = 254;
            WriteToDxl(id, controlTableDictionary["baud rate"], newBaud);
        }

        /// <summary>
        /// Returns Return Delay Time
        /// Return Delay Time is the delay time from an Instruction Packet is 
        /// transmitted, until a Status Packet is received (0-254).
        /// Unit: 2 usec
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Return Delay Time, range: 0-254</returns>
        public static int GetReturnDelayTime(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["return delay time"]);
        }

        /// <summary>
        /// Set Return Delay Time
        /// Return Delay Time is the delay time from an Instruction Packet is 
        /// transmitted, until a Status Packet is received (0-254).
        /// Unit: 2 usec
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="newReturnDelayTime">New Return Delay Time value, range: 0-254</param>
        public static void SetReturnDelayTime(int id, int newReturnDelayTime)
        {
            if (newReturnDelayTime < 0) newReturnDelayTime = 0;
            if (newReturnDelayTime > 254) newReturnDelayTime = 254;
            WriteToDxl(id, controlTableDictionary["return delay time"], newReturnDelayTime);
        }

        /// <summary>
        /// Returns the CW Angle Limit
        /// If value is set to 0, Wheel Mode is chosen. Other values, Joint Mode (servo).
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>CW Angle Limit</returns>
        public static int GetCWAngleLimit(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["cw angle limit(l)"]);
        }

        /// <summary>
        /// Sets the CW Angle Limit
        /// If value is set to 0, Wheel Mode is chosen. Other values, Joint Mode (servo).
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="newCWAngleLimit">New CW Angle Limit value</param>
        public static void SetCWAngleLimit(int id, int newCWAngleLimit)
        {
            // Only checks if the input values are too low, values over 2047 may be used to enter Multi-turn Mode:
            if (newCWAngleLimit < 0) newCWAngleLimit = 0; 
            WriteToDxl(id, controlTableDictionary["cw angle limit(l)"], newCWAngleLimit);
        }

        /// <summary>
        /// Returns the CCW Angle Limit
        /// If value is set to 0, Wheel Mode is chosen. Other values, Joint Mode (servo).
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>CCW Angle Limit</returns>
        public static int GetCCWAngleLimit(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ccw angle limit(l)"]);
        }

        /// <summary>
        /// Sets the CCW Angle Limit
        /// If value is set to 0, Wheel Mode is chosen. Other values, Joint Mode.
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="newCCWAngleLimit">New CCW Angle Limit value</param>
        public static void SetCCWAngleLimit(int id, int newCCWAngleLimit)
        {
            // Only checks if the input values are too low, values over 2047 may be used to enter Multi-turn Mode:
            if (newCCWAngleLimit < 0) newCCWAngleLimit = 0;
            WriteToDxl(id, controlTableDictionary["ccw angle limit(l)"], newCCWAngleLimit);
        }

        /// <summary>
        /// Returns the Highest Limit Temperature
        /// NB! Should not be changed from its default value (70).
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Highest Limit Temperature</returns>
        public static int GetTheHighestLimitTemperature(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["the highest limit temperature"]);
        }

        /// <summary>
        /// Sets the Highest Limit Temperature
        /// NB! Should not be changed from its default value (70). 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New Highest Limit Temperature value</param>
        public static void SetTheHighestLimitTemperature(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["the highest limit temperature"], value);
        }

        /// <summary>
        /// Returns the Lowest Limit Voltage
        /// Lowest Limit Voltage is the lowest value in the voltage operation range.
        /// Valid values: 50 - 250. 
        /// Unit: 0.1V
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Lowest Limit Voltage</returns>
        public static int GetTheLowestLimitVoltage(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["the lowest limit voltage"]);
        }

        /// <summary>
        /// Sets the Lowest Limit Voltage
        /// Lowest Limit Voltage is the lowest value in the voltage operation range.
        /// Valid values: 50 - 250.
        /// Unit: 0.1V
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New Lowest Limit Voltage value, range: 50-250</param>
        public static void SetTheLowestLimitVoltage(int id, int value)
        {
            if (value < 50) value = 50;
            if (value > 250) value = 250;
            WriteToDxl(id, controlTableDictionary["the lowest limit voltage"], value);
        }

        /// <summary>
        /// Returns the Highest Limit Voltage
        /// Highest Limit Voltage is the highest value in the voltage operation range.
        /// Valid values: 50 - 250.
        /// Unit: 0.1V
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Highest Limit Voltage</returns>
        public static int GetTheHighestLimitVoltage(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["the highest limit voltage"]);
        }

        /// <summary>
        /// Sets the Highest Limit Voltage
        /// Highest Limit Voltage is the highest value in the voltage operation range.
        /// Valid values: 50 - 250.
        /// Unit: 0.1V
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New Highest Limit Voltage value, range: 50-250</param>
        public static void SetTheHighestLimitVoltage(int id, int value)
        {
            if (value < 50) value = 50;
            if (value > 250) value = 250;
            WriteToDxl(id, controlTableDictionary["the highest limit voltage"], value);
        }

        /// <summary>
        /// Returns Max Torque
        /// How much torque the actuator produces.
        /// Valid values: 0 - 1023 (0% - 100%).  
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Max Torque, range: 0-1023</returns>
        public static int GetMaxTorque(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["max torque(l)"]);
        }

        /// <summary>
        /// Sets the Max Torque
        /// How much torque the actuator produces.
        /// Valid values: 0 - 1023 (0% - 100%).  
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New Max Torque value, range: 0-1023</param>
        public static void SetMaxTorque(int id, int value)
        {
            if (value < 0) value = 0;
            if (value > 1023) value = 1023;
            WriteToDxl(id, controlTableDictionary["max torque(l)"], value);
        }

        /// <summary>
        /// Returns Status Return Level
        /// Decides how to return the Status Packet.
        /// Value 0: No return against all commands (except PING)
        /// Value 1: Return only for the READ command
        /// Value 2: Return for all commands
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Status Return Level, 0, 1 or 2</returns>
        public static int GetStatusReturnLevel(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["status return level"]);
        }

        /// <summary>
        /// Sets the Status Return Level
        /// Decides how to return the Status Packet.
        /// Value 0: No return against all commands (except PING)
        /// Value 1: Return only for the READ command
        /// Value 2: Return for all commands
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New Status Return Level value, 0, 1 or 2</param>
        public static void SetStatusReturnLevel(int id, int value)
        {
            if (value < 0 || value > 3) return; 
            else WriteToDxl(id, controlTableDictionary["status return level"], value);
        }

        /// <summary>
        /// Returns Alarm LED status
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>0 if off, 1 else</returns>
        public static int GetAlarmLED(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["alarm led"]);
        }

        /// <summary>
        /// Sets the Alarm LED
        /// Off: 0, on: 1
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New Alarm LED status value, 0 or 1</param>
        public static void SetAlarmLED(int id, int value)
        {
            if (value != 0 || value != 1) return;
            else WriteToDxl(id, controlTableDictionary["alarm led"], value);
        }

        /// <summary>
        /// Returns Alarm Shutdown status
        /// The Dynamixel can protect itself by detecting errors during operation.
        /// At shutdown, Torque limit is set to 0. 
        /// The settings depend on values in a byte; each bit decides whether 
        /// the error corresponding to that byte-position is to be detected or not (logic OR on each bit):
        /// 
        /// Bit 7: 0
        /// Bit 6: Instruction error
        /// Bit 5: Overload error
        /// Bit 4: CheckSum error
        /// Bit 3: Range error
        /// Bit 2: Overheating error
        /// Bit 1: Angle limit error
        /// Bit 0: Input voltage error
        /// 
        /// Example: 0X05 (00000101) will turn on both Input voltage error and Overheating error.
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>See description</returns>
        public static int GetAlarmShutdown(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["alarm shutdown"]);
        }

        /// <summary>
        /// Sets the Alarm Shutdown 
        /// The Dynamixel can protect itself by detecting errors during operation.
        /// At shutdown, Torque limit is set to 0. 
        /// The settings depend on values in a byte; each bit decides whether 
        /// the error corresponding to that byte-position is to be detected or not (logic OR on each bit):
        /// 
        /// Bit 7: 0
        /// Bit 6: Instruction error
        /// Bit 5: Overload error
        /// Bit 4: CheckSum error
        /// Bit 3: Range error
        /// Bit 2: Overheating error
        /// Bit 1: Angle limit error
        /// Bit 0: Input voltage error
        /// 
        /// Example: 0X05 (00000101) will turn on both Input voltage error and Overheating error.
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New Alarm Shutdown value (see description)</param>
        public static void SetAlarmShutdown(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["alarm shutdown"], value);
        }

        /// <summary>
        /// Returns Torque Enable
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Off: 0, on: 1</returns>
        public static int GetTorqueEnable(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["torque enable"]);
        }

        /// <summary>
        /// Sets the Torque Enable status
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">Off: 0, on: 1</param>
        public static void SetTorqueEnable(int id, int value)
        {
            if (value != 0 || value != 1) return;
            else WriteToDxl(id, controlTableDictionary["torque enable"], value);
        }

        /// <summary>
        /// Returns LED status
        /// Based on values in a byte (logic OR on each bit)
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Bit 2: BLUE LED, Bit 1: GREEN, Bit 0: RED LED</returns>
        public static int GetLED(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["led"]);
        }

        /// <summary>
        /// Sets the LED status
        /// Based on values in a byte (logic OR on each bit)
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">Bit 2: BLUE LED, Bit 1: GREEN, Bit 0: RED LED</param>
        public static void SetLED(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["led"], value);
        }

        /// <summary>
        /// Returns the CW Compliance Margin
        /// The margin designates the area around the goal position that receives no torque
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>CW Compliance Margin, range: 0-255</returns>
        public static int GetCWComplianceMargin(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["cw compliance margin"]);
        }

        /// <summary>
        /// Sets the CW Compliance Margin
        /// The margin designates the area around the goal position that receives no torque
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New CW Compliance Margin value, range: 0-255</param>
        public static void SetCWComplianceMargin(int id, int value)
        {
            if (value < 0) value = 0;
            if (value > 255) value = 255;
            WriteToDxl(id, controlTableDictionary["cw compliance margin"], value);
        }


        /// <summary>
        /// Returns the CCW Compliance Margin
        /// The margin designates the area around the goal position that receives no torque
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>CCW Compliance Margin, range: 0-255</returns>
        public static int GetCCWComplianceMargin(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ccw compliance margin"]);
        }

        /// <summary>
        /// Sets the CCW Compliance Margin
        /// The margin designates the area around the goal position that receives no torque
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New CCW Compliance Margin value, range: 0-255</param>
        public static void SetCCWComplianceMargin(int id, int value)
        {
            if (value < 0) value = 0;
            if (value > 255) value = 255;
            WriteToDxl(id, controlTableDictionary["ccw compliance margin"], value);
        }

        /// <summary>
        /// Returns the CW Compliance Slope
        /// Sets the level of torque near the goal position.
        /// There are seven levels; higher value means more flexibility:
        /// 
        /// 2, 4, 8, 16, 32, 64, 128
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>CW Compliance Slope (see description)</returns>
        public static int GetCWComplianceSlope(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["cw compliance slope"]);
        }


        /// <summary>
        /// Sets the CW Compliance Slope
        /// Sets the level of torque near the goal position.
        /// There are seven levels; higher value means more flexibility:
        /// 
        /// 2, 4, 8, 16, 32, 64, 128
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New CW Compliance Slope value (2, 4, 8, 16, 32, 64, 128) </param>
        public static void SetCWComplianceSlope(int id, int value)
        {
            if (value < 0) value = 0;
            if (value > 255) value = 254;
            WriteToDxl(id, controlTableDictionary["cw compliance slope"], value);
        }

        /// <summary>
        /// Returns the CCW Compliance Slope
        /// Sets the level of torque near the goal position.
        /// There are seven levels; higher value means more flexibility:
        /// 
        /// 2, 4, 8, 16, 32, 64, 128
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>CCW Compliance Slope (see description)</returns>
        public static int GetCCWComplianceSlope(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ccw compliance slope"]);
        }

        /// <summary>
        /// Sets the CCW Compliance Slope
        /// Sets the level of torque near the goal position.
        /// There are seven levels; higher value means more flexibility:
        /// 
        /// 2, 4, 8, 16, 32, 64, 128
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New CCW Compliance Slope value (2, 4, 8, 16, 32, 64, 128)</param>
        public static void SetCCWComplianceSlope(int id, int value)
        {
            if (value < 0) value = 0;
            if (value > 255) value = 254;
            WriteToDxl(id, controlTableDictionary["ccw compliance slope"], value);
        }


        /// <summary>
        /// Returns the Goal Position
        /// 
        /// 0-1023, the unit is 0.29 degrees.
        /// 
        /// If Goal Position is out of range, Alarm Limit Error will be triggered, and 
        /// Alarm LED/Alarm Shutdown will be executed.
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Goal Position, range: 0-1023</returns>
        public static int GetGoalPosition(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["goal position(l)"]);
        }

        /// <summary>
        /// Sets the Goal Position
        /// 
        /// 0-1023, the unit is 0.29 degrees.
        /// 
        /// If Goal Position is out of range, Alarm Limit Error will be triggered, and 
        /// Alarm LED/Alarm Shutdown will be executed.
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New Goal Position value, range: 0-1023</param>
        public static void SetGoalPosition(int id, int value)
        {
            if (value < 0) value = 0;
            if (value > 1023) value = 1023;
            WriteToDxl(id, controlTableDictionary["goal position(l)"], value);
        }

        /// <summary>
        /// Returns the Moving Speed
        /// 
        /// Range and unit of the value varies, depending on operation mode:
        /// 
        /// JOINT MODE - range: 0-1023, unit: 0.111rpm, example: value 300 --> 33.3rpm
        /// WHEEL MODE - range: 0-2047 (0-1023 CCW, 1024-2047 CW), unit: 0.1%
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Moving Speed (see description)</returns>
        public static int GetMovingSpeed(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["moving speed(l)"]);
        }

        /// <summary>
        /// Sets the Moving Speed
        /// 
        /// Range and unit of the value varies, depending on operation mode:
        /// 
        /// JOINT MODE - range: 0-1023, unit: 0.111rpm, example: value 300 --> 33.3rpm
        /// WHEEL MODE - range: 0-2047 (0-1023 CCW, 1024-2047 CW), unit: 0.1%
        /// 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">new Moving Speed value (see description)</param>
        public static void SetMovingSpeed(int id, int value)
        {
            if (value < 0) value = 0;
            else
            {
                if (GetMovementMode(id) == 0) // WHEEL MODE
                { 
                    if(value > 2047) value = 2047;
                }
                else // JOINT MODE
                {
                    if(value > 1023) value = 1023;
                }
            }

            WriteToDxl(id, controlTableDictionary["moving speed(l)"], value);
        }

        /// <summary>
        /// Returns the Torque Limit
        /// Range: 0-1023, unit: 0.1%
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Torque Limit</returns>
        public static int GetTorqueLimit(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["torque limit(l)"]);
        }

        /// <summary>
        /// Sets the Torque Limit
        /// Range: 0-1023, unit: 0.1%
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">New Torque Limit value, range: 0-1023</param>
        public static void SetTorqueLimit(int id, int value)
        {
            if (value < 0) value = 0;
            if (value > 1023) value = 1023;
            WriteToDxl(id, controlTableDictionary["torque limit(l)"], value);
        }

        /// <summary>
        /// Returns Present Position
        /// Range: 0-1023, unit: 0.29 degrees
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Present Position</returns>
        public static int GetPresentPosition(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present position(l)"]);
        }

        /// <summary>
        /// Returns the Present Speed
        /// Range: 0-2047 (0-1023 CCW, 1024-2047 CW)
        /// Units: JOINT MODE: 0.111rpm, WHEEL MODE: 0.1% 
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Present Speed (see description)</returns>
        public static int GetPresentSpeed(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present speed(l)"]);
        }

        /// <summary>
        /// Returns the Present Load
        /// Range: 0-2047 (0-1023 CCW, 1024-2047 CW)
        /// Unit: 0.1%
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Present Load</returns>
        public static int GetPresentLoad(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present load(l)"]);
        }

        /// <summary>
        /// Returns Present Voltage
        /// Unit: value * 10 = Voltage
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Present Voltage</returns>
        public static int GetPresentVoltage(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present voltage"]);
        }

        /// <summary>
        /// Returns Present Temperature
        /// Internal temperature of the Dynamixel
        /// Unit: Degrees Celsius
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Present Temperature</returns>
        public static int GetPresentTemperature(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["present temperature"]);
        }

        /// <summary>
        /// Returns whether Instruction is registered
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>False: 0, true: 1</returns>
        public static int GetRegistered(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["registered"]);
        }

        /// <summary>
        /// Returns whether there is any movement
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>False: 0, true: 1</returns>
        public static int GetMoving(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["moving"]);
        }

        /// <summary>
        /// Returns whether EEPROM is locked
        /// EEPROM is a memory area (addresses 0-18) that can be locked from modification
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>False: 0, true: 1</returns>
        public static int GetLock(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["lock"]);
        }

        /// <summary>
        /// Sets the EEPROM lock
        /// EEPROM is a memory area (addresses 0-18) that can be locked from modification
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="value">Lock: 1, unlock: 0</param>
        public static void SetLock(int id, int value)
        {
            if (value != 0 || value != 1) return;
            else WriteToDxl(id, controlTableDictionary["lock"], value);
        }

        /// <summary>
        /// Returns the Punch 
        /// Punch is the minimum voltage that will be applied to the motor when the position 
        /// is just outside the compliance margin. It is needed to overcome internal gear friction.
        /// Default value: 32
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Punch</returns>
        public static int GetPunch(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["punch(l)"]);
        }

        /// <summary>
        /// Sets the Punch
        /// Punch is the minimum voltage that will be applied to the motor when the position
        /// is just outside the compliance margin. It is needed to overcome internal gear friction.
        /// Default value: 32
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value">New Punch value, range: 32-1023</param>
        public static void SetPunch(int id, int value)
        {
            if (value < 32) value = 32;
            if (value > 1023) value = 1023;
            WriteToDxl(id, controlTableDictionary["punch(l)"], value);
        }








        // ADDITIONAL METHODS for improved usability:

        /// <summary>
        /// Toggles the Torque on or off
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        public void TorqueEnableSwitch(int id)
        {
            int status = GetTorqueEnable(id);
            if (status > 0) SetTorqueEnable(id, 0); // if on, turn off
            else SetTorqueEnable(id, 1); // if off, turn on
        }


        /// <summary>
        /// Returns whether an Instruction has been registered or not
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>true/false</returns>
        public static bool IsInstructionRegistered(int id)
        {
            return (GetRegistered(id) > 0 ? true : false);
        }


        /// <summary>
        /// Returns whether an Dynamixel actuator is moving or not
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>true/false</returns>
        public static bool IsMoving(int id)
        {
            return (GetMoving(id) > 0 ? true : false);
        }

        /// <summary>
        /// Returns whether EEPROM is locked or not
        /// EEPROM is a memory area (addresses 0-18) that can be locked
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true/false</returns>
        public static bool IsEEPROMLocked(int id)
        {
            return (GetLock(id) > 0 ? true : false);
        }


        /// <summary>
        /// Turns on WHEEL MODE on the Dynamixel actuator
        /// WHEEL MODE: The actuator rotates 360 degrees like a regular motor
        /// </summary>
        /// <param name="id"></param>
        public static void ToggleWheelMode(int id)
        {
            SetCWAngleLimit(id, 0);
            SetCCWAngleLimit(id, 0);
        }

        /// <summary>
        /// Turns on JOINT MODE on the Dynamixel actuator
        /// JOINT MODE: The actuator moves at a set angle range (CW angle limit, CCW angle limit)
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="newCWAngleLimit">New CW Angle Limit</param>
        /// <param name="newCCWAngleLimit">New CCW Angle Limit</param>
        public static void ToggleJointMode(int id, int newCWAngleLimit, int newCCWAngleLimit)
        {
            SetCWAngleLimit(id, newCWAngleLimit);
            SetCCWAngleLimit(id, newCCWAngleLimit);
        }

        /// <summary>
        /// Returns the goal position as an angular value
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Angular goal position value</returns>
        public static int GetGoalPositionAngular(int id)
        {
            return AngularValueFromDxlValue(GetGoalPosition(id));
        }

        /// <summary>
        /// Sets the goal position based on angular input
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <param name="angularPosition">Angular goal position value</param>
        public static void SetGoalPositionAngular(int id, int angularPosition)
        {
            SetGoalPosition(id, AngularValueToDxlValue(angularPosition));
        }

        /// <summary>
        /// Returns the present position as an angular value
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>Position as angular value</returns>
        public static int GetPresentPositionAngular(int id)
        {
            return AngularValueFromDxlValue(GetPresentPosition(id));
        }



        /// <summary>
        /// Returns Dynamixel actuator movement mode (WHEEL MODE or JOINT MODE (servo))
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        /// <returns>WHEEL MDOE: 0, JOINT MODE: 1</returns>
        public static int GetMovementMode(int id)
        {
            if (GetCWAngleLimit(id) == 0 && GetCCWAngleLimit(id) == 0) return 0; // WHEEL MODE
            else return 1; // JOINT MODE
        }








        /* 
         * The following subroutines are used internally: 
         */

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

        // Converts a Dxl position value into an angular value
        private static int AngularValueFromDxlValue(int value)
        {
            return (int)(value * 0.29); // 0.29 degrees/DxlPositionValue
        }

        // Converts an angular value into a Dxl position value
        private static int AngularValueToDxlValue(int value)
        {
            return (int)(value / 0.29); // 0.29 degrees/DxlPositionValue
        }


        // Help functions and variables:
        private static int[] singleByteAddresses = new int[] { 2, 3, 4, 5, 11, 12, 13, 16, 17, 18, 24, 25, 26, 27, 28, 29, 42, 43, 44, 46, 47 };
        private static bool IsSingleByteAddress(int address)
        {
            return singleByteAddresses.Contains(address);
        }

        /// <summary>
        /// Dictionary containing the control table. Key is parameter name, value is address. 
        /// </summary>
        private static Dictionary<string, int> controlTableDictionary = new Dictionary<string, int>();
        static ActuatorControl() // dictionary entries:
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




    }
}
