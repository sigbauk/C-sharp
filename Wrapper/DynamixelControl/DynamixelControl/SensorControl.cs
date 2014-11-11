using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamixelControl
{
    /// <summary>
    /// The methods in this class are used for communication between the computer and the Dynamixel sensor(s).
    /// 
    /// Supplementary information can be found online at: http://support.robotis.com/en/product/auxdevice/sensor/dxl_ax_s1.htm
    /// 
    /// </summary>
    public class SensorControl
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
        /// Left IR sensor value for distance measure. 
        /// Infrared rays are emitted from the IR-emitting part. The sensors measure the amount of reflected rays. 
        /// A higher value means that more rays are reflected, e.g. objects are located closer. 
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>A value between 0-255</returns>
        public static int GetIRLeftFireData(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ir left fire data"]);
        }

        /// <summary>
        /// Center IR sensor value for distance measure. 
        /// Infrared rays are emitted from the IR-emitting part. The sensors measure the amount of reflected rays. 
        /// A higher value means that more rays are reflected, e.g. objects are located closer. 
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>A value between 0-255</returns>
        public static int GetIRCenterFireData(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ir center fire data"]);
        }

        /// <summary>
        /// Right IR sensor value for distance measure. 
        /// Infrared rays are emitted from the IR-emitting part. The sensors measure the amount of reflected rays. 
        /// A higher value means that more rays are reflected, e.g. objects are located closer. 
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>A value between 0-255</returns>
        public static int GetIRRightFireData(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ir right fire data"]);
        }

        /// <summary>
        /// Left Light Brightness Sensor
        /// Measures the amount of infrared rays. 
        /// Similar to the distance measurement, but without any IR-self-emittion. 
        /// A higher value means more brightness
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>A value between 0-255</returns>
        public static int GetLightLeftData(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["light left data"]);
        }

        /// <summary>
        /// Center Light Brightness Sensor
        /// Measures the amount of infrared rays. 
        /// Similar to the distance measurement, but without any IR-self-emittion. 
        /// A higher value means more brightness
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>A value between 0-255</returns>
        public static int GetLightCenterData(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["light center data"]);
        }

        /// <summary>
        /// Right Light Brightness Sensor
        /// Measures the amount of infrared rays. 
        /// Similar to the distance measurement, but without any IR-self-emittion. 
        /// A higher value means more brightness
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>A value between 0-255</returns>
        public static int GetLightRightData(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["light right data"]);
        }

        /// <summary>
        /// Returns whether an object is detected within the defined range or not.
        /// If IR Distance Sensor value (IR Fire Data) is greater than the compare value, 1 is returned.
        /// The detection compare value can be changed by using the function SetIRObstacleDetectCompareRD
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>0: no object detected within range, 1: object detected</returns>
        public static int GetIRObstacleDetected(int id) 
        {
            return ReadFromDxl(id, controlTableDictionary["ir obstacle detected"]);
        }


        /// <summary>
        /// Returns whether the brightness sensor value (Light data) is greater than the compare value, or not.
        /// The detection compare value can be changed by using the function SetIRObstacleDetectCompareRD
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>0: darker than reference value, 1: brighter than reference value</returns>
        public static int GetLightDetected(int id) 
        {
            return ReadFromDxl(id, controlTableDictionary["light detected"]);
        }

        /// <summary>
        /// Measures the level of sound being recorded by the microphone. 
        /// The sound level is measured 3800 times/sec
        /// Returns a numerical value: If no sound, 127-128 is returned, and the 
        /// value approaces 0 or 255 as it gets _louder_. 
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>No sound: 127-128. Louder sounds: values close to 0 or 255</returns>
        public static int GetSoundData(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["sound data"]);
        }


        /// <summary>
        /// The maximum sound level
        /// If the sound level during sound measurement exceeds the current maximum,
        /// the value is updated (SetSoundDataMaxHold). 
        /// To make sure that the value will be updated, reset the value before measurement (set value to 0)!
        /// Sound levels beneath 128 are ignored. 255 is maximum. 
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>Maximum sound level: </returns>
        public static int GetSoundDataMaxHold(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["sound data max hold"]);
        }

        /// <summary>
        /// Sets the maximum sound level
        /// This function can be used to reset the maximum sound level value, 
        /// to ensure that the value is up to date after a measurement, by setting the value to 0.
        /// (The maximum sound level is only updated if it exceeds the current value. Therefore,
        /// if the current value is louder than the loudest sound level during a measurement, the 
        /// value will not be updated)
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <param name="value">Maximum sound level value. To reset, send 0</param>
        public static void SetSoundDataMaxHold(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["sound data max hold"], value);
        }

        /// <summary>
        /// Returns the number of times a certain sound level is measured
        /// (See online manual for supplementary information)
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns></returns>
        public static int GetSoundDetectedCount(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["sound detected count"]);
        }

        /// <summary>
        /// See online manual for information
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <param name="value"></param>
        public static void SetSoundDetected(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["sound detected count"], value);
        }

        /// <summary>
        /// Saves the time of the moment a sound detection occurd.
        /// See online manual for supplementary information
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns></returns>
        public static int GetSoundDetectedTime(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["sound detected time"]);
        }

        /// <summary>
        /// See online manual for information
        /// </summary>
        /// <param name="id"><Dynamixel sensor ID/param>
        /// <param name="value"></param>
        public static void SetSoundDetectedTime(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["sound detected time"], value);
        }


        /// <summary>
        /// Returns the current set buzzer note (noteAddress) on the Dynamixel sensor
        /// Note table:
        /// http://support.robotis.com/en/product/auxdevice/sensor/dxl_ax_s1.htm#Ax_S1_Address_28
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>The current set buzzer note</returns>
        public static int GetBuzzerData0(int id)
        {
            return ReadWordFromDxl(id, controlTableDictionary["buzzer data 0"]);
        }
        
       /// <summary>
       /// Plays the desired buzzer note (noteAddress) on the Dynamixel sensor
       /// Note table: 
       /// http://support.robotis.com/en/product/auxdevice/sensor/dxl_ax_s1.htm#Ax_S1_Address_28
       /// </summary>
       /// <param name="id">Dynamixel sensor ID</param>
       /// <param name="noteAddress">Buzzer note (see buzzer note table online)</param>
        public static void SetBuzzerData0(int id, int noteAddress)
        {
            WriteToDxl(id, controlTableDictionary["buzzer data 0"], noteAddress);
        }


        /// <summary>
        /// Returns the current set buzzer ringing time. 
        /// A returned value of 50 --> 5 seconds.
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>Unit: 0.1 second</returns>
        public static int GetBuzzerData1(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["buzzer data 1"]);
        }

        /// <summary>
        /// Set buzzer ringing time
        /// A value of 50 --> 5 seconds.
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <param name="value">Ringing time. Unit: 0.1 second</param>
        public static void SetBuzzerData1(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["buzzer data 1"], value);
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
        /// See online manual for instructions
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <param name="value"></param>
        public static void SetRegistered(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["registered"], value);
        }

        /// <summary>
        /// IR remote controller communication status.
        /// AX-S1 can communicate through its infrared emitters and sensors.
        /// If data is received, the value is updated to '2', e.g. 2 bytes are received.
        /// If these data are read, the value is set back to '0'.
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>2: new, unread data. 0: no new data</returns>
        public static int GetIRRemoconArrived(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ir remocon arrived"]);
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
        /// Returns received Remocon sensor data (IR remote control data).
        /// If this data is read, remocon arrived data will be set to 0 (marked as read).
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>Received Remocon data</returns>
        public static int GetRemoconRXData(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["remocon rx data 0"]);
        }

        /// <summary>
        /// Returns Remocon data (IR remote control data) to be transmitted 
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>Remocon data to be transmitted</returns>
        public static int GetRemoconTXData(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["remocon tx data 0"]);
        }

        /// <summary>
        /// Set the Remocon data (IR remote control data) to be transmitted
        /// 2 bytes of data can be transmitted
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <param name="value">Value to transmit via IR</param>
        public static void SetRemoconTXData(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["remocon tx data 0"], value);
        }

        /// <summary>
        /// Returns the current IR obstacle detection compare value
        /// This value is used in the IRObstacleDetected method.
        /// If '0': low sensitive mode; used for close range.
        /// See online manual for supplementary information:
        /// http://support.robotis.com/en/product/auxdevice/sensor/dxl_ax_s1.htm#Ax_S1_Address_34
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>The current IR detection compare value</returns>
        public static int GetIRObstacleDetectCompareRD(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["ir obstacle detect comparerd"]);
        }


        /// <summary>
        /// Sets the current IR obstacle detection compare value
        /// This value is used in the IRObstacleDetected method.
        /// See online manual for supplementary information:
        /// http://support.robotis.com/en/product/auxdevice/sensor/dxl_ax_s1.htm#Ax_S1_Address_34
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void SetIRObstacleDetectCompareRD(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["ir obstacle detect compared"], value);
        }

        /// <summary>
        /// Returns the current Light detect compare value. 
        /// This value is used in the GetLightDetected method.
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>Light detect compare value</returns>
        public static int GetLightDetectCompareRD(int id)
        {
            return ReadFromDxl(id, controlTableDictionary["light detect comparerd"]);
        }

        /// <summary>
        /// Sets the current Light detect compare value. 
        /// This value is used in the GetLightDetected method.
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <param name="value">New light detect compare value</param>
        public static void SetLightDetectCompareRD(int id, int value)
        {
            WriteToDxl(id, controlTableDictionary["light detect compared"], value);
        }

        

        /*
         * ADDITIONAL METHODS for improved usability: 
         */


        /// <summary>
        /// Returns the current set buzzer note on the Dynamixel sensor. 
        /// (This method runs GetBuzzerData0)
        /// See online manual for note table:
        /// http://support.robotis.com/en/product/auxdevice/sensor/dxl_ax_s1.htm#Ax_S1_Address_28
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>The current buzzer note</returns>
        public static int GetCurrentBuzzerNote(int id)
        {
            return GetBuzzerData0(id);
        }

        /// <summary>
        /// Play buzzer notes. Simple beep sounds can be made. 52 musical notes can be made 
        /// in buzzer tones, and there are also whole- and halftones in each octave.
        /// (This method runs SetBuzzerData0).
        /// See online manual for note table: 
        /// (http://support.robotis.com/en/product/auxdevice/sensor/dxl_ax_s1.htm#Ax_S1_Address_28)
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <param name="noteAddress">Buzzer note to play</param>
        public static void PlayBuzzerNote(int id, int noteAddress)
        {
            SetBuzzerData0(id, noteAddress);
        }


        /// <summary>
        /// Returns the current set buzzer ringing time. 
        /// A returned value of 50 --> 5 seconds.
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <returns>Unit: 0.1 second</returns>
        public static int GetBuzzerRingingTime(int id)
        {
            return GetBuzzerData1(id);
        }

        /// <summary>
        /// Set buzzer ringing time
        /// A value of 50 --> 5 seconds.
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        /// <param name="value">Ringing time. Unit: 0.1 second</param>
        public static void SetBuzzerRingingTime(int id, int value)
        {
            SetBuzzerData1(id, value);
        }

        /// <summary>
        /// Resets the Sound Data Max Hold, so that it is prepared for a new measurement.
        /// </summary>
        /// <param name="id">Dynamixel sensor ID</param>
        public static void ResetSoundDataMaxHold(int id)
        {
            SetSoundDataMaxHold(id, 0);
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

       






        // Help functions and variables:
        private static int[] singleByteAddresses = new int[] { 2, 3, 4, 5, 16, 26, 27, 28, 29, 30, 31, 32, 33, 35, 36, 37, 40, 41, 44, 46, 47, 52, 53 };
        private static bool IsSingleByteAddress(int address)
        {
            return singleByteAddresses.Contains(address);
        }

        /// <summary>
        /// Dictionary containing the control table. Key is parameter name, value is address. 
        /// </summary>
        private static Dictionary<string, int> controlTableDictionary = new Dictionary<string, int>();
        static SensorControl() // dictionary entries:
        {
            controlTableDictionary.Add("model number(l)", 0);
            controlTableDictionary.Add("model number(h)", 1);
            controlTableDictionary.Add("version of firmware", 2);
            controlTableDictionary.Add("id", 3);
            controlTableDictionary.Add("baud rate", 4);
            controlTableDictionary.Add("return delay time", 5);
            controlTableDictionary.Add("status return level", 16);
            controlTableDictionary.Add("ir left fire data", 26);
            controlTableDictionary.Add("ir center fire data", 27);
            controlTableDictionary.Add("ir right fire data", 28);
            controlTableDictionary.Add("light left data", 29);
            controlTableDictionary.Add("light center data", 30);
            controlTableDictionary.Add("light right data", 31);
            controlTableDictionary.Add("ir obstacle detected", 32);
            controlTableDictionary.Add("light detected", 33);
            controlTableDictionary.Add("sound data", 35);
            controlTableDictionary.Add("sound data max hold", 36);
            controlTableDictionary.Add("sound detected count", 37);
            controlTableDictionary.Add("sound detected time(l)", 38);
            controlTableDictionary.Add("sound detected time(h)", 39);
            controlTableDictionary.Add("buzzer data 0", 40);
            controlTableDictionary.Add("buzzer data 1", 41);
            controlTableDictionary.Add("registered", 44);
            controlTableDictionary.Add("ir remocon arrived", 46);
            controlTableDictionary.Add("lock", 47);
            controlTableDictionary.Add("remocon rx data 0", 48);
            controlTableDictionary.Add("remocon rx data 1", 49);
            controlTableDictionary.Add("remocon tx data 0", 50);
            controlTableDictionary.Add("remocon tx data 1", 51);
            controlTableDictionary.Add("ir obstacle detect comparerd", 52);
            controlTableDictionary.Add("light detect comparerd", 53);
        }






    }
}
