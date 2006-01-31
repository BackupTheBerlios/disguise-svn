// created on 30.12.2005 at 10:50
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DisGUISE.Backend
{
    /* Devstream is a fix for the FileStream class for use with character devices:
       Since seeking inside the file is impossible, but the StreamWriter tries to do this
       because of FileStream not detecting this disability, this class sets the CanSeek property
       to false.
     */
    internal class DevStream:FileStream
    {
        public DevStream(String filename, FileAccess access):base(filename, FileMode.Open, access)
        {
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

    }
}
