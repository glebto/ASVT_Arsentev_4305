// See https://aka.ms/new-console-template for more information
using Lab5.Network.Common;

internal class MessageUdpServer : UdpServerBase, IMessageApi
{
    public MessageUdpServer(Uri listenAddress) : base(listenAddress)
    {
    }

    public Task<bool> SendMessage(string message)
    {
        if (message[0] == '_')
            Console.WriteLine($"New information about sensor's activity: {message}");
        else
        {
            Console.WriteLine($"New information about weather: {message}");
            string[] words = message.Split(' ');
            Console.WriteLine($"City id: {words[0]}");
            Console.WriteLine($"NameCity: {words[1]}");
            Console.WriteLine($"Weather: {words[2]}");
        }
        return Task.FromResult(true);
    }

    protected override async Task ProcessCommandAsync(Command command)
    {
        var commandCode = (CommandCode)command!.Code;
        Console.WriteLine($"+ command: {commandCode}");

        await SendMessage(command.Arguments["Data"]?.ToString() ?? string.Empty);
        //await SendMessage1(command.Arguments["Data"]?.ToString() ?? string.Empty);
    }
}