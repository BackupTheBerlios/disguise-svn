// created on 29.12.2005 at 16:55
using System;
using System.Collections;
using DisGUISE.SEWidgets;
using System.Text.RegularExpressions;

namespace DisGUISE.Events.old
{

    public interface IRawEventListener
    {
        void EventFired(String line);
        int GetUniqueID();
    }

    public interface IFireStarter
    {
        void addAction(Action action);
    }

    public abstract class Action
    {
        public abstract void trigger();
    }
}
