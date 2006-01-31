// created on 30.12.2005 at 10:15
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using DisGUISE.Phone;
using DisGUISE.SEWidgets.Events;
using DisGUISE.Backend;

namespace DisGUISE.SEWidgets
{
    public class SEGauge:SEWidget
    {
        public enum SEGaugeInteractiveMode
        {
            None = 0, Program, User
        }

        private String title;

        private SEGaugeInteractiveMode interactive;
        // According to the documentation, this cannot be greater than 30 in interactive mode
        // But in fact, 28 is the limit - Thank you SonyEricsson!
        private int max;
        private int val;

        public event IntegerAcceptHandler OnIntegerAccept;

        public SEGauge(IPhonePort port, String title, SEGaugeInteractiveMode interactive, int max, int val):base(port, "SEGAUGE")
        {
            this.title = title;
            this.interactive = interactive;
            this.max = max;
            this.val = val;

            this.OnWidgetEvent += new WidgetEventHandler(processWidgetEvent);
        }

        private void SetValue(int newval)
        {
            PhonePort.AddCommand(new ATCommand("AT*SEGUP=" + this.id + "," + newval));
            // We should intercept the reply, but this here is OK for the moment
            val = newval;
        }

        public int Value
        {
            get
            {
                return val;
            }
            set
            {
                SetValue(value);
            }
        }

        protected override ATCommand ConstructATCommand()
        {
            int form = 0;
            return new ATCommand("AT*" + this.cmdbase + "=\"" + title + "\"," + (int) interactive + ",1," + form + "," + val + "," + max);
        }

        protected void processWidgetEvent(Object sender, WidgetEventArgs e)
        {
            if (e.Type == SEWidgetEventType.AcceptInteger) {
                if (OnIntegerAccept != null)
                    OnIntegerAccept(this, new IntegerAcceptArgs(this, int.Parse(e.Payload)));
            }
        }

    }

    namespace Events
    {
        public class IntegerAcceptArgs:EventArgs
        {
            private int _data;
            public int Data
            {
                get
                {
                    return _data;
                }
            }
            private SEGauge _input;
            public SEGauge Input
            {
                get
                {
                    return _input;
                }
            }

            public IntegerAcceptArgs(SEGauge input, int data):base()
            {
                this._data = data;
                this._input = input;
            }
        }

        public delegate void IntegerAcceptHandler(Object sender, IntegerAcceptArgs e);
    }

}
