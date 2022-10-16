using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

namespace Socket_Server
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            StartServer();
            return 0;
        }

        public static void StartServer()
        {
            while (true)
            {


                /*Get host ip that is used to establish a connection
                 * In this case, we get one IP of localhost that is IP : 127.0.0.1
                 * If a host has multiple addresses, you will get a list of addresses*/
                IPHostEntry host = Dns.GetHostEntry("192.168.167.191");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5000);

                try
                {
                    //Create a Socket that will use Tcp protocol
                    Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    //A socket must be associated whit an endpoint using the bind method
                    listener.Bind(localEndPoint);
                    //Specify how many requests a Socket can listen before it gives Server busy response.
                    //We will listen 10 requests at a time
                    listener.Listen(10);

                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();

                    // Incoming data from the client.
                    string data = null;
                    byte[] bytes = null;

                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                        if(data.IndexOf("<start>")> -1)
                        {
                            string command = "start Documents\\C#_Projecten\\Socket\\LodexCraft\\bin\\Debug\\net6.0\\LodexCraft.exe";
                            Process.Start("cmd.exe", "/C" + command);
                            
                        }
                        if(data.IndexOf("<reboot>")> -1)
                        {
                            string command = "shutdown -i";
                            Process.Start("cmd.exe", "/C" + command);
                        }
                    }

                    Console.WriteLine("Text reieved : {0}", data);

                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    // handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Console.WriteLine("\n Press any key to continue...");
                Console.ReadKey();
            }        
        }
    }
}