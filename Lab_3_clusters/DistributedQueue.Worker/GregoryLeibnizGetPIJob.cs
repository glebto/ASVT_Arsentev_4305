using System;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedQueue.Common
{
    internal class GregoryLeibnizGetPIJob : IComputePiJob
    {
        public bool isSimple(int N)
        {
            //чтоб убедится простое число или нет достаточно проверить не делитсья ли 
            //число на числа до его половины
            for (int i = 2; i < (int)(N / 2); i++)
            {
                if (N % i == 0)
                    return false;
            }
            return true;
        }
        public Task ComputePyAsync(string name, int iterrations, CancellationToken token)
        {

            var startTime = DateTime.Now;

            //long sum = 1;
            //long a=1;

            //var iterrationsToCheck = 1000;
            //var iterrationCurrent = 0;

            //for (int i = 0; i < iterrations; i++)
            //{
            //    sum = sum + sum;

            //    Console.WriteLine($"{DateTime.Now}: Compute task: {name}, %: {sum}");
            //    Console.WriteLine(sum);
            //}

            int N;//число до которого будем находить простые числа
            N = iterrations;//вводим N
            for (int i = 2; i <= N; i++)
            {
                if (isSimple(i))
                {
                    Console.WriteLine($"{DateTime.Now}: Compute task: {name}, Number: {i}"); ;
                }
            }
        
        //метод который определяет простое число или нет
        

        //Console.WriteLine($"GetSTEPEN: {name}, Iterrations: {iterrations}, CHISLO={a}");

            return Task.CompletedTask;
        }
    }
}
