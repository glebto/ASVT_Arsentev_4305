using System;
using System.Drawing;
using System.Numerics;
using Lab5.Network.Common;
using Lab5.Network.Common.UserApi;

internal class Program
{
    private static object _locker = new object();

    public static async Task Main(string[] args)
    {
        var serverAdress = new Uri("tcp://127.0.0.1:5555");
        var client = new NetTcpClient(serverAdress);
        Console.WriteLine($"Connect to server at {serverAdress}");
        await client.ConnectAsync();

        var userApi = new UserApiClient(client);
        await ManageUsers(userApi);
        client.Dispose();
    }

    private static async Task ManageUsers(IUserApi userApi)
    {
        //PrintMenu();
        while (true)
        {
            

           //PrintMenu();

            while (true)
            {
                string[] weatherArray = new string[7]; // Создаём массив из 5 элементов

                // Заполняем массив данными о погоде
                weatherArray[0] = "Cloudy";
                weatherArray[1] = "Clear";
                weatherArray[2] = "Variable Cloud";
                weatherArray[3] = "Cloudy";
                weatherArray[4] = "Variable Cloud";
                weatherArray[5] = "Rain";
                weatherArray[6] = "Light rain";

                Console.Clear();
                var users = await userApi.GetAllAsync();
                Console.Clear();
                Console.WriteLine($"| Id    |     NameCity         |      Weather    |");
                foreach (var user in users)
                {
                    Console.WriteLine($"| {user.Id,5} | {user.Name,20} | {user.Active,15} |");
                }
                Thread.Sleep(1000);
                Random random = new Random();

                int CityStatus = random.Next(1, 10);
                int CityStatus11 = random.Next(1, 7);
                Console.WriteLine(CityStatus);
                Console.WriteLine(CityStatus11);
                string CityStatus1 = CityStatus.ToString();
                var userIdString = CityStatus1;
                int.TryParse(userIdString, out var userId);
                var user1 = await userApi.GetAsync(userId);
                var Name = user1?.Name;
                var Active = user1?.Active;

                var addUser = new User(
                    Id: user1.Id,
                    Name: user1?.Name,
                    Age: 30,
                    Active: weatherArray[CityStatus11]
                );
                var addResult = await userApi.UpdateAsync(CityStatus, addUser); 
            }
            
    }

      
    }
}
