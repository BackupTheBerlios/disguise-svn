// created on 05.01.2006 at 16:23
// by Stefan Tomanek <stefan@pico.ruhr.de>


namespace DisGUISE.Backend
{
    /* This abstract class acts as a base for all other classes that want
       to contact the phone via its port object
     */

    public abstract class PhoneInteractor
    {
        private IPhonePort port;
        public PhoneInteractor(IPhonePort port)
        {
            this.port = port;
        }

        public IPhonePort PhonePort
        {
            get
            {
                return this.port;
            }
        }
    }

}
