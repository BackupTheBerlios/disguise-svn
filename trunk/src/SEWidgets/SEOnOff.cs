// created on 30.12.2005 at 10:19
// by Stefan Tomanek <stefan@pico.ruhr.de>


using System;
using DisGUISE.SEWidgets.Events;
using DisGUISE.Backend;

namespace DisGUISE.SEWidgets
{
    public class SEOnOff:SEWidget
    {
        public enum SEOnOffSelection
        {
            Off = 0, On = 1
        }

        private String title;
        private SEOnOffSelection status;

        public event BooleanAcceptHandler OnAcceptBoolean;

        public SEOnOff(IPhonePort port, String title, SEOnOffSelection status):base(port, "SEONO")
        {
            this.title = title;
            this.status = status;

            this.OnWidgetEvent += new WidgetEventHandler(ProcessWidgetEvent);
        }

        protected override ATCommand ConstructATCommand()
        {
            return new ATCommand("AT*" + this.cmdbase + "=\"" + title + "\"," + (int) status + ",1");
        }

        protected void ProcessWidgetEvent(Object sender, WidgetEventArgs e)
        {
            if (e.Type == SEWidgetEventType.AcceptBoolean) {
                Console.WriteLine(e.Payload);
                if (OnAcceptBoolean != null)
                    OnAcceptBoolean(this, new BooleanAcceptArgs(this, (e.Payload.Equals("1") ? true : false)));
            }
        }

    }
}
