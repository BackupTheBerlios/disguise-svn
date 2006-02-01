// created on 01.02.2006 at 19:07
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Collections;
using DisGUISE.Backend;

namespace DisGUISE.Phone
{ 
    /// <summary>Indicates whether a key has been (or should be) pressed, released or both</summary>
    // We should think about turning this into a Flags enumeration
    public enum KeyAction
    {
        Pressed = 0,
        Released,
        Both
    }
    
    /// <summary>
    /// A <c>KeypadSequence</c> transmit a series of key actions to the phone.
    /// The phones reacts to those as if they had occured on the real keypad.
    /// </summary>
    public class KeypadSequence : PhoneInteractor
    {
        private Queue keys;
        
        // Just to hold both values together
        // Perhaps a struct is enough?
        private class KeyOp
        {
            public KeyCode code;
            public KeyAction action;
            
            public KeyOp(KeyCode code, KeyAction action)
            {
                this.code = code;
                this.action = action;
            }
        }
        
        public KeypadSequence(IPhonePort port) : base(port)
        {
            keys = new Queue();
        }
        
        /// <summary>
        /// Add a new keypad activity to the execution queue.
        /// </summary>
        /// <param name="code">The key to be acted upon</param>
        /// <param name="action">The way the button should be dealt with (Press, Release, Both)</param>
        public void AddKeyActivity(KeyCode code, KeyAction action)
        {
            lock (keys) {
                keys.Enqueue(new KeyOp(code, action));
            }
        }
        
        private ATCommand ConstructATCommand()
        {
            String cmd;
            lock (keys) {
                cmd = "AT*EKEY="+keys.Count;
                foreach (KeyOp op in keys) {
                    cmd = cmd + ",\"" + op.code.Code + "\","+(int)op.action;
                }
            }
            return new ATCommand(cmd);
        }
        
        /// <summary>
        /// Clear the execution queue. After this, the clean <c>KeypadSequence</c> can be
        /// used again.
        /// </summary>
        public void Clear()
        {
            lock (keys) {
                keys.Clear();    
            }
        }
        
        /// <summary>
        /// Send the accumulated sequence of key activities to the phone. 
        /// </summary>
        /// <remark>
        /// The queue is not cleared afterwards, it is possible to reuse the
        /// same sequence multiple times
        /// </remark>
        public void Submit()
        {
            ATCommand cmd = ConstructATCommand();
            PhonePort.AddCommand(cmd);
        }
        
    }
}