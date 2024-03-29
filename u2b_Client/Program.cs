﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace u2b_Client
{
    public class MyMessage
    {
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }
    class Program
    {

        private static int Port = 9000;

        static async Task Main(string[] args)
        {


            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();

            var endpoint = new IPEndPoint(IPAddress.Loopback, Port);
            var socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endpoint);
            var networkStream = new NetworkStream(socket, true);

            string GET = File.ReadAllText(@"C:\Users\axels\Google Drive\Code\VStudio_source\repos\u2b_Server-Client\u2b_Client\GET_Request.txt");

            //var myMessage = new MyMessage
            //{
            //    IntProperty = 404,
            //    StringProperty = "Hello World"
            //};

            Console.WriteLine("Sending");
            //Print(myMessage);
            var bytesToSend = Encoding.UTF8.GetBytes(GET);
            await networkStream.WriteAsync(bytesToSend, 0, bytesToSend.Length).ConfigureAwait(false);

            var buffer = new byte[16384];
            var bytesReceived = await networkStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            var messageReceived = Encoding.UTF8.GetString(buffer);
            //var protocol = new XmlMessageProtocol();
            //var protocol = new JsonMessageProtocol();
            //await protocol.SendAsync(networkStream, myMessage).ConfigureAwait(false);

            //await SendAsync(networkStream, myMessage).ConfigureAwait(false);
            //var responseMsg = await ReceiveAsync<MyMessage>(networkStream).ConfigureAwait(false);

            //var responseMsg = await protocol.ReceiveAsync(networkStream).ConfigureAwait(false);
            //var response = Convert(responseMsg);

            Console.WriteLine("Received:\n\n" + messageReceived);
            //Console.WriteLine(responsMsg.ToString());
            //Print(response);

            Console.ReadLine();
        }

        static MyMessage Convert(JObject jObject)
            => jObject.ToObject(typeof(MyMessage)) as MyMessage;
        static MyMessage Convert(XDocument xmlDocument)
            => new XmlSerializer(typeof(MyMessage)).Deserialize(new StringReader(xmlDocument.ToString())) as MyMessage;

        static void Print(MyMessage m) => Console.WriteLine($"MyMessage.IntProperty = {m.IntProperty}, MyMessage.StringProperty = {m.StringProperty}");
    }
}
