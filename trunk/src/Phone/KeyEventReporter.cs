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
    /// <summary>
    /// An object of this class is able to report key press events of the phone. Everytime an arbitrary
    /// key on the phone is pressed by the user, an event is triggered.
    /// </summary>
    public class KeyEventReporter:EventReporter
    {
        // This was buggy, the line got flooded ith the same event over and over again (also with minicom)
        // No idea what happened
        private static Regex reKEY = new Regex("^\\+CKEV: (.*),(1|0)$");
        
        /// <summary>
        /// This event is triggered everytime a phone key is pressed (or released). 
        /// </summary>
        public event KeyEventHandler OnKeyPress;
        
        public KeyEventReporter(IPhonePort port):base(port, true)
        {
            // Nothing to do
        }

        protected override ATCommand ConstructATCommand()
        {
            // CMER == Mobile Equipment Event Reporting
            //  3 == Unsolicitated result codes (aka events)
            //        are forwarded to the terminal (aka us) immediatly
            //        (0 to disable)
            //  2 == keypad events will be reported
            //        (0 to disable)
            return new ATCommand("AT+CMER=3,2");
        }

        protected override void ProcessRawEvent(Object sender, RawEventArgs e)
        {
            Match m = reKEY.Match(e.Line);
            if (m.Success) {
                String key = m.Groups[1].Value;
                // press or release?
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
            
            /// <value>The code of the activated key.</value>
            public String Key
            {
                get
                {
                    return _key;
                }
            }
            
            /// <value>Indicates whether the key was pressed (or released)</value>
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
