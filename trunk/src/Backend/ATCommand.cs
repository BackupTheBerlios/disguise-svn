// created on 30.12.2005 at 10:47
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Threading;

namespace DisGUISE.Backend
{
    /// <summary>
    /// This class represents an AT command that can be send to the phone through the IPhonePort interface.
    /// </summary>
    public class ATCommand
    {
        // This contains the real command
        private String cmd;
        // The data returned by the phone is stored here
        private String result;
        /*
         * Indicated whether the command has been already transmitted to the phone
         *
         * A command goes through 3 phases once it is added to the command queue
         * 1. Waiting for transmission
         * 2. When the command is transmitted to the phone, this flag is turned to a true value
         * 3. If all results are received (either with OK or ERROR), the command is removed from the queue
         */
        private bool transmitted;

        // The lock object, which is internally used to wait for the incoming data
        private Object locker = "";
        
        /// <summary>Instantiates an instance of the class with the supplied specific command string.</summary>
        /// <param name="cmd">The AT command that should be sent to the phone</param>
        public ATCommand(String cmd)
        {
            this.cmd = cmd;
            this.transmitted = false;
        }
        
        /// <value>Indicates whether the command has been transmitted to the phone.</value>
        /// <remarks>While reading this property is harmless, write access should be restricted to the IPhonePort object!</remarks>
        public bool Transmitted
        {
            get
            {
                return this.transmitted;
            }
            set
            {
                this.transmitted = value;
            }
        }
        
        /// <value>The command string the object was instantiated with</value>
        public String Command
        {
            get
            {
                return this.cmd;
            }
        }

        /// <summary>This method waits until the ready() method is triggered, which usually happens once all reply data is received</summary>
        /// <remarks>This method is also used by the IPhonePort object, using it outside of this class is not recommended.</remarks>
        public void WaitForResult()
        {
            lock(locker) {
                Monitor.Wait(locker);
            }
        }

        /// <summary>Once this method is triggered, the lock on waitForResult() is released, allowing the program to continue with the returned data</summary>
        /// <remarks>This method is also used by the IPhonePort object, using it outside of this class is not recommended.</remarks>
        public void Ready()
        {
            lock(locker) {
                Monitor.PulseAll(locker);
            }
        }

        /// <summary>This method is triggered everytime a new line of data belonging to this command is received.
        /// The line is then added to the internal buffer of the instance where it can be retrieved later.
        /// </summary>
        public void AddLine(String line)
        {
            result = result + line + "\n";
        }
        
        /// <summary>
        /// Retrieves the result string from the object. To ensure that the data is complete,
        /// this method should be called after Ready() released WaitForResult() released the lock.
        /// Usually, this is done in the IPhonePort object, so the use of this method outside of it
        /// should not be necessary.
        /// </summary>
        /// <value>The data received from the phone in response to the command.</value>
        public String Result
        {
            get
            {
                return this.result;
            }
        }
    }
}
