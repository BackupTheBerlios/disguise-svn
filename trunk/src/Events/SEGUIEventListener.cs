// created on 30.12.2005 at 10:43
using System;
using DisGUISE.SEWidgets;
using System.Text.RegularExpressions;

namespace DisGUISE.Events.old
{
    public enum SEWidgetEventType
    {
        // There are more, these are enough for the beginning
        Cancel = 0, Previous, No, Yes, Accept, AcceptIndex, DeleteIndex, AcceptMany, AcceptDate, AcceptTime, AcceptBoolean, AcceptString, AcceptInteger, SoftKey
    }

    public abstract class SEWidgetEventListener:SEEventListener
    {
      public SEWidgetEventListener():base() {
                }

        public abstract void WidgetEventTriggered(SEWidget sender, String payload);
    }

    public abstract class SEMenuTriggerListener:SEEventListener
    {
        public SEMenuTriggerListener():base()
        {
        }

        public abstract void MenuTriggered(SEAccessoryMenuItem item);
    }

    public abstract class SEWidgetTypeEventListener:SEWidgetEventListener
    {
        private static Regex re = new Regex("^([0-9]+)(?:,(.*?))?$");

        public SEWidgetTypeEventListener():base()
        {
        }

        public override void WidgetEventTriggered(SEWidget sender, String payload)
        {
            Match m = re.Match(payload);
            if (m.Success) {
                SEWidgetEventType type = (SEWidgetEventType) int.Parse(m.Groups[1].Value);
                this.WidgetEventTriggered(sender, type, m.Groups[2].Value);
            }
        }

        public abstract void WidgetEventTriggered(SEWidget sender, SEWidgetEventType type, String payload);
    }


    public abstract class SEGUISingleEventListener:SEWidgetTypeEventListener
    {
        private SEWidgetEventType type;

        public SEWidgetEventType Type
        {
            get
            {
                return type;
            }
        }

        public SEGUISingleEventListener(SEWidgetEventType type):base()
        {
            this.type = type;
        }

        public override void WidgetEventTriggered(SEWidget sender, SEWidgetEventType type, String payload)
        {
            if (type == this.type) {
                this.SingleEventTriggered(sender, payload);
            }
        }

        public abstract void SingleEventTriggered(SEWidget sender, String payload);
    }

    public abstract class SEGUIListSelectListener:SEGUISingleEventListener
    {
        public SEGUIListSelectListener():base(SEWidgetEventType.AcceptIndex)
        {
        }

        public override void SingleEventTriggered(SEWidget sender, String payload)
        {
            int id = int.Parse(payload);
            ListItemSelected(sender, id);
        }

        public abstract void ListItemSelected(SEWidget sender, int index);
    }

    public abstract class SEGUIIntegerAcceptListener:SEGUISingleEventListener
    {
        public SEGUIIntegerAcceptListener():base(SEWidgetEventType.AcceptInteger)
        {
        }

        public override void SingleEventTriggered(SEWidget sender, String payload)
        {
            int val = int.Parse(payload);
            IntegerAccepted(sender, val);
        }

        public abstract void IntegerAccepted(SEWidget sender, int val);
    }
}
