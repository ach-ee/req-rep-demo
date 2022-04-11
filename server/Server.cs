using NetMQ;
using NetMQ.Sockets;

class Server : IDisposable
{
    private CancellationTokenSource _cancelSource = new();

    private NetMQQueue<int> _queue = new();

    readonly string[] _clientIds = {"go-client", "rust-client"};

    public Server()
    {
        var token = _cancelSource.Token;

        Task.Factory.StartNew(() => {
            using (var poller = new NetMQPoller{ _queue })
            using (var server = new RouterSocket("@tcp://localhost:5555"))
            {
                server.ReceiveReady += (s, a) => Receive(server.ReceiveMultipartMessage());
                poller.Add(server);

                poller.RunAsync();

                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        poller.Stop();
                        return;
                    }

                    int client = 0;
                    if (_queue.TryDequeue(out client, new TimeSpan(0, 0, 1)))
                    {
                        Send(server, client);
                    }
                }
            }
        }, token,TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public void Dispose()
    {
        _cancelSource.Cancel();
    }

    private void Send(NetMQSocket socket, int client)
    {
        Console.WriteLine($"Sending request to client {_clientIds[client]}");
        var message = new NetMQMessage();
        message.Append(_clientIds[client]);
        message.AppendEmptyFrame();
        message.Append("Hello");

        socket.SendMultipartMessage(message);
    }

    public void SendRequest(int client)
    {
        if (client < 0 || client >= _clientIds.Length)
        {
            Console.WriteLine($"Invalid client {client}");
        }

        _queue.Enqueue(client);
    }

    public void SendLongRequest(int client)
    {
        Console.WriteLine($"Sending long request to client {client}");
    }

    private void Receive(NetMQMessage message)
    {
        Console.WriteLine($"Received: {message[3].ConvertToString()}");
    }
}