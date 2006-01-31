// created on 30.12.2005 at 10:30
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;

namespace DisGUISE.Phone
{
    public enum CallStatus
    {
        Unknown = -1, Idle = 0, Calling, Connecting, Active, Hold, Waiting, Alerting, Busy
    }
    public enum CallType
    {
        Unknown = -1, Voice = 1, Data = 2, Fax = 4, Voice2 = 128
    }

    public class Call
    {
        private int id;
        private CallStatus status;
        private CallType type;
        private String number;

        public int ID
        {
            get
            {
                return id;
            }
        }

        public CallStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        public String Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }
        public CallType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public Call(int id):this(id, CallStatus.Unknown, CallType.Unknown)
        {
        }

        public Call(int id, CallStatus status, CallType type)
        {
            this.id = id;
            Status = status;
            Type = type;
        }
    }
}
