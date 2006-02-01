// created on 30.12.2005 at 10:38
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Text.RegularExpressions;
using DisGUISE.Backend;
using DisGUISE.Backend.Events;

namespace DisGUISE.Phone
{
    /// <summary>
    /// <para>
    /// This abstract class acts as a framework for classes that wish to listen for
    /// raw events and process them.
    /// </para>
    /// <para>
    /// Many events require prior activation through a
    /// special AT command. This can be achieved through the method <c>ConstructATCommand</c>:
    /// It acts as a factory and should produce the <c>ATCommand</c> object to enable
    /// event transmission.
    /// </para>
    /// <para>
    /// If the reporter does not need initialization, the constructor
    /// parameter <c>needInit</c> should be set to false, and <c>ConstructATCommand</c> can
    /// safely return <c>null</c>.
    /// </para>
    /// </summary>
    public abstract class EventReporter:PhoneInteractor
    {
        private bool needInit;
        
        /// <summary>
        /// Instantiate and prepare the object.
        /// </summary>
        /// <param name="port">The phone port from which to receive events from.</param>
        /// <param name="needInit">Indicates whether the phone needs initialization to report events.</param>
        public EventReporter(IPhonePort port, bool needInit):base(port)
        {
            this.needInit = needInit;
        }
        
        /// <summary>
        /// <para>Construct the AT command necessary for event transmission activation.</para>
        /// <para>If no such command is needed, simply return null.</para>
        /// </summary>
        /// <returns>The <c>ATCommand</c> object needed for event initialization.</returns>
        protected abstract ATCommand ConstructATCommand();
        
        /// <summary>
        /// This method registers the object at the <c>IPhonePort</c> for raw events, as well as transmits the AT command to the phone.
        /// </summary>
        /// <remarks>This method should be only called once for each object!</remarks>
        public void Install()
        {
            this.PhonePort.OnRawEvent += new RawEventHandler(this.ProcessRawEvent);
            if (needInit) {
                ATCommand cmd = ConstructATCommand();
                if (cmd != null)
                {
                    PhonePort.AddCommand(cmd);
                }
            }
        }
        protected abstract void ProcessRawEvent(Object sender, RawEventArgs e);
    }

}
