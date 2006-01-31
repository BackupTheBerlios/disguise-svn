// created on 05.01.2006 at 16:20
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Collections;
using System.Text.RegularExpressions;
using DisGUISE.Backend;

namespace DisGUISE.Phone
{
    // This class represents a single entry in the phone book
    public class PhoneBookEntry
    {
        private String name;
        private String number;


        public PhoneBookEntry(String name, String number)
        {
            this.name = name;
            this.number = number;
        }

        public String Name
        {
            get
            {
                return name;
            }
        }
        public String Number
        {
            get
            {
                return number;
            }
        }
    }

    /* This class provides a simple way of accessing the phone book of the cellphone
       Due to a strange bug in the behaviour of the phone, the queries cannot be performed "live"
       in a reliable way (call activity disrupts the retrievel of phone book entries), so the book has to be
       refreshed in regular periods in order to keep the cached information on a recent level. 
     */
    public class PhoneBook:PhoneInteractor
    {
        // With this regular expression we determine the maximal number of entries
        private static Regex reMax = new Regex("^\\+CPBR: \\([0-9]+-([0-9]+)\\),[0-9]+,[0-9]+$");
        /* This regex detects a valid phonebook entry, consisting of:
         * - a unique ID number
         * - the phone number itself (perhaps with + prepended)
         * - the format of the number (145 indicated a number with + attached)
         * - Name and type of the number, seperated by /
         *   H = Home, W = Work etc.
         */
        private static Regex reEntry = new Regex("^\\+CPBR: ([0-9]+),\"([+0-9]*)\",(128|129|145|161),\"(.*)/([HWOMF])\"");

        // Sadly, receiving a call event while looking up the phonebook seriously disrupts the transfer, often several entries are missing
        // Therefore, we implement caching to avoid such lookups during incoming or outgoing calls
        private Hashtable cache;

        public PhoneBook(IPhonePort port):base(port)
        {
            // init the cache
            cache = new Hashtable();
            Refresh();
        }

        // Retrieve a fresh copy of the phone book
        public void Refresh()
        {
            cache = GetBook();
        }

        // Query the cache by number (which is actually a string, due to "+")
        public PhoneBookEntry LookupByNumber(String number)
        {
            // Perhaps we could search with more tolerance in the future
            // by expanding + and prefixes
            foreach(PhoneBookEntry pbe in cache.Values) {
                if (pbe.Number.Equals(number)) {
                    return pbe;
                }
            }
            // Maybe throw an exception here?
            return null;
        }

        // Query the cache by name
        public PhoneBookEntry LookupByName(String name)
        {
            foreach(PhoneBookEntry pbe in cache.Values) {
                if (pbe.Name.Equals(name)) {
                    return pbe;
                }
            }
            return null;
        }

        // Retrieve the phone book from the phone and package it into a hashtble
        private Hashtable GetBook()
        {
            String result = PhonePort.AddCommand(new ATCommand("AT+CPBR=1," + GetMaxIndex()));
            Hashtable rt = new Hashtable();
            foreach(String l in result.Split('\n')) {
                Match m = reEntry.Match(l);

                if (m.Success) {
                    int id = int.Parse(m.Groups[1].Value);
                    String number = m.Groups[2].Value;
                    String name = m.Groups[4].Value;
                    // String type = m.Groups[5].Value;
                    rt[id] = new PhoneBookEntry(name, number);
                }
            }
            return rt;
        }

        // Retrieve the maximum number of phonebook entries
        private int GetMaxIndex()
        {
            String result = PhonePort.AddCommand(new ATCommand("AT+CPBR=?"));
            Match m = reMax.Match(result);
            String max = m.Groups[1].Value;
            return int.Parse(max);
        }

    }

}
