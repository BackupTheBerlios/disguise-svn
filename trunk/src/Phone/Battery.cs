// created on 05.01.2006 at 17:31
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Text.RegularExpressions;
using DisGUISE.Backend;

namespace DisGUISE.Phone
{
    /// <summary>
    /// This class provides access to the battery and power status of the phone.
    /// </summary>
    public class Battery:PhoneInteractor
    {
        // The regular expression used for extracting the necessary information
        private Regex reBat = new Regex("^\\+CBC: ([012]), ([0-9]+)$");
        
        /// <summary>
        /// Creates an object that accesses the power situation of the phone.
        /// </summary>
        /// <param name="port">The port to communicate with.</param>
        public Battery(IPhonePort port):base(port)
        {
            // Nothing to do
        }
        
        /// <value>The battery level of the phone.</value>
        public int ChargeLevel
        {
            get
            {
                return int.Parse(GetMatch().Groups[2].Value);
            }
        }
        
        /// <value>Indicates whether the phone is connected to AC.</value>
        public bool OnAC
        {
            get
            {
                return (int.Parse(GetMatch().Groups[1].Value) > 0);
            }
        }
        /// <value>Indicates whether battery resides inside the phone.</value>
        public bool BatteryConnected
        {
            get
            {
                return (int.Parse(GetMatch().Groups[1].Value) < 2);
            }
        }

        private Match GetMatch()
        {
            // issue the command...
            return reBat.Match(PhonePort.AddCommand(new ATCommand("AT+CBC")));
        }

    }
}
