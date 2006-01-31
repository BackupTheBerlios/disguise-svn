// created on 29.12.2005 at 16:44
// by Stefan Tomanek <stefan@pico.ruhr.de>

using DisGUISE;
using DisGUISE.Backend;
using DisGUISE.SEWidgets.Events;
using System.Collections;
using System;

namespace DisGUISE.SEWidgets
{
    public class SEListItem
    {
        private String _label;
        private int _image;
        private bool _deletable;
        private bool _dimmed;
        private bool _selected;

        public String Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }
        public int Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }
        public bool Dimmed
        {
            get
            {
                return _dimmed;
            }
            set
            {
                _dimmed = value;
            }
        }
        public bool Deletable
        {
            get
            {
                return _deletable;
            }
            set
            {
                _deletable = value;
            }
        }
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }

        /*
           The image ID is similar to the ASCII table, most numbers only display an square, but if you reach the printable 
           characters at 32 (space), they are interpreted as ASCII codes

           1    bell icon
           2    speaker icon
           10   new line symbol
           32   nothing (space)
           33   !
           34   "
           35   $
           [...] and the ASCII saga continues...
         */
        public SEListItem(String label, int image, bool dimmed, bool selected, bool deletable)
        {
            this._label = label;
            this._image = image;
            this._dimmed = dimmed;
            this._selected = selected;
            this._deletable = deletable;
        }

        public SEListItem(String label, int image):this(label, image, false, false, false)
        {
        }

        public SEListItem(String label):this(label, 0)
        {
        }

        protected int B2I(bool b)
        {
            return b ? 1 : 0;
        }

        public override String ToString()
        {
            return "\"" + this.Label + "\"," + this.Image + "," + B2I(this.Dimmed) + "," + B2I(this.Selected) + "," + B2I(this.Deletable);
        }

        public virtual void Select(SEWidget widget)
        {

        }

        public virtual void Delete()
        {

        }
    }

    public enum SEListType
    {
        // One-of-Many, Some-of-Many, Data list (Menu)
        OOM = 1, SOM, Data
    }

    public class SEList:SEWidget
    {
        private String title;
        private SEListType type;
        private IList items;

        public SEList(IPhonePort port, String title, SEListType type):base(port, "SELIST")
        {
            this.title = title;
            this.type = type;
            items = new ArrayList();

            this.OnWidgetEvent += new WidgetEventHandler(ProcessWidgetEvent);
        }

        public void AddItem(SEListItem item)
        {
            items.Add(item);
        }

        protected override ATCommand ConstructATCommand()
        {
            String i = "";

            foreach(SEListItem n in items) {
                i = i + "," + n.ToString();
            }

            String cmd = "AT*" + this.cmdbase + "=\"" + title + "\"," + (int) this.type + "," + "0" + "," + items.Count + "," + "0" + "," + "1" + i;
            return (new ATCommand(cmd));
        }

        protected void ProcessWidgetEvent(Object sender, WidgetEventArgs e)
        {
            if (e.Type == SEWidgetEventType.AcceptIndex) {
                int index = int.Parse(e.Payload);
                SEListItem item = (SEListItem) items[index];
                item.Select(this);
            }
        }

    }
}
