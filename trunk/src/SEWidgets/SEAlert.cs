// created on 30.12.2005 at 10:23
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using DisGUISE.Backend;

namespace DisGUISE.SEWidgets
{
    class SEAlert:SEWidget
    {
        private String text;
        public enum SEAlertType
        {
            None = 0, Alert, Confirmation, Error, Info, Warning, Feedback
        }
        private SEAlertType type;

        public SEAlert(IPhonePort port, String text, SEAlertType type):base(port, "SELERT")
        {
            this.text = text;
            this.type = type;
        }

        protected override ATCommand ConstructATCommand()
        {
            return new ATCommand("AT*" + this.cmdbase + "=\"" + text + "\"," + (int) type + ",1");
        }

    }
}
