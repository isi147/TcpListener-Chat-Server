using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Server
{
    static HashSet<TcpClient> clients = new HashSet<TcpClient>();

    static void Main()
    {
        var ip = IPAddress.Parse("127.0.0.1");
        var port = 27001;
        var listener = new TcpListener(ip, port);
        listener.Start();

        Console.WriteLine("Server is running...");

        while (true)
        {
            var client = listener.AcceptTcpClient();
            Console.WriteLine($"{client.Client.RemoteEndPoint} connected...");
            clients.Add(client);

            Task.Run(() => HandleClient(client));
        }
    }

    static void HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
        var reader = new BinaryReader(stream);

        try
        {
            while (true)
            {
                string message = reader.ReadString();
                Console.WriteLine($"{client.Client.RemoteEndPoint} says: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            client.Close();
            clients.Remove(client);
            Console.WriteLine($"{client.Client.RemoteEndPoint} disconnected...");
        }
    }
}
