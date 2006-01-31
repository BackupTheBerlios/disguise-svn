// created on 30.12.2005 at 10:22
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using DisGUISE.SEWidgets.Events;
using DisGUISE.Backend;

namespace DisGUISE.SEWidgets
{
    public class SEStringInput:SEWidget
    {
        public enum SEInputMode
        {
            Any = 0, Real, Integer, Phonenumber, URL, EMail
        }

        private String title;
        private String prompt;
        private String defaultText;
        private int predictive;
        private SEInputMode mode;

        public event StringAcceptHandler OnStringAccept;

        public SEStringInput(IPhonePort port, String title, String prompt, String defaultText, int predictive, SEInputMode mode):base(port, "SESTRI")
        {
            this.title = title;
            this.prompt = prompt;
            this.defaultText = defaultText;
            this.predictive = predictive;
            this.mode = mode;

            this.OnWidgetEvent += new WidgetEventHandler(this.ProcessWidgetEvent);
        }

        protected override ATCommand ConstructATCommand()
        {
            return new ATCommand("AT*" + this.cmdbase + "=\"" + title + "\",\"" + prompt + "\",\"" + defaultText + "\"," + predictive + "," + (int) mode + ",1");
        }

        protected void ProcessWidgetEvent(Object sender, WidgetEventArgs e)
        {
            if (e.Type == SEWidgetEventType.AcceptString) {
                // Remove " at beginning and end
                String load = e.Payload.Substring(1, e.Payload.Length - 2);
                if (OnStringAccept != null)
                    OnStringAccept(this, new StringAcceptArgs(this, load));
            }
        }

    }

    namespace Events
    {
        public class StringAcceptArgs:EventArgs
        {
            private String _data;
            public String Data
            {
                get
                {
                    return _data;
                }
            }
            private SEStringInput _input;
            public SEStringInput Input
            {
                get
                {
                    return _input;
                }
            }

            public StringAcceptArgs(SEStringInput input, String data):base()
            {
                this._data = data;
                this._input = input;
            }
        }
        public delegate void StringAcceptHandler(Object sender, StringAcceptArgs e);
    }
}
