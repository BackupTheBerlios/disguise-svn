// created on 30.12.2005 at 10:18
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using DisGUISE.SEWidgets.Events;
using DisGUISE.Backend;

namespace DisGUISE.SEWidgets
{

    public class SEDateInput:SEWidget
    {
        public enum SEDateInputMode
        {
            Date = 1, Time = 2
        }

        private String title;
        private SEDateInputMode mode;
        private DateTime time;

        public SEDateInput(IPhonePort port, String title, SEDateInputMode mode, DateTime time):base(port, "SEDATE")
        {
            this.title = title;
            this.mode = mode;
            this.time = time;

            this.OnWidgetEvent += new WidgetEventHandler(ProcessWidgetEvent);
        }

        protected override ATCommand ConstructATCommand()
        {
            String date = String.Format("{0:yyyy}/{0:MM}/{0:dd}", time);
            String clock = String.Format("{0:HH}:{0:mm}:{0:ss}", time);
              return new ATCommand("AT*" + this.cmdbase + "=\"" + title + "\"," + (int) mode + ",1,0,\"" + date + "\",\"" + clock + "\"");
        }

        protected void ProcessWidgetEvent(Object sender, WidgetEventArgs e)
        {
            Console.WriteLine("Type is " + e.Type);
            Console.WriteLine("Payload is " + e.Payload);
        }
    }
}
