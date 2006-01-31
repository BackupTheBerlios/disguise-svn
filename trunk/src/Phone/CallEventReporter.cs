// created on 30.12.2005 at 10:36
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Collections;
using System.Text.RegularExpressions;
using DisGUISE.Backend;
using DisGUISE.Backend.Events;
using DisGUISE.Phone.Events;

namespace DisGUISE.Phone
{
    public class CallEventReporter:EventReporter
    {
        private Hashtable calls;
        // this has to be fine tuned
        private static Regex reCALL = new Regex("^\\*ECAV: ([0-9]+),([0-9]+),([0-9]+)(?:,([0-9]*))?(?:,([0-9]*))?(?:,\"([0-9]*)\",(145|129))?$");

        public CallEventHandler OnPhoneCall;

        public CallEventReporter(IPhonePort port):base(port, true)
        {
            calls = new Hashtable();
        }

        protected override ATCommand ConstructATCommand()
        {
            return new ATCommand("AT*ECAM=1");
        }

        private Call GetCall(int id)
        {
            if (calls.ContainsKey(id)) {
                return (Call) calls[id];
            }
            // Create a new call object
            Call call = new Call(id);
            calls.Add(id, call);
            return call;
        }

        protected override void ProcessRawEvent(Object sender, RawEventArgs e)
        {
            Match m = reCALL.Match(e.Line);
            if (m.Success) {
                int callID = int.Parse(m.Groups[1].Value);
                CallStatus callStatus = (CallStatus) int.Parse(m.Groups[2].Value);
                CallType callType = (CallType) int.Parse(m.Groups[3].Value);

                Call call = GetCall(callID);
                call.Status = callStatus;
                call.Type = callType;

                String number = m.Groups[6].Value;

                // We should add more properties here, like exit code etc.
                if (number.Length > 0) {
                    // If number type is 145, we have to prepend a + to the number
                    if (m.Groups[7].Value.Equals("145")) {
                        call.Number = "+" + number;
                    } else {
                        call.Number = number;
                    }
                }

                if (OnPhoneCall != null) {
                    OnPhoneCall(this, new CallEventArgs(call));
                }
            }
        }
    }
    namespace Events
    {
        public class CallEventArgs:EventArgs
        {
            private Call _callinfo;

            public Call Callinfo
            {
                get
                {
                    return _callinfo;
                }
            }

            public CallEventArgs(Call callinfo):base()
            {
                this._callinfo = callinfo;
            }
        }

        public delegate void CallEventHandler(Object sender, CallEventArgs e);
    }

}
