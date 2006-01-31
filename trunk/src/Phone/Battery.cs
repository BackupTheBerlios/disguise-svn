// created on 05.01.2006 at 17:31
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Text.RegularExpressions;
using DisGUISE.Backend;

namespace DisGUISE.Phone
{
    /* This is a simple class that queries the phones battery and power status
     */
    public class Battery:PhoneInteractor
    {
        // The regular expression used for extracting the necessary information
        private Regex reBat = new Regex("^\\+CBC: ([012]), ([0-9]+)$");

        public Battery(IPhonePort port):base(port)
        {
            // Nothing to do
        }

        public int ChargeLevel
        {
            get
            {
                return int.Parse(GetMatch().Groups[2].Value);
            }
        }

        public bool OnAC
        {
            get
            {
                return (int.Parse(GetMatch().Groups[1].Value) > 0);
            }
        }

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
