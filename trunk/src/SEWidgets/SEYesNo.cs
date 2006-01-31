// created on 30.12.2005 at 10:20
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using DisGUISE.Backend;

namespace DisGUISE.SEWidgets
{
    public class SEYesNo:SEWidget
    {
        private String title;
        private String question;

        public SEYesNo(IPhonePort port, String title, String question):base(port, "SEYNQ")
        {
            this.title = title;
            this.question = question;
        }

        protected override ATCommand ConstructATCommand()
        {
            return new ATCommand("AT*" + this.cmdbase + "=\"" + title + "\",\"" + question + "\",1");
        }
    }

    namespace Events
    {
        public class BooleanAcceptArgs:EventArgs
        {
            private bool _data;
            public bool Data
            {
                get
                {
                    return _data;
                }
            }
            private SEOnOff _input;
            public SEOnOff Input
            {
                get
                {
                    return _input;
                }
            }

            public BooleanAcceptArgs(SEOnOff input, bool data):base()
            {
                this._data = data;
                this._input = input;
            }
        }
        public delegate void BooleanAcceptHandler(Object sender, BooleanAcceptArgs e);
    }

}
