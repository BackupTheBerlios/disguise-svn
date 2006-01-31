// created on 30.12.2005 at 10:48
// by Stefan Tomanek <stefan@pico.ruhr.de>

using System;
using System.IO;
using System.Threading;
using System.Collections;
using DisGUISE.Backend.Events;
using DisGUISE.SEWidgets;

namespace DisGUISE.Backend
{
    /* This class ensures that every raw event received from the phone
       is propagated in its own thread, since passing the messages without
       threading could lead to deadlocks if new commands are issued from inside
       an event handler.
     */
    internal class RawEventPropagator
    {
        private RawEventHandler handler;
        private RawEventArgs e;
        public RawEventPropagator(RawEventHandler handler, RawEventArgs e)
        {
            this.handler = handler;
            this.e = e;

            ThreadStart threadDelegate = new ThreadStart(this.Run);
            Thread newThread = new Thread(threadDelegate);
              newThread.Start();
        }

        public void Run()
        {
            handler(this, e);
        }
    }

    /* The DeviceFilePort object handles all communication with the phone. It does encapsulate
       the bluetooth connection via the RFCOMM API, and keeps track of issued commands and their
       responses
     */
    public class DeviceFilePort:IPhonePort
    {
        private String filename;
        private FileStream stream;
        private StreamReader sr;

        private bool reading;

        private Thread readThread;
        private Queue commands;

        // Time unttil a new command is sent
        private static int sleepTime = 100;

        public event RawEventHandler OnRawEvent;

        public DeviceFilePort(String filename):base()
        {
            this.filename = filename;
            // Open the device file
            stream = new DevStream(filename, FileAccess.ReadWrite);
            sr = new StreamReader(this.stream);
            Console.WriteLine("Opened device " + this.filename);
            reading = false;
            commands = new Queue();
        }

        public void Start()
        {
            // Launch the reader thread
            reading = true;
            readThread = new Thread(new ThreadStart(ReadPort));
            readThread.Start();

            // Try to bring the phone interface into a clean state
            String r = this.AddCommand(new ATCommand("AT&F"));
            r.Equals("");       // Just to suppress the compiler warning
            //Console.WriteLine("»»" + r + "««");
            // Strange things can happen if we have echos enabled:
            // This might be a bug in the program, but maybe also in the phone
            // (since minicom sometimes shows similar symptoms)
            String r2 = this.AddCommand(new ATCommand("ATE=0"));
            r2.Equals("");      // Just to suppress the compiler warning
            //Console.WriteLine("»»" + r2 + "««");
        }

        public void Stop()
        {
            // SEWidget.cleanUp();
            // The read loop will terminate on the next incoming line
            reading = false;
            lock(sr) {
                // If it is already closed, this will be false
                if (sr.BaseStream.CanRead) {
                    Console.WriteLine("Closing device...");
                    // Let's close the file
                    sr.Close();
                    Console.WriteLine("Device closed");
                    // ...and abort the thread (ugh)
                    readThread.Abort();
                }
            }
            // Be nice to the thread and let him rejoin us.
            readThread.Join();
        }

        /* This method will run in a thread and continously wait for incoming data
         */
        private void ReadPort()
        {
            sr.DiscardBufferedData();
            while (reading) {
                sr.BaseStream.Flush();
                // Retrieve the next line from the phone
                String line = sr.ReadLine();
                if (line == null) {
                    // Maybe we closed the socket? Exit the loop
                    break;
                } else if (!line.Equals("")) {
                    // We don't want to process empty lines
                    ProcessLine(line);
                }
            }
            Console.WriteLine("Stopped reading, closing the device");
            // Perhaps stop() has already closed the stream
            lock(sr) {
                if (stream.CanRead) {
                    sr.Close();
                }
            }
        }

        /* This method takes care of received lines
         */
        private void ProcessLine(String line)
        {
            // If there is a command being processed, the incoming line
            // must belong to him
            if (commands.Count > 0) {
                ATCommand next = (ATCommand) commands.Peek();

                // If we receive an OK, the answer is finished
                if (line.Equals("OK")) {
                    // We wait some time before we submit new commands (Bug?)
                    Thread.Sleep(sleepTime);
                    CommandFinished(true);
                } else if (line.Equals("ERROR")) {
                    // If we get ERROR, there might be something wrong with the command
                    CommandFinished(false);
                } else {
                    // It's just an arbitrary reply belonging to our active command, so
                    // we store the result in the ATCommand object
                    if (next.Transmitted) {
                        next.AddLine(line);
                    }
                }
            } else if (!line.Equals("")) {
                // This is probably unsolicitated result code (Let's hope so)
                if (OnRawEvent != null) {
                    // We create a propagator that will tell the world about our new event in a new thread
                    new RawEventPropagator(OnRawEvent, new RawEventArgs(line));
                }
            }
        }

        /* This method gets called once a command is finished, either closed by OK or ERROR
         * Depending on this, the success parameter will be set accordingly.
         */
        private void CommandFinished(bool success)
        {
            ATCommand active = (ATCommand) commands.Peek();
            if (!success) {
                Console.WriteLine("ERROR (" + active.Command + ")");
            }
            // Mark the command as ready
            active.Ready();
            // remove it from the command queue
            commands.Dequeue();
            // and process the next one
            TransmitNextCommmand();
        }

        public String AddCommand(ATCommand cmd)
        {
            // place the new command into the queue
            commands.Enqueue(cmd);
            // and try to submit it - If there is already a command running, this call has no effect
            TransmitNextCommmand();
            // wait for the command to finish
            cmd.WaitForResult();
            // and extract the result
            String result = cmd.Result;
            return result;
        }

        /* This method transmits the next command in queue to the phone. Before this, it checks whether
           there is already a command in progress - In this case, calling this method is safe and has no
           effect
         */
        private void TransmitNextCommmand()
        {
            // Are there any commands in queue?
            if (commands.Count > 0) {
                // Look at the first one
                ATCommand next = (ATCommand) commands.Peek();
                // Is it already transmitted?
                if (!next.Transmitted) {
                    // Let's go
                    next.Transmitted = true;
                    String cmd = next.Command;

                    StreamWriter sw = new StreamWriter(this.stream);

                    sw.NewLine = "\r";
                    sw.WriteLine(cmd);

                    Console.WriteLine("Transmitting command »" + cmd + "«");
                    // Make sure the command is out
                    sw.Flush();
                }
            }
        }
    }
}
