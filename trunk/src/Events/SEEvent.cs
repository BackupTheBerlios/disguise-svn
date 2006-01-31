// created on 29.12.2005 at 16:10
using System;

namespace DisGUISE.Events.old
{
    class SERawEvent
    {
        private String _line;
        private IRawEventListener receiver;
        public SERawEvent(String line, IRawEventListener receiver)
        {
            this._line = line;
            this.receiver = receiver;
        }

        public String line
        {
            get
            {
                return _line;
            }
        }

        public void informReceiver()
        {
            this.receiver.EventFired(line);
        }
    }
}
