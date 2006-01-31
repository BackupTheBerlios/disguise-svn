// created on 29.12.2005 at 16:27
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Text.RegularExpressions;
using DisGUISE.Backend;

namespace DisGUISE.SEWidgets
{
    public class SEItem:PhoneInteractor
    {
        protected int id;
        protected String cmdbase;

        public SEItem(IPhonePort port, String cmdbase):base(port)
        {
            this.cmdbase = cmdbase;
        }

        protected static int B2I(bool b)
        {
            return b ? 1 : 0;
        }

        protected int ExtractID(String result)
        {
            Regex re = new Regex("^\\*" + cmdbase + ": ([0-9]+)$");
            foreach(String l in result.Split("\n".ToCharArray())) {
                Match m = re.Match(l);
                if (m.Success) {
                    // Console.WriteLine("My new ID is "+m.Groups[1].ToString());
                    return int.Parse(m.Groups[1].ToString());
                }
            }
            // Throw an Exception?
            return -1;
        }

    }
}
