using System;

namespace u2b_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var echo = new EchoServer();

            echo.Start();

            Console.WriteLine("Echo Server is running");
            Console.ReadLine();
        }
    }
}
