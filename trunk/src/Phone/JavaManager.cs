// created on 05.02.2006 at 16:38
// by Stefan Tomanek <stefan@pico.ruhr.de>
using DisGUISE.Backend;
using System.Collections;
using System;
using System.Text.RegularExpressions;

namespace DisGUISE.Phone
{
    /// <summary>
    /// Instances of this class represent java applications installed on the phone.
    /// </summary>
    public class JavaApplication : PhoneInteractor
    {
        private String _name;
        private String _vendor;
        private String _version;
        
        private int _appid;
        
        internal JavaApplication(IPhonePort port, String name, String vendor, String version, int appid) : base(port)
        {
            this._name = name;
            this._vendor = vendor;
            this._version = version;
            this._appid = appid;
        }
        
        /// <value>The name of the application</value>
        public String Name
        {
            get { return _name; }
        }
        /// <value>The version of the application</value>
        public String Version
        {
            get { return _version; }
        }
        /// <value>The vendor of the application</value>
        public String Vendor
        {
            get { return _vendor; }
        }
        
        /// <summary>
        /// This method launches the application on the phone. There is however no
        /// feedback about the launch succeeding or not.
        /// </summary>
        public void Start()
        {
            ATCommand cmd = new ATCommand("AT*EJAVA=4,"+this._appid);
            PhonePort.AddCommand(cmd);
        }
    }
    
    /// <summary>
    /// This class provides access to the Java applications installed on the phone.
    /// </summary>
    public class JavaManager:PhoneInteractor
    {
        private Regex reApp = new Regex("^\\*EJAVA: \"(.*?)\",\"(.*?)\",\"(.*?)\",([0-9]+),([0-9]+)$");
        
        public JavaManager(IPhonePort port):base(port)
        {
        }
        
        /// <summary>
        /// Retrieve a list of all installed Java applications.
        /// </summary>
        /// <returns>
        /// A collection of <c>JavaApplication</c> objects.
        /// </returns>
        public ICollection GetApplications()
        {
            ATCommand cmd = new ATCommand("AT*EJAVA=1");
            String result = PhonePort.AddCommand(cmd);
            
            ArrayList list = new ArrayList();
            
            // A result line looks like this:
            //               name            vendor       ver    id   ??
            // *EJAVA: "Aero Mission 3D","Sony Ericsson","1.1",65539,3
            // The documentation does not tell anything about this either,
            // it does not even mention anything except for name and id
            foreach(String l in result.Split('\n')) {
                Match m = reApp.Match(l);
                if (m.Success) {
                    String name = m.Groups[1].Value;
                    String vendor = m.Groups[2].Value;
                    String version = m.Groups[3].Value;
                    int id = int.Parse(m.Groups[4].Value);
                    JavaApplication app = new JavaApplication(this.PhonePort, name, vendor, version, id);
                    list.Add(app);
                }
            }
            return list;
        }
    }
}