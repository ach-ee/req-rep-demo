bool showMenu = true;

Server server = new();

while (showMenu)
{
    showMenu = MainMenu();
}

bool MainMenu()
{
    Console.Clear();
    Console.WriteLine("Available options:");
    Console.WriteLine("1. Send request to client A");
    Console.WriteLine("2. Send request to client B");
    Console.WriteLine("3. Send long request to client A");
    Console.WriteLine("4. Exit");
    Console.WriteLine("\r\nChoose an option: ");

    switch (Console.ReadLine())
    {
        case "1":
            server.SendRequest("A");
            break;
        case "2":
            server.SendRequest("A");
            break;
        case "3":
            server.SendLongRequest("A");
            break;
        case "4":
            Console.WriteLine("Exiting. Goodbye...");
            return false;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
    return true;
}