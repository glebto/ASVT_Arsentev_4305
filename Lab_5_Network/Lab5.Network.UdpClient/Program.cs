// See https://aka.ms/new-console-template for more information
using Lab5.Network.Common;

internal class Program
{
    private static object _locker = new object();

    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");


        var serverAdress = new Uri("udp://127.0.0.1:7777");
        var client = new NetUdpClient(serverAdress);
        Console.WriteLine($"Connect to server at {serverAdress}");

        var messageApi = new MessageApiClient(client);
        await ManageMessages(messageApi);
        client.Dispose();
    }

    private static async Task ManageMessages(IMessageApi messageApi)
    {
        PrintMenu();

        while(true) {
            var key = Console.ReadKey(true);

            PrintMenu();

            if (key.Key == ConsoleKey.D1) 
            {
                Console.Write("Enter id: ");
                var message1 = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter NameCity: ");
                var message2 = Console.ReadLine() ?? string.Empty;
                Console.Write("Enter weather: ");
                var message3 = Console.ReadLine() ?? string.Empty;
                var message = message1 +' '+ message2 +' '+ message3;
                await messageApi.SendMessage(message);
                Console.WriteLine($"Information sent: {message}");
            }
            if (key.Key == ConsoleKey.D2)
            {
                Console.Write("Enter 1 - active, 0 - off: ");
                var key1 = Console.ReadKey(true);
                string message1 = " ";
                if (key1.Key == ConsoleKey.D1)
                    message1 = "Active";
                else if (key1.Key == ConsoleKey.D0)
                    message1 = "Off";
                var message = '_'+message1+'_';
                await messageApi.SendMessage(message);
                Console.WriteLine($"Information sent: {message}");
            }

            if (key.Key == ConsoleKey.Escape)
            {
                break;
            }
        }
        Console.ReadKey();
    }

    private static void PrintMenu()
    {
        lock (_locker)
        {
            Console.WriteLine("1 - Send information about weather");
            Console.WriteLine("2 - Send information about sensor's activity");
            Console.WriteLine("-------");
        }
    }
}