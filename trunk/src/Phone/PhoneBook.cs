// created on 05.01.2006 at 16:20
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.Collections;
using System.Text.RegularExpressions;
using DisGUISE.Backend;

namespace DisGUISE.Phone
{
    /// <summary>
    /// This class represents a single entry in the phone book.
    /// </summary>
    public class PhoneBookEntry
    {
        private String name;
        private String number;

        /// <summary>Instantiate a new entry.</summary>
        /// <param name="name">The name of the contact.</param>
        /// <param name="number">The phone number of the contact.</param>
        public PhoneBookEntry(String name, String number)
        {
            this.name = name;
            this.number = number;
        }
        
        /// <value>The name contained in the entry.</value>
        public String Name
        {
            get
            {
                return name;
            }
        }
        
        /// <value>The phone number contained in the entry.</value>
        public String Number
        {
            get
            {
                return number;
            }
        }
    }

    /// <summary>
    /// <para>This class provides a simple way of accessing the phone book of the cellphone.</para>
    /// <para>
    /// Due to a strange bug in the behaviour of the phone, the queries cannot be performed "live"
    /// in a reliable way (call activity disrupts the retrievel of phone book entries), so the book has to be
    /// refreshed in regular periods in order to keep the cached information on a recent level.
    /// </para>
    /// </summary>
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
        
        /// <summary>
        /// <para>Create a new instance of this class, and connect it to the supplied phone port.</para>
        /// <para>After initialization, the phone book is retrieved from the phone and cached.</para>
        /// </summary>
        public PhoneBook(IPhonePort port):base(port)
        {
            // init the cache
            cache = new Hashtable();
            Refresh();
        }

        /// <summary>Retrieve a fresh copy of the phone book and store it inside the cache.</summary>
        public void Refresh()
        {
            cache = GetBook();
        }

        /// <summary>Query the cache by phone number.</summary>
        /// <param name="number">The phone number to search for.</param>
        /// <returns>The entry matching the supplied phone number, or, if no match can be found, <c>null</c>.</returns>
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
        
        /// <summary>Query the cache by name.</summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The entry matching the supplied name, or, if no match can be found, <c>null</c>.</returns>
        public PhoneBookEntry LookupByName(String name)
        {
            foreach(PhoneBookEntry pbe in cache.Values) {
                if (pbe.Name.Equals(name)) {
                    return pbe;
                }
            }
            return null;
        }

        // Retrieve the phone book from the phone and package it into a hashtable
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
