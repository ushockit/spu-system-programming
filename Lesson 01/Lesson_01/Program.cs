using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lesson_01
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Main start");
            /*Task task = new Task(() =>
            {
                Console.WriteLine("Task start");

                for(int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Value = {i + 1}");
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Task end");
            });
            task.Start();*/
            /*Task task = Task.Run(() =>
            {
                Console.WriteLine("Task start");

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Value = {i + 1}");
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Task end");
            });
            task.Wait(3000);

            for (int k = 0; k < 30; k++)
            {
                Console.WriteLine($"Main loop value = {k}");
                Thread.Sleep(500);
            }*/


            // Вложенные задачи
            // Ожидание дочерних задач
            /*Task parentTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Start parent task");

                Task childTask = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Start child task");


                    for(int k = 0; k < 10; k++)
                    {
                        Console.WriteLine($"Child task = {k}");
                        Thread.Sleep(100);
                    }

                    Console.WriteLine("Start end task");
                }, TaskCreationOptions.AttachedToParent);

                Console.WriteLine("End parent task");
            });


            parentTask.Wait();*/


            var task = Task.Run(() =>
            {
                int sum = 0;
                for(int i = 0; i < 10; i++)
                {
                    sum += i;
                    Thread.Sleep(500);
                }
                return sum;
            });
            // int res = task.Result;


            Console.WriteLine("Main end");
            Console.WriteLine("Hello World!");
        }
    }
}
