using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Text;
using Newtonsoft.Json;
using Microsoft.SqlServer.Server;

namespace Gui.Socksifier
{
    public enum LogLevel
    {
        None = 0,
        Info = 1,
        Deb = 2,
        All = 3,
    }

    public enum ProxyGatewayStatus
    {
        Stopped,
        Connected,
        Disconnected,
        Error
    }

    public enum ProxyGatewayEvent
    {
        Connected,
        Disconnected,
        Message,
        AddressError,
        NdisError,
        Normal = 999
    }

    public enum SupportedProtocolsEnum
    {
        TCP,
        UDP,
        BOTH
    }

    public class LogEntry
    {
        [JsonProperty("time")]
        public long time_stamp { get; set; }
        [JsonProperty("type")]
        public ProxyGatewayEvent tunnel_event { get; set; }
        [JsonProperty("msg")]
        public string description { get; set; }
        [JsonProperty("data")]
        public ulong data { get; set; }
        public LogEntry()
        {
        }
        public LogEntry(long time_stamp, ProxyGatewayEvent e, string description)
        {
            this.time_stamp = time_stamp;
            tunnel_event = e;
            this.description = description;
        }

        public LogEntry(long time_stamp, ProxyGatewayEvent e, ulong data)

        {
            this.time_stamp = time_stamp;
            this.tunnel_event = e;
            this.data = data;
        }

    }

    public class LogEventArgs : EventArgs
    {
        public List<LogEntry> log { get; set; }

        public LogEventArgs(List<LogEntry> log)
        {
            this.log = log;
        }
    }

    public class Socksifier
    {

        public Socksifier(LogLevel log_level)
        {
            log_event = new AutoResetEvent(false);
            SocksifyInterop.socksify_init((int)log_level);
            SocksifyInterop.set_log_event(log_event.SafeWaitHandle.DangerousGetHandle());

            logging_thread = new Thread(new ThreadStart(LogThread));
            logging_thread.Start();
        }

        public Socksifier() { }
        ~Socksifier()
        {
            // Set flag that we are going to exit
            logger_thread_active = false;

            log_event.Set();

            if (logging_thread.IsAlive)
                logging_thread.Join();

            SocksifyInterop.stop();
        }

        event EventHandler<LogEventArgs> LogEvent;
        enum EventMx
        {
            connected,
            disconnected,
            address_error,
            normal,

        };
        private void LogThread()
        {
            // 这里是 log_thread 方法的实现
            // 例如，循环记录日志信息
            do
            {
                log_event.WaitOne(LogEventInterval);

                // Exit if thread was awakened to do it
                if (!logger_thread_active)
                    break;


                var logsJson = SocksifyInterop.read_log(); // Assuming ReadLog() returns a similar structure to the C++ version
                if (logsJson.Length > 0)
                {
                    List<LogEntry> logs = JsonConvert.DeserializeObject<List<LogEntry>>(logsJson);
                    LogEvent?.Invoke(this, new LogEventArgs(logs));
                }

            }
            while (logger_thread_active);

        }

        public bool Start() {
            return SocksifyInterop.start();
        }
        public bool Stop() { 
            return SocksifyInterop.stop();
        }
        public IntPtr AddSocks5Proxy(String endpoint, String username, String password, SupportedProtocolsEnum protocols, bool start)
        {
            return SocksifyInterop.add_socks5_proxy(endpoint, (int)protocols, start, username, password);
        }
        public bool AssociateProcessNameToProxy(String processName, IntPtr proxy)
        {
            return SocksifyInterop.associate_process_name_to_proxy(processName, proxy);
        }

        public Int32 LogEventInterval { get; set; }


        UInt32 LogLimit { get; set; }


        private static Socksifier instance = null;
        private static readonly object padlock = new object();
        public static void Initialize(LogLevel level=LogLevel.All)
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Socksifier(level);
                }
            }
        }
        public static Socksifier Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new Exception("Singleton not initialized.");
                }
                return instance;
            }
        }
        AutoResetEvent log_event { get; set; }
        Thread logging_thread { get; set; }
        volatile bool logger_thread_active = true;

    };

}

