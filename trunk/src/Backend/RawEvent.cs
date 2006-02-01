// created on 22.01.2006 at 16:10
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;

namespace DisGUISE.Backend.Events
{
    /// <summary>
    /// Objects of this class encapsulate single unsolicitated result codes. Those lines are transmitted
    /// by the phone everytime something "out of line" occurs, such as an incoming call or user interaction;
    /// Therefore, they are on the lowest level of events.
    /// </summary>
    public class RawEventArgs:EventArgs
    {
        private String _line;
        /// <value>The string sent by the phone</value>
        public String Line
        {
            get
            {
                return _line;
            }
        }
        /// <param name="line">The string sent by the phone</param>
        public RawEventArgs(String line):base()
        {
            this._line = line;
        }
    }

    public delegate void RawEventHandler(Object sender, RawEventArgs e);

    public class TestRawHandler
    {
        public void incomingLine(Object sender, RawEventArgs e)
        {
            Console.WriteLine("«Event» " + e.Line);
        }
    }


}
