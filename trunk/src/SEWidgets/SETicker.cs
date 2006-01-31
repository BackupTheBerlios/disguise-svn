// created on 30.12.2005 at 10:17
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using DisGUISE.Backend;

namespace DisGUISE.SEWidgets
{
    class SETicker:SEWidget
    {
        private String text;

        public SETicker(IPhonePort port, String text):base(port, "SETICK")
        {
            this.text = text;
        }

        protected override ATCommand ConstructATCommand()
        {
            return new ATCommand("AT*" + this.cmdbase + "=\"" + text + "\",1");
        }
    }
}
