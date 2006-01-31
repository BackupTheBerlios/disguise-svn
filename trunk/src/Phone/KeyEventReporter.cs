// created on 30.12.2005 at 10:33
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Collections;
using System.Text.RegularExpressions;
using DisGUISE.Backend;
using DisGUISE.Backend.Events;
using DisGUISE.Phone.Events;

namespace DisGUISE.Phone
{
    // This was buggy, the line got flooded ith the same event over and over again (also with minicom)
    // No idea what happened
    public class KeyEventReporter:EventReporter
    {
        private static Regex reKEY = new Regex("^\\+CKEV: (.*),(1|0)$");

        public event KeyEventHandler OnKeyPress;

        public KeyEventReporter(IPhonePort port):base(port, true)
        {
        }

        protected override ATCommand ConstructATCommand()
        {
            return new ATCommand("AT+CMER=3,2");
        }

        protected override void ProcessRawEvent(Object sender, RawEventArgs e)
        {
            Match m = reKEY.Match(e.Line);
            if (m.Success) {
                String key = m.Groups[1].Value;
                bool pressed = (int.Parse(m.Groups[2].Value) == 1);
                if (OnKeyPress != null) {
                    OnKeyPress(this, new KeyEventArgs(key, pressed));
                }
            }
        }
    }

    namespace Events
    {
        public class KeyEventArgs:EventArgs
        {
            private String _key;
            private bool _pressed;
            public String Key
            {
                get
                {
                    return _key;
                }
            }
            public bool Pressed
            {
                get
                {
                    return _pressed;
                }
            }
            public KeyEventArgs(String key, bool pressed):base()
            {
                this._key = key;
                this._pressed = pressed;
            }
        }

        public delegate void KeyEventHandler(Object sender, KeyEventArgs e);
    }
}
