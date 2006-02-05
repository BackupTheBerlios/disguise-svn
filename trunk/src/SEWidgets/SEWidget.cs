// created on 30.12.2005 at 10:27
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Collections;
using System.Text.RegularExpressions;
using DisGUISE.Phone;
using DisGUISE.SEWidgets.Events;
using DisGUISE.Backend;
using DisGUISE.Backend.Events;

namespace DisGUISE.SEWidgets
{

    public abstract class SEWidget:SEItem
    {
        private bool visible;
        private bool autodestruct;
        private static Regex reGUI = new Regex("^\\*SEGUII: ([0-9]+),([0-9]+)(?:,(.*))$");

        public event WidgetEventHandler OnWidgetEvent;

        // We keep track of all widgets to end the UI session once all are destroyed
        private static Hashtable widgetList = new Hashtable();

        public bool Visible
        {
            get
            {
                return visible;
            }
        }
        public bool AutoDestruct
        {
            get
            {
                return autodestruct;
            }
            set
            {
                autodestruct = value;
            }
        }

        public SEWidget(IPhonePort port, String cmdbase):base(port, cmdbase)
        {
            autodestruct = true;
            port.OnRawEvent += new RawEventHandler(this.ProcessRawEvent);
        }

        protected void ProcessRawEvent(Object sender, RawEventArgs e)
        {
            Match m = reGUI.Match(e.Line);
            if (m.Success && m.Groups[1].Value.Equals(this.id.ToString())) {
                SEWidgetEventType type = (SEWidgetEventType) int.Parse(m.Groups[2].Value);
                String payload = m.Groups[3].Value;

                // Trigger new events
                // Console.WriteLine("EVENT! »"+e.Line+"«");

                WidgetEventArgs args = new WidgetEventArgs(this, type, payload);

                if (OnWidgetEvent != null)
                    OnWidgetEvent(this, args);

                if (AutoDestruct)
                    this.Destroy();
            }
        }

        protected abstract ATCommand ConstructATCommand();

        private void DestroySession()
        {
            Console.WriteLine("There are " + widgetList.Count + " SEWidgets alive");
            if (widgetList.Count == 0) {
                // All widgets are destroyed, we terminate the session
                PhonePort.AddCommand(new ATCommand("AT*SEUIS=0"));
            }
        }

        private void CreateSession()
        {
            lock(widgetList) {
                widgetList.Add(this, this);
                Console.WriteLine("There are " + widgetList.Count + " SEWidgets alive");
                if (widgetList.Count == 1) {
                    PhonePort.AddCommand(new ATCommand("AT*SEUIS=1"));
                }
            }
        }

        public virtual void Show()
        {
            if (!Visible) {
                CreateSession();
                String r = PhonePort.AddCommand(ConstructATCommand());
                this.id = ExtractID(r);
                visible = true;
            }
        }

        public virtual void Destroy()
        {
            lock(widgetList) {
                widgetList.Remove(this);

                Console.WriteLine("Destroy() has been called for item " + this.id);
                PhonePort.AddCommand(new ATCommand("AT*SEDEL=" + this.id));
                visible = false;
                Console.WriteLine("Destroying session");
                DestroySession();
            }
        }

    }

    namespace Events
    {
        public enum SEWidgetEventType
        {
            // There are more, these are enough for the beginning
            Cancel = 0, Previous, No, Yes, Accept, AcceptIndex, DeleteIndex, AcceptMany, AcceptDate, AcceptTime, AcceptBoolean, AcceptString, AcceptInteger, SoftKey
        }

        public class WidgetEventArgs:EventArgs
        {
            private SEWidget _widget;
            private SEWidgetEventType _type;
            private String _payload;
            public SEWidget Widget
            {
                get
                {
                    return _widget;
                }
            }
            public SEWidgetEventType Type
            {
                get
                {
                    return _type;
                }
            }
            public String Payload
            {
                get
                {
                    return _payload;
                }
            }

            public WidgetEventArgs(SEWidget widget, SEWidgetEventType type, String payload)
            {
                this._widget = widget;
                this._payload = payload;
                this._type = type;
            }
        }

        public delegate void WidgetEventHandler(Object sender, WidgetEventArgs e);
    }
}
