using NetMQ;
using NetMQ.Sockets;

class Server 
{
    private RouterSocket _socket = new RouterSocket("@tcp://localhost:5555");

    readonly string[] _clientIds = {"go-client", "rust-client"};

    public void SendRequest(int client)
    {
        if (client < 0 || client >= _clientIds.Length)
        {
            Console.WriteLine($"Invalid client {client}");
        }

        Console.WriteLine($"Sending request to client {_clientIds[client]}");
        var message = new NetMQMessage();
        message.Append(_clientIds[client]);
        message.AppendEmptyFrame();
        message.Append("Hello");

        _socket.SendMultipartMessage(message);

        var receivedMessage = _socket.ReceiveMultipartMessage();

        Console.WriteLine($"Received: {receivedMessage[3].ConvertToString()}");
    }

    public void SendLongRequest(int client)
    {
        Console.WriteLine($"Sending long request to client {client}");
    }
}