// created on 30.12.2005 at 10:30
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;

namespace DisGUISE.Phone
{
    /// <summary>The status of a call is indicated by this enumeration.</summary>
    public enum CallStatus
    {
        Unknown = -1, Idle = 0, Calling, Connecting, Active, Hold, Waiting, Alerting, Busy
    }
    /// <summary>The type of a call is indicated by this enumeration.</summary>
    public enum CallType
    {
        Unknown = -1, Voice = 1, Data = 2, Fax = 4, Voice2 = 128
    }
    
    /// <summary>
    /// A Call object contains all information about a specific call.
    /// </summary>
    public class Call
    {
        private int id;
        private CallStatus status;
        private CallType type;
        private String number;
        
        /// <value>The internal ID of the call.</value>
        public int ID
        {
            get
            {
                return id;
            }
        }
        
        /// <value>The status of the call.</value>
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
        
        /// <value>The phone number dialed (from), if available.</value>
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
        /// <value>The type of the call.</value>
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
        
        /// <summary>
        /// Create a Call object with still unknown parameters.
        /// Those can be specified later, if more call events arrive.
        /// </summary>
        /// <param name="id">The unique ID of the call handed over from the phone</param>
        public Call(int id):this(id, CallStatus.Unknown, CallType.Unknown)
        {
        }
        
        /// <summary>
        /// Create a Call object with still unknown parameters.
        /// </summary>
        /// <param name="id">The unique ID of the call handed over from the phone.</param>
        /// <param name="status">The current status of the call.</param>
        /// <param name="type">The type of the call.</param>
        public Call(int id, CallStatus status, CallType type)
        {
            this.id = id;
            Status = status;
            Type = type;
        }
    }
}
