// created on 30.12.2005 at 10:48
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using DisGUISE.Backend.Events;

namespace DisGUISE.Backend
{
    /// The IPhonePort interface defines the methods for communication with the phone device
    public interface IPhonePort
    {
        /// This method adds a new command to the command queue, and will return the answer strings
        /// of the phone once the command is executed
        String AddCommand(ATCommand cmd);
        /// Terminate to the connection to the phone and stop processing further events and commands
        void Stop();

        event RawEventHandler OnRawEvent;
    }
}
