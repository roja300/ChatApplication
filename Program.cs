using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatApplicationServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            
        }

        public static void InitialiseServer()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public static string GetIP()
        {
            try
            {
                return "127.0.0.1";
            }

            catch
            {
                return "127.0.0.1";
            }
        }
    }
}