// created on 05.01.2006 at 16:23
// by Stefan Tomanek <stefan@pico.ruhr.de>


namespace DisGUISE.Backend
{
    /// <summary>
    /// This abstract class acts as a base for all other classes that want
    /// to contact the phone via its port object.
    /// </summary>
    public abstract class PhoneInteractor
    {
        private IPhonePort port;
        /// <param name="port">The port communication should be established with.</param>
        public PhoneInteractor(IPhonePort port)
        {
            this.port = port;
        }
        
        /// <value>The port communication can be established with.</value>
        public IPhonePort PhonePort
        {
            get
            {
                return this.port;
            }
        }
    }

}
