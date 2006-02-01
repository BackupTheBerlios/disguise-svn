// created on 30.12.2005 at 10:48
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using DisGUISE.Backend.Events;

namespace DisGUISE.Backend
{
    /// <summary>The IPhonePort interface defines the methods for basic communication with the phone device.</summary>
    public interface IPhonePort
    {
        /// <summary>
        /// This method adds a new command to the command queue, and will return the answer strings
        /// of the phone once the command is executed
        /// </summary>
        String AddCommand(ATCommand cmd);
        /// <summary>Terminate to the connection to the phone and stop processing further events and commands</summary>
        void Stop();
        
        /// <event>This event gets triggered everytime an unsolicitated line is received</event> 
        event RawEventHandler OnRawEvent;
    }
}
