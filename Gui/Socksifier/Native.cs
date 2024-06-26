using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using HANDLE = System.IntPtr;
namespace Gui.Socksifier
{
    public class SocksifyInterop
    {

        private const string DllName = "socksify.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern void socksify_init(int level);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr add_socks5_proxy(string endpoint, int protocol, bool start, string login, string password);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool start();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool stop();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool associate_process_name_to_proxy(string processName, IntPtr proxyId);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern void set_log_limit(uint logLimit);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern uint get_log_limit();

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern void set_log_event(IntPtr logEvent);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern string read_log();
    }


}
