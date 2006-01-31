// created on 22.01.2006 at 16:10
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;

namespace DisGUISE.Backend.Events
{
    public class RawEventArgs:EventArgs
    {
        private String _line;

        public String Line
        {
            get
            {
                return _line;
            }
        }

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
