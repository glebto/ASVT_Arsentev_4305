    using MPI;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();
            var sw = new Stopwatch();
            MPI.Environment.Run(ref args, communicator =>
            {
                int rank = communicator.Rank;
                int size = communicator.Size;
                int rA = 10; // Количество строк в первой матрице
                int cA = 10; // Количество столбцов в первой матрице (и строк в векторе)
                int[] mA = new int[rA * cA];
                int[] vB = new int[cA];
                int[] vC = new int[rA];

                if (rank == 0)
                {
                    sw.Start();
                    for (int i = 0; i < rA; i++)
                    {
                        for (int j = 0; j < cA; j++)
                        {
                            mA[i * cA + j] = rand.Next(10); // Заполнение на рандом от 0 до 10
                        }
                    }

                    for (int i = 0; i < cA; i++)
                    {
                           vB[i] = rand.Next(10); // Заполнение на рандом от 0 до 10
                    }

                    for (int i = 1; i < size; i++)
                    {
                        foreach (int item in mA) // Отправка данных другим процессам
                        {
                            communicator.Send(item, i, 0);
                        }

                        foreach (int item in vB)
                        {
                            communicator.Send(item, i, 1);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < rA; i++) // Получение данных другими процессами
                    {
                        for (int j = 0; j < cA; j++)
                        {
                            mA[i * cA + j] = communicator.Receive<int>(0, 0);
                        }
                    }
                    for (int i = 0; i < cA; i++)
                    {
                            vB[i] = communicator.Receive<int>(0, 1);
                    }
                }

                int rowsPerProcess = rA / size;
                int startRow = rank * rowsPerProcess;
                int endRow = (rank + 1) * rowsPerProcess;

                for (int i = startRow; i < endRow; i++) // Перемножение определенных за процесом строк
                {
                        vC[i] = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            vC[i] += mA[i * cA + k] * vB[k];
                        }
                }

                if (rank == 0) // Получение данных главным процессом
                {
                    for (int i = 1; i < size; i++)
                    {
                        for (int row = i * rowsPerProcess; row < (i + 1) * rowsPerProcess; row++)
                        {
                            for (int col = 0; col < 1; col++)
                            {
                                vC[row + col] = communicator.Receive<int>(i, 2);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = startRow; i < endRow; i++) // Отправка данных другими процессами
                    {
                            communicator.Send(vC[i], 0, 2);
                    }
                }

                if (rank == 0)
                {
                    Console.WriteLine("Матрица:"); // Вывод на экран для пользователя
                    for (int i = 0; i < rA; i++)
                    {
                        for (int j = 0; j < cA; j++)
                        {
                            Console.Write(mA[i + j] + "\t");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("Вектор:");
                    for (int i = 0; i < cA; i++)
                    {
                            Console.Write(vB[i] + "\t");
                            Console.WriteLine();
                    }
                    Console.WriteLine("Результирующая матрица:");
                    for (int i = 0; i < rA; i++)
                    {
                            Console.Write(vC[i] + "\t");
                            Console.WriteLine();
                    }
                    Console.WriteLine($"Elapsed {sw.Elapsed}.");
                }
            
            });
        }
    }
