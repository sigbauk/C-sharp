using System.Runtime.InteropServices;

namespace DynamixelControl
{
    /// <summary>
    /// This class wraps the DynamixelControl methods so that they can be used with C#. 
    /// </summary>
    unsafe public class Wrapper
    {

        /*
         * DEVICE CONTROL METHODS - subroutines to control the communication devices 
         */

        /// <summary>
        /// Attempts to initialize the communication devices
        /// </summary>
        /// <param name="devIndex">Number of connected communication devices or default PORTNUM</param> 
        /// <param name="baudnum"></param>
        /// <returns>1 if success, 0 if failure</returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_initialize(int devIndex, int baudnum);
        /// <summary>
        /// Terminates the communication devices 
        /// </summary>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_terminate();

        /*
         *
         * PACKET COMMUNICATION METHODS - subroutines used to transmit and receive packets
         * 
         */

        /// <summary>
        /// Transmits instruction packet to Dynamixel
        /// </summary>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_tx_packet();

        /// <summary>
        /// Extracts status packet from the driver buffer
        /// </summary>
        /// <returns>status packet</returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_rx_packet();


        /// <summary>
        /// Runs dxl_tx_packet (transmits an instruction packet to the Dynamixel)
        /// If success, run dxl_rx_packet (status packet is extracted from driver buffer).
        /// </summary>
        /// <returns>status packet</returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_txrx_packet();


        /// <summary>
        /// Get the current communication status
        /// </summary>
        /// <returns>communication status identifier</returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_get_result();


        

        /*
         *
         * SET/GET PACKET METHODS - subroutines to make and see the packets
         * 
        */
        
        /// <summary>
        /// Sets the instruction packet ID
        /// </summary>
        /// <param name="id">Dynamixel ID to transmit instruction packet to</param>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_set_txpacket_id(int id);


        /// <summary>
        /// Sets the instruction of the instruction packet
        /// </summary>
        /// <param name="instruction">instruction, see Control table</param>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_set_txpacket_instruction(int instruction);


        /// <summary>
        /// Sets the parameter of the instruction packet
        /// </summary>
        /// <param name="index">parameter number to be set</param>
        /// <param name="value">instruction value. See control table</param>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_set_txpacket_parameter(int index, int value);


        /// <summary>
        /// Sets the instruction packet length
        /// </summary>
        /// <param name="length">Length of the instruction packet</param>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_set_txpacket_length(int length);


        /// <summary>
        /// Checks whether the status packet error equals the inputted error
        /// </summary>
        /// <param name="errbit">Bit flag to check whether errors occur or not</param>
        /// <returns>1 if true, 0 if false</returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_get_rxpacket_error(int errbit);


        /// <summary>
        /// Get length of status packet
        /// </summary>
        /// <returns>Length of status packet</returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_get_rxpacket_length();

        /// <summary>
        /// Gets the parameter value of the status packet
        /// </summary>
        /// <param name="index">is the parameter number to be set</param>
        /// <returns>The parameter value at the "index"th element of status packet</returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_get_rxpacket_parameter(int index);



        /*
         * HIGH COMMUNICATION METHODS - subroutines used to functionalize frequently used 
         * communication packets for user convenience
         */

        /// <summary>
        /// Ping a Dynamixel to evaluate its existence
        /// </summary>
        /// <param name="id">Dynamixel actuator ID</param>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_ping(int id);

        /// <summary>
        /// Read one byte from the Dynamixel actuator
        /// </summary>
        /// <param name="id">Dynamixel ID</param>
        /// <param name="address">The read data value</param>
        /// <returns></returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_read_byte(int id, int address);

        /// <summary>
        /// Write one byte to the Dynamixel actuator
        /// </summary>
        /// <param name="id">Dynamixel ID</param>
        /// <param name="address">Location of data to operate on (see Control Table)</param>
        /// <param name="value">Value to write on the Dynamixel at the desired address</param>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_write_byte(int id, int address, int value);


        /// <summary>
        /// Two bytes can be read from the Dynamixel actuator
        /// </summary>
        /// <param name="id">Dynamixel ID</param>
        /// <param name="address">Location of data to operate on (see Control Table)</param>
        /// <returns></returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_read_word(int id, int address);

        /// <summary>
        /// Two bytes can be written to the Dynamixel actuator
        /// </summary>
        /// <param name="id">Dynamixel ID</param>
        /// <param name="address">Location of data to operate on (see Control Table)</param>
        /// <param name="value">The value to write on the Dynamixel</param>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern void dxl_write_word(int id, int address, int value);

        /*
         * 
         * UTILITY METHODS - other useful subroutines
         * 
         */

        /// <summary>
        /// Combines a lowbyte and a highbyte and returns a word
        /// </summary>
        /// <param name="lowbyte">Lower byte to be made of WORD-type</param>
        /// <param name="highbyte">Higher byte to be made of WORD-type</param>
        /// <returns></returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_makeword(int lowbyte, int highbyte);

        /// <summary>
        /// Returns the lowbyte of a word
        /// </summary>
        /// <param name="word">Word-type data to extract lower byte from</param>
        /// <returns>The lower byte extracted from word</returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_get_lowbyte(int word);

        /// <summary>
        /// Returns the highbyte of a word
        /// </summary>
        /// <param name="word">Word-type data to extract higher byte from</param>
        /// <returns>The higher byte extracted from word</returns>
        [DllImport("DynamixelControl32_dll.dll")]
        public static extern int dxl_get_highbyte(int word);


    
    }
}
