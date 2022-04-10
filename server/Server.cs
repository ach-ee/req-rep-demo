class Server 
{
    public void SendRequest(string client)
    {
        Console.WriteLine($"Sending request to client {client}");
    }

    public void SendLongRequest(string client)
    {
        Console.WriteLine($"Sending long request to client {client}");
    }
}