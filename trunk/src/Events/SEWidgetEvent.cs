// created on 11.01.2006 at 15:03
using System;

namespace DisGUISE.events
{

    public class SERawEventArgs:EventArgs
    {
        private string line;

        public SERawEventArgs(string rawLine):base()
        {
            this.line = rawLine;
        }

        public String RawLine
        {
            get
            {
                return line;
            }
        }
    }

}
