using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        public static string userName = "Client";
        public static IPHostEntry hostsMachine { get; set; }
        public static IPAddress internetProtocolAddress { get; set; }
        public static IPEndPoint localEndPoint { get; set; }
        public static Socket senderSocket { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "CLIENTTEMP";
           
            Console.WriteLine("What is your username? ");
            userName = Console.ReadLine();

            SocketInitialisation();

            Thread threadReadMessage = new Thread(ReadMessage);
            Thread threadSendMessageToServer = new Thread(SendMessageToServer);

            threadReadMessage.Start();
            threadSendMessageToServer.Start();
        }

        static void SocketInitialisation()
        {
            IPHostEntry hostsMachine = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress internetProtocolAddress = hostsMachine.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(internetProtocolAddress, 1111);

            senderSocket = new Socket(internetProtocolAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                senderSocket.Connect(localEndPoint);
                Console.WriteLine("Connected to server");
            }
            catch
            {
                Console.WriteLine("Could not connect");
            }
        }

        static void SendMessageToServer()
        {
            while (true)
            {
                string messageToSend = null;
                Console.Write("You: ");
                messageToSend = Console.ReadLine();

                if (messageToSend != null)
                {
                    byte[] message = Encoding.ASCII.GetBytes(userName + ": " + messageToSend + "<EOF>");
                    int bytesSent = senderSocket.Send(message);
                }
            }
        }

        static void ReadMessage()
        {
            while (true)
            {
                string data = null;
                byte[] bytes = null;
                while (senderSocket.Connected)
                {
                    bytes = new byte[1024];
                    int bytesRec = senderSocket.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }

                if (data != null)
                {
                    ClearCurrentConsoleLine();
                }
                Console.WriteLine(data);
                Console.Write("You: ");
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}