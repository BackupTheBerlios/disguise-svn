// created on 30.12.2005 at 10:24
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Collections;
using DisGUISE.SEWidgets.Events;
using DisGUISE.Backend;
using DisGUISE.Backend.Events;

namespace DisGUISE.SEWidgets
{

    public class SEAccessoryMenuItem:SEItem
    {
        public enum AccessoryMenuCategory
        {
            Connectivity = 0, Bluetooth, Entertainment, Messaging, Organizer, SettingsGeneral, SettingsSounds, SettingsDisplay, SettingsCalls, Multimedia, Imaging, Phonebook, Application, Accessories
        }

        private String name;
        private AccessoryMenuCategory category;

        public event AccessoryMenuEventHandler OnSelect;

        public SEAccessoryMenuItem(IPhonePort port, String name, AccessoryMenuCategory category):base(port, "SEAM")
        {
            this.name = name;
            this.category = category;
            construct("");
        }

        public void construct(String prefix)
        {
            String r = PhonePort.AddCommand(new ATCommand("AT*" + this.cmdbase + "=\"" + this.name + prefix + "\"," + (int) this.category));
            // Console.WriteLine ("»» " + r + " ««");
            this.id = ExtractID(r);
            // port.addListener (this);
            PhonePort.OnRawEvent += new RawEventHandler(this.ProcessRawLine);
        }

        protected void ProcessRawLine(Object sender, RawEventArgs e)
        {
            if (e.Line.Equals("*SEAAI: " + this.id)) {
                Console.WriteLine("Menu item »" + name + "« has been triggered via delegate");
                if (OnSelect != null)
                    this.OnSelect(this, new AccessoryMenuEventArgs(this));
            }
        }

        public void Destruct()
        {
            this.name = "";
            construct("");
        }

    }

    namespace Events
    {
        public class AccessoryMenuEventArgs:EventArgs
        {
            private SEAccessoryMenuItem _item;

            public SEAccessoryMenuItem Item
            {
                get
                {
                    return _item;
                }
            }

            public AccessoryMenuEventArgs(SEAccessoryMenuItem item):base()
            {
                this._item = item;
            }
        }

        public delegate void AccessoryMenuEventHandler(Object sender, AccessoryMenuEventArgs e);
    }

}
