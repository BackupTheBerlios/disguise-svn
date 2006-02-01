// created on 01.02.2006 at 17:12
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Collections;
using DisGUISE.Backend;

namespace DisGUISE.Phone
{
    /*
      From the SE reference documentation    
      #       35      Hash (number sign)       
      *       42      Star (*)        
      0       48      Number key 0       
      1       48      Number key 1       
      2       48      Number key 2       
      3       48      Number key 3       
      4       48      Number key 4       
      5       48      Number key 5       
      6       48      Number key 6       
      7       48      Number key 7       
      8       48      Number key 8       
      9       48      Number key 9       
      :       58      Escape character for manufacturer specific keys    
      <       60      Left arrow        
      >       62      Right arrow        
      C/c     67/99   Clear display (C/CLR)       
      D/d     68/100  Volume down        
      L/l     76/108  phone lock (LOCK) (If supported by ME)   
      P/p     80/112  Power (PWR)        
      U/u     85/117  Volume up        
      V/v     86/118  Down arrow        
      [       91      Soft key 1       
      ]       93      Soft key 2       
      ^       94      Up arrow        
      :J      58+74   Joystick button pressed       
      :C      58+99   Camera button        
      :O      58+79   Operator button        
      :R      58+82   Return button        
      H/h     200     Button pushed on the MC link (Bluetooth) headset  
      :M      58+77   video call (If supported by ME)    
      :F      58+70   camera focus (camera key half press) (If supported by ME)
      :(      58+40   flip closed (If supported by ME)    
      :)      58+41   flip opened (If supported by ME)    
      :{      58+123  camera lens cover closed (If supported by ME)  
      :}      58+125  camera lens cover opened (If supported by ME)  
      :[      58+91   Jack knife closed (If supported by ME)   
      :]      58+93   Jack knife closed (If supported by ME)   
      :D      58+68   multi task button (shortcut to desktop) (If supported by ME)
      :L      58+76   flash lamp button (If supported by ME)   
      :P      58+80   "Push to talk" button (If supported by ME)  
      :S      58+83   media player button (If supported by ME)   
      :=      58+61   fire (gamepad)        
      :<      58+60   up left (gamepad)       
      :|      58+124  up right (gamepad)       
      :V      58+86   down left (gamepad)       
      :>      58+62   down right (gamepad       
      :1      58+49   Game A (gamepad)       
      :2      58+50   Game B (gamepad)       
      :3      58+51   Game C (gamepad)       
      :4      58+51   Game D (gamepad)       
    */
     
    /// <summary>
    /// The KeyCode class contains all possible key codes generated and sent to the phone. All codes
    /// can be accessed via static members, and incoming strings can be identified by a Lookup method.
    /// </summary> 
    public class KeyCode
    {
        // We keep all instances in a hashmap, so we can easily lookup an instance by its code
        private static Hashtable keymap = new Hashtable();
        
        // That's a lot of code...and boring one as well.
        // Let's hope the member names are self explaining.
        public static readonly KeyCode Hash = new KeyCode("#", "Hash (number sign)");
        public static readonly KeyCode Star = new KeyCode("*", "Star (*)");
        public static readonly KeyCode Number0 = new KeyCode("0", "Number key 0");
        public static readonly KeyCode Number1 = new KeyCode("1", "Number key 1");
        public static readonly KeyCode Number2 = new KeyCode("2", "Number key 2");
        public static readonly KeyCode Number3 = new KeyCode("3", "Number key 3");
        public static readonly KeyCode Number4 = new KeyCode("4", "Number key 4");
        public static readonly KeyCode Number5 = new KeyCode("5", "Number key 5");
        public static readonly KeyCode Number6 = new KeyCode("6", "Number key 6");
        public static readonly KeyCode Number7 = new KeyCode("7", "Number key 7");
        public static readonly KeyCode Number8 = new KeyCode("8", "Number key 8");
        public static readonly KeyCode Number9 = new KeyCode("9", "Number key 9");
        public static readonly KeyCode EscapeCharacter = new KeyCode(":", "Escape character for manufacturer specific keys");
        public static readonly KeyCode LeftArrow = new KeyCode("<", "Left arrow");
        public static readonly KeyCode RightArrow = new KeyCode(">", "Right arrow");
        public static readonly KeyCode ClearDisplay = new KeyCode("C/c", "Clear display (C/CLR)");
        public static readonly KeyCode VolumeDown = new KeyCode("D/d", "Volume down");
        public static readonly KeyCode PhoneLock = new KeyCode("L/l", "phone lock (LOCK) (If supported by ME)");
        public static readonly KeyCode Power = new KeyCode("P/p", "Power (PWR)");
        public static readonly KeyCode VolumeUp = new KeyCode("U/u", "Volume up");
        public static readonly KeyCode DownArrow = new KeyCode("V/v", "Down arrow");
        public static readonly KeyCode SoftKey1 = new KeyCode("[", "Soft key 1");
        public static readonly KeyCode SoftKey2 = new KeyCode("]", "Soft key 2");
        public static readonly KeyCode UpArrow = new KeyCode("^", "Up arrow");
        public static readonly KeyCode JoystickButton = new KeyCode(":J", "Joystick button pressed");
        public static readonly KeyCode CameraButton = new KeyCode(":C", "Camera button");
        public static readonly KeyCode OperatorButton = new KeyCode(":O", "Operator button");
        public static readonly KeyCode ReturnButton = new KeyCode(":R", "Return button");
        public static readonly KeyCode HeadsetButton = new KeyCode("H/h", "Button pushed on the MC link (Bluetooth) headset");
        public static readonly KeyCode VideoCall = new KeyCode(":M", "video call (If supported by ME)");
        public static readonly KeyCode CameraFocus = new KeyCode(":F", "camera focus (camera key half press) (If supported by ME)");
        public static readonly KeyCode FlipClosed = new KeyCode(":(", "flip closed (If supported by ME)");
        public static readonly KeyCode FlipOpened = new KeyCode(":)", "flip opened (If supported by ME)");
        public static readonly KeyCode LensCoverClosed = new KeyCode(":{", "camera lens cover closed (If supported by ME)");
        public static readonly KeyCode LensCoverOpened = new KeyCode(":}", "camera lens cover opened (If supported by ME)");
        public static readonly KeyCode JackKnifeClosed = new KeyCode(":[", "Jack knife closed (If supported by ME)");
        public static readonly KeyCode JackKnifeOpened = new KeyCode(":]", "Jack knife opened (If supported by ME)");
        public static readonly KeyCode MultiTaskButton = new KeyCode(":D", "multi task button (shortcut to desktop) (If supported by ME)");
        public static readonly KeyCode FlashLampButton = new KeyCode(":L", "flash lamp button (If supported by ME)");
        public static readonly KeyCode PushToTalkButton = new KeyCode(":P", "Push to talk button (If supported by ME)");
        public static readonly KeyCode MediaPlayerButton = new KeyCode(":S", "media player button (If supported by ME)");
        public static readonly KeyCode Fire = new KeyCode(":=", "fire (gamepad)");

