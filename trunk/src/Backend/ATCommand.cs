// created on 30.12.2005 at 10:47
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Threading;

namespace DisGUISE.Backend
{
    /* This class represents an AT command that can be send to the phone through the IPhonePort interface.
     */
    public class ATCommand
    {
        // This contains the real command
        private String cmd;
        // The data returned by the phone is stored here
        private String result;
        private bool transmitted;

        // The lock object, which is internally used to wait for the incoming data
        private Object locker = "";

        public ATCommand(String cmd)
        {
            this.cmd = cmd;
            this.transmitted = false;
        }

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

        public String Command
        {
            get
            {
                return this.cmd;
            }
        }

        /* This method waits until the ready() method is triggered, which usually
           happens once all reply data is received
         */
        public void WaitForResult()
        {
            lock(locker) {
                Monitor.Wait(locker);
            }
        }

        /* Once this method is triggered, the lock on waitForResult() is released,
           allowing the program to continue with the returned data
         */
        public void Ready()
        {
            lock(locker) {
                Monitor.PulseAll(locker);
            }
        }

        /* This method is triggered everytime a new line of data
           belonging to this command is received
         */
        public void AddLine(String line)
        {
            result = result + line + "\n";
        }

        public String Result
        {
            get
            {
                return this.result;
            }
        }
    }
}
