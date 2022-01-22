using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lesson_02
{
    class RunnableAction
    {
        public event Action<RunnableAction> StartWaiting;
        public event Action<RunnableAction> StartWorking;
        public event Action<RunnableAction> Complete;

        static Semaphore sem = new Semaphore(3, 3);
        static Random rnd = new Random();
        Task task;

        public int Progress { get; private set; } = 0;

        public int? Id => task?.Id;

        public RunnableAction()
        {
            task = new Task(() =>
            {
                StartWaiting?.Invoke(this);
                sem.WaitOne();
                StartWorking?.Invoke(this);

                Thread.Sleep(rnd.Next(3000, 5000));
                while (Progress < 100)
                {
                    Progress++;
                    Thread.Sleep(rnd.Next(30, 50));
                }

                sem.Release();
                Complete?.Invoke(this);
            });
        }

        public void Start()
        {
            task.Start();
        }
    }

    class WorkedActions
    {
        public List<RunnableAction> Actions { get; set; } = new List<RunnableAction>();
        public List<RunnableAction> WaitActions { get; set; } = new List<RunnableAction>();
        public List<RunnableAction> WorkActions { get; set; } = new List<RunnableAction>();
        public List<RunnableAction> CompletedActions { get; set; } = new List<RunnableAction>();

        public void Save(RunnableAction action)
        {
            Actions.Add(action);

            action.StartWaiting += Action_StartWaiting;
            action.StartWorking += Action_StartWorking;
            action.Complete += Action_Complete;
        }

        private void Action_Complete(RunnableAction action)
        {
            WorkActions.Remove(action);
            CompletedActions.Add(action);
        }

        private void Action_StartWorking(RunnableAction action)
        {
            WaitActions.Remove(action);
            WorkActions.Add(action);
        }

        private void Action_StartWaiting(RunnableAction action)
        {
            WaitActions.Add(action);
        }

        public void Print()
        {
            Console.WriteLine("-= COMPLETE actions =-");
            CompletedActions.ForEach(action => ShowAction(action));
            Console.WriteLine("-= --------------------- =-");

            Console.WriteLine("-= WORK actions =-");
            WorkActions.ForEach(action => ShowAction(action));
            Console.WriteLine("-= --------------------- =-");

            Console.WriteLine("-= WAIT actions =-");
            WaitActions.ForEach(action => ShowAction(action));
            Console.WriteLine("-= --------------------- =-");

        }

        private void ShowAction(RunnableAction action)
        {
            Console.WriteLine($"#{action.Id} - {action.Progress}%");
        } 
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            // List<string> items = new List<string>();
            // Task task = new Task(() =>
            // {
            //     Console.WriteLine("Task run");
            //     // Get data from DB
            //     items.AddRange(new string[] { "Item 1", "Item 2", "Item 3" });
            //     Console.WriteLine("Task end");
            // });
            // Console.WriteLine($"Size = {items.Count}");
            // task.Start();
            // Console.WriteLine($"Size = {items.Count}");
            // 
            // task.Wait();
            // Console.WriteLine($"Size = {items.Count}");

            // Demo();

            /**
             * Mutex
             */

            // var mutex = new Mutex();
            // 
            // List<Task> tasks = new List<Task>();
            // int counter = 0;
            // for (int i = 0; i < 15; i++)
            // {
            //     var task = Task.Run(() =>
            //     {
            //         mutex.WaitOne();
            //         var idx = ++counter;
            //         Console.WriteLine($"Start #{idx}");
            //         for (int k = 0; k < 20; k++)
            //         {
            //             Console.WriteLine($"#{idx} - {k}");
            //             Thread.Sleep(1000);
            //         }
            //         mutex.ReleaseMutex();
            //     });
            //     tasks.Add(task);
            // }
            // 
            // Task.WaitAll(tasks.ToArray());

            // var semaphore = new Semaphore(3, 3);
            // List<Task> tasks = new List<Task>();
            // int counter = 0;
            // for (int i = 0; i < 15; i++)
            // {
            //     var task = Task.Run(() =>
            //     {
            //         semaphore.WaitOne();
            //         var idx = ++counter;
            //         Console.WriteLine($"Start #{idx}");
            //         for (int k = 0; k < 20; k++)
            //         {
            //             Console.WriteLine($"#{idx} - {k}");
            //             Thread.Sleep(1000);
            //         }
            //         semaphore.Release();
            //     });
            //     tasks.Add(task);
            // }
            // 
            // Task.WaitAll(tasks.ToArray());

            WorkedActions workedActions = new WorkedActions();
            Random rnd = new Random();
            int max = 50;

            Task.Run(() =>
            {
                while (max > 0)
                {
                    RunnableAction action = new RunnableAction();
                    workedActions.Save(action);
                    action.Start();

                    Thread.Sleep(rnd.Next(300, 1000));
                    max--;
                }
            });

            while (true)
            {
                Console.Clear();

                // Console.WriteLine("[1] Start a new thread");
                // var key = Console.ReadKey().Key;
                // 
                // switch(key)
                // {
                //     case ConsoleKey.D1:
                //         RunnableAction action = new RunnableAction();
                //         workedActions.Save(action);
                //         
                //         break;
                // }

                workedActions.Print();

                Thread.Sleep(100);
            }
        }

        private static async void Demo()
        {
            List<string> items = new List<string>();
            string[] data = await GetData();
            items.AddRange(data);
            Console.WriteLine($"Size = {items.Count}");
        }

        private static async Task<string[]> GetData()
        {
            return await Task.Run(() =>
            {
                return new string[] { "Item 1", "Item 2", "Item 3" };
            });
        }
    }
}