        // WTF? Seems to be a strange quirk in the documentation
        public static readonly KeyCode GamePadUpLeft = new KeyCode(":<", "up left (gamepad)");
        public static readonly KeyCode GamepadUpRight = new KeyCode(":|", "up right (gamepad)");
        public static readonly KeyCode GamepadDownLeft = new KeyCode(":V", "down left (gamepad)");
        public static readonly KeyCode GamepadDownRight = new KeyCode(":>", "down right (gamepad");
        public static readonly KeyCode GamepadA = new KeyCode(":1", "Game A (gamepad)");
        public static readonly KeyCode GamepadB = new KeyCode(":2", "Game B (gamepad)");
        public static readonly KeyCode GamepadC = new KeyCode(":3", "Game C (gamepad)");
        public static readonly KeyCode GamepadD = new KeyCode(":4", "Game D (gamepad)");
        
        // Each instance holds the code and a description (in case someone gets curious)
        private String _code;
        private String _description;
        
        /// <value>The raw keycode string associated with that key.</value>
        public String Code
        {
            get
            {
                return _code;
            }
        }
        
        /// <value>The descriptive text for the button, really handy if you cannot find it on the phone.</value>
        public String Description
        {
            get
            {
                return _description;
            }
        }
        
        /// <remark>Only the key codestring is compared, the description is ignored.</remark>
        public override bool Equals(Object o)
        {
            if (o == null) return false;
            
            KeyCode c = o as KeyCode;
            if ((Object) c == null) return false;
            
            return c == this;
        }
        
        public override int GetHashCode()
        {
            // This should be enough
            return (this.Code+this.Description).GetHashCode();   
        }
        
        public static bool operator == (KeyCode x, KeyCode y)
        {
            return x.Code.Equals(y.Code);
        }
        
        public static bool operator != (KeyCode x, KeyCode y)
        {
            return ! x.Code.Equals(y.Code);
        }
        
        private KeyCode(String code, String description)
        {
            this._code = code;
            this._description = description;
            lock (keymap) {
                keymap[code] = this;
            }
        }
        
        /// <summary>
        /// This method looks up a <c>KeyCode</c> object based on the key code string
        /// (which might be received from the phone). If no matching keycode is found,
        /// a new object is generated, since the builtin table inside the class might be
        /// incomplete. However, this behaviour might change in future releases, so use
        /// it with caution.
        /// </summary>
        /// <param name="chars">The key code string.</param>
        /// <returns>The <c>KeyCode</c> object belonging to the code string.</returns>
        public static KeyCode Lookup(String chars)
        {
            lock (keymap) {
               if (keymap.ContainsKey(chars)) {
                   return (KeyCode) keymap[chars];    
               } else {
                   // What shall we do? Something strange might be happening, or our
                   // list of possible keycodes is incomplete. We might either return a
                   // new, "custom" keycode, or we simply return null (Maybe throw an exception?)
                   return new KeyCode(chars, "Custom keycode, not defined in database");
               }
            }
        }
    }
}
