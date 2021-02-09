using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace u2b_Server
{
    public class EchoServer
    {
        public void Start(int port = 9000)
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, port); //loopback - only listening to the 121.0.0.1 -not listening on the network.

            var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(128);

            _ = Task.Run(() => DoEcho(socket));

            //_ or a discard - a placeholder variable that are intentionally unused in application code. Unnassigned variable with no value.
            //Impletation of discard is communicationg intent that the variable will not be used - you will ignore the result. Works in tuples (0.0, _),
            //Can save memory allocation
        }
        private async Task DoEcho(Socket socket)
        {
            do
            {
                //Following code turns aync callback into a Task and returns the actual socket.

                var clientSocket = await Task.Factory.FromAsync(
                    new Func<AsyncCallback, object, IAsyncResult>(socket.BeginAccept), //takes care of connecting client
                    new Func<IAsyncResult, Socket>(socket.EndAccept), //completed connection  
                    null).ConfigureAwait(false);

                Console.WriteLine("ECHO SERVER :: CLIENT CONNECTED");

                using (var stream = new NetworkStream(clientSocket, true))
                {
                    var buffer = new byte[1024];
                    do
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

                        if (bytesRead == 0)
                            break;
                        //The connection is not physical so the client can do a bad disconnection and the server will never know.

                        await stream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                    } while (true);
                }

            } while (true);
        }
    }
}
