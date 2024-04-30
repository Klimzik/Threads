using System;
using System.Diagnostics;
using System.Threading;

namespace Threads
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int seed = 23;
            int size = 200;
            int numberOfThreads = 4;
            int[,] matrix = new int[size, size];


            matrix = genereateMatrix(size, seed);
            int[,] copyMatrix = (int[,])matrix.Clone();
            int[,] matrixVerified = matrixMultiplicationVerified((int[,])matrix.Clone());

            int numberOfIterations = 20;
            long totalElapsedTime = 0;
            for (int i = 0; i < numberOfIterations; i++)
            {
                int[,] matrix_ = genereateMatrix(size, seed);
                int[,] result = new int[size, size];


                Thread[] th = new Thread[numberOfThreads];
                for (int j = 0; j < numberOfThreads; j++)
                {
                    int p = j;
                    th[j] = new Thread(() => matrixMultiplication(ref matrix, numberOfThreads, p, (int[,])copyMatrix.Clone()));
                    th[j].Name = String.Format("Thread: {0}", j + 1);
                }

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (Thread t in th)
                    t.Start();

                foreach (Thread t in th)
                    t.Join();
                stopwatch.Stop();

                long elapsedTime = stopwatch.ElapsedMilliseconds;
                totalElapsedTime += elapsedTime;

                Console.WriteLine($"Czas wykonania dla próby {i + 1}: {elapsedTime} ms");
            }

            double averageElapsedTime = (double)totalElapsedTime / numberOfIterations;
            Console.WriteLine($"Średni czas wykonania dla {numberOfIterations} prób: {averageElapsedTime} ms");

            /*Thread[] th = new Thread[numberOfThreads];
            for (int i = 0; i < numberOfThreads; i++)
            {
                int p = i;
                th[i] = new Thread(() => matrixMultiplication(ref matrix, numberOfThreads, p, (int[,])copyMatrix.Clone()));
                th[i].Name = String.Format("Thread: {0}", i + 1);
            }
            foreach (Thread t in th)
                t.Start();

            foreach (Thread t in th)
                t.Join();*/

            /*            DisplayMatrix(matrix);
                        Console.WriteLine("\n");
                        DisplayMatrix(matrixVerified);
                        checkIfTheSameMatrix(matrix, matrixVerified);*/


            /*int numberOfIterations = 20;
            long totalElapsedTime = 0;
            for (int i = 0; i < numberOfIterations; i++)
            {
                int[,] matrix_ = genereateMatrix(size, seed);
                int[,] result = new int[size, size];

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                matrixMultiWithParallel(matrix_, result, numberOfThreads);
                stopwatch.Stop();

                long elapsedTime = stopwatch.ElapsedMilliseconds;
                totalElapsedTime += elapsedTime;

                Console.WriteLine($"Czas wykonania dla próby {i + 1}: {elapsedTime} ms");
            }

            // Oblicz średni czas wykonania
            double averageElapsedTime = (double)totalElapsedTime / numberOfIterations;
            Console.WriteLine($"Średni czas wykonania dla {numberOfIterations} prób: {averageElapsedTime} ms");*/




            //Console.WriteLine("Wynikowa macierz:");
            //DisplayMatrix(result);


            //checkIfTheSameMatrix(matrixVerified, result);
        }

        static int[,] genereateMatrix(int size, int seed)
        {
            Random random = new Random(seed);
            int[,] matrix = new int[size, size];
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = random.Next(0, 20);
                }
            }
            return matrix;
        }

        //every matrix is square
        static void matrixMultiplication(ref int[,] matrix, int numberOfThreads, int numberOfThread, int [,] matrix_)
        {
            int size = matrix.GetLength(0);

            int[,] copyMatrix = (int[,])matrix_.Clone();

            int rowsPerThread = size / numberOfThreads;
            int start = rowsPerThread * numberOfThread;
            int end = rowsPerThread * (numberOfThread + 1);

            int temp = 0;

            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp = 0;
                    for (int k = 0; k < size; k++)
                    {
                        temp += copyMatrix[i, k] * copyMatrix[k, j];
                    }
                    matrix[i, j] = temp;
                }
            }
        }

        static void matrixMultiWithParallel(int[,] matrix, int[,] result, int numberOfThreads)
        {
            int size = matrix.GetLength(0);
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = numberOfThreads;

            Parallel.For(0, size, options, i =>
            {
                for (int j = 0; j < size; j++)
                {
                    int temp = 0;
                    for (int k = 0; k < size; k++)
                    {
                        temp += matrix[i, k] * matrix[k, j];
                    }
                    result[i, j] = temp;
                }
            });
        }

        static int[,] matrixMultiplicationVerified(int[,] matrix)
        {
            int size = matrix.GetLength(0);
            int temp = 0;
            int[,] copyMatrix = (int[,])matrix.Clone();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    temp = 0;
                    for (int k = 0; k < size; k++)
                    {
                        temp += copyMatrix[i, k] * copyMatrix[k, j];
                    }
                    matrix[i, j] = temp;
                }
            }
            return matrix;
        }

        static bool checkIfTheSameMatrix(int[,] matrix_1, int[,] matrix_2)
        {
            int rows = matrix_1.GetLength(0);
            int cols = matrix_1.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (matrix_1[i, j] != matrix_2[i, j])
                    {
                        Console.WriteLine("Macierze nie sa takie same");
                        return false;
                    }
                }
            }
            Console.WriteLine("Macierze sa takie same");
            return true;
        }

        static void DisplayMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + "||");
                }
                Console.WriteLine();
            }
        }

    }
}