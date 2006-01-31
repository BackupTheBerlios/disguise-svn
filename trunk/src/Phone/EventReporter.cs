// created on 30.12.2005 at 10:38
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Text.RegularExpressions;
using DisGUISE.Backend;
using DisGUISE.Backend.Events;

namespace DisGUISE.Phone
{

    public abstract class EventReporter:PhoneInteractor
    {
        private bool needInstall;
        public EventReporter(IPhonePort port, bool needInstall):base(port)
        {
            this.needInstall = needInstall;
            port.OnRawEvent += new RawEventHandler(this.ProcessRawEvent);
        }

        protected abstract ATCommand ConstructATCommand();

        public void Install()
        {
            if (needInstall) {
                ATCommand cmd = ConstructATCommand();
                if (cmd != null)
                {
                    PhonePort.AddCommand(cmd);
                }
            }
        }
        protected abstract void ProcessRawEvent(Object sender, RawEventArgs e);
    }

}
